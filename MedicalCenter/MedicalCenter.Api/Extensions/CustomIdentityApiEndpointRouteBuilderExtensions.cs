using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using FluentValidation;
using MedicalCenter.Domain;
using MedicalCenter.Api.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

namespace MedicalCenter.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
    /// </summary>
    public static class CustomIdentityApiEndpointRouteBuilderExtensions
    {
        // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();

        /// <summary>
        /// Add endpoints for registering, logging in, and logging out using ASP.NET Core Identity.
        /// </summary>
        /// <typeparam name="TUser">The type describing the user. This should match the generic parameter in <see cref="UserManager{TUser}"/>.</typeparam>
        /// <param name="endpoints">
        /// The <see cref="IEndpointRouteBuilder"/> to add the identity endpoints to.
        /// Call <see cref="EndpointRouteBuilderExtensions.MapGroup(IEndpointRouteBuilder, string)"/> to add a prefix to all the endpoints.
        /// </param>
        /// <returns>An <see cref="IEndpointConventionBuilder"/> to further customize the added endpoints.</returns>
        public static IEndpointConventionBuilder MapCustomIdentityApi<TUser>(this IEndpointRouteBuilder endpoints)
            where TUser : AppUser, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.

            var routeGroup = endpoints.MapGroup("");

            // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
            // https://github.com/dotnet/aspnetcore/issues/47338
            routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] RegisterPatientRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                const string RoleName = "patient";

                var userManager = sp.GetRequiredService<UserManager<TUser>>();
                var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
                var registerPatientRequestValidator = sp.GetRequiredService<IValidator<RegisterPatientRequest>>();

                var validationResult = await registerPatientRequestValidator.ValidateAsync(registration);

                if (!validationResult.IsValid)
                {
                    var sb = new StringBuilder();

                    foreach (var failure in validationResult.Errors)
                    {
                        sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                    }

                    return CreateValidationProblem("400", sb.ToString());
                }

                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($"{nameof(MapCustomIdentityApi)} requires a user store with email support.");
                }

                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var emailStore = (IUserEmailStore<TUser>)userStore;
                var email = registration.Email;

                if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                {
                    return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
                }

                var user = new TUser();
                await userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, email, CancellationToken.None);

                user.FirstName = registration.FirstName;
                user.LastName = registration.LastName;                

                await userStore.UpdateAsync(user, CancellationToken.None);

                var result = await userManager.CreateAsync(user, registration.Password);

                if (!result.Succeeded)
                {
                    return CreateValidationProblem(result);
                }

                var isRoleExists = await roleManager.RoleExistsAsync(RoleName);

                if (!isRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleName));
                }

                await userManager.AddToRoleAsync(user, RoleName);

                return TypedResults.Ok();
            });

            routeGroup.MapPost("/login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
                ([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                var loginRequestValidator = sp.GetRequiredService<IValidator<LoginRequest>>();

                var validationResult = await loginRequestValidator.ValidateAsync(login);

                if (!validationResult.IsValid)
                {
                    var sb = new StringBuilder();

                    foreach (var failure in validationResult.Errors)
                    {
                        sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                    }

                    return TypedResults.Problem(sb.ToString(), statusCode: StatusCodes.Status400BadRequest);
                }

                var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
                var isPersistent = (useCookies == true) && (useSessionCookies != true);
                signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

                if (result.RequiresTwoFactor)
                {
                    if (!string.IsNullOrEmpty(login.TwoFactorCode))
                    {
                        result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                    }
                    else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                    {
                        result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                    }
                }

                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }

                // The signInManager already produced the needed response in the form of a cookie or bearer token.
                return TypedResults.Empty;
            });

            return new IdentityEndpointsConventionBuilder(routeGroup);
        }

        private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
            TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
            });

        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }

        private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
            where TUser : class
        {
            return new()
            {
                Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
                IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            };
        }

        // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
        private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
        {
            private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

            public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
            public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
        {
            public string? Name => null;
        }
    }
}

