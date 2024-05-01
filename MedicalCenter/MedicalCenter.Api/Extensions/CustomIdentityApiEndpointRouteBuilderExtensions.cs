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
using MedicalCenter.Data.Repositories.Interfaces;
using MedicalCenter.Api.Models.Interfaces;

namespace MedicalCenter.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add identity endpoints.
    /// </summary>
    public static class CustomIdentityApiEndpointRouteBuilderExtensions
    {
        // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();

        const string IdValidationFailMessage = "The Id is missing or too long!";
        const string UserNotFoundMessage = "The user is not found!";
        const string BadRequestStatusCode = "400";
        const string NotFoundStatusCode = "404";
        const int UserIdMaxLength = 450;

        public static IEndpointConventionBuilder MapCustomIdentityApi<TUser>(this IEndpointRouteBuilder endpoints)
            where TUser : AppUser, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.

            var routeGroup = endpoints.MapGroup("");

            // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
            // https://github.com/dotnet/aspnetcore/issues/47338
            routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] RegisterPatientRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                const string RoleName = "patient";
                var registerPatientRequestValidator = sp.GetRequiredService<IValidator<RegisterPatientRequest>>();

                var validationResult = await registerPatientRequestValidator.ValidateAsync(registration);

                if (!validationResult.IsValid)
                {
                    var sb = new StringBuilder();

                    foreach (var failure in validationResult.Errors)
                    {
                        sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                    }

                    return CreateValidationProblem(BadRequestStatusCode, sb.ToString());
                }

                return await AddUserAsync<TUser>(registration, sp, RoleName);
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

            // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
            // https://github.com/dotnet/aspnetcore/issues/47338
            routeGroup.MapPost("/register-doctor", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] RegisterDoctorRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                const string RoleName = "doctor";

                var registerDoctorRequestValidator = sp.GetRequiredService<IValidator<RegisterDoctorRequest>>();

                var validationResult = await registerDoctorRequestValidator.ValidateAsync(registration);

                if (!validationResult.IsValid)
                {
                    var sb = new StringBuilder();

                    foreach (var failure in validationResult.Errors)
                    {
                        sb.Append($"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage} ");
                    }

                    return CreateValidationProblem(BadRequestStatusCode, sb.ToString());
                }

                return await AddUserAsync<TUser>(registration, sp, RoleName);
            }).RequireAuthorization("admin");

            routeGroup.MapDelete("/delete-doctor", async Task<Results<NoContent, ValidationProblem>>
                ([FromBody] DeleteDoctorRequest deleteRequest, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                if (string.IsNullOrEmpty(deleteRequest.Id) || deleteRequest.Id.Length > UserIdMaxLength)
                {
                    return CreateValidationProblem(BadRequestStatusCode, IdValidationFailMessage);
                }

                var userManager = sp.GetRequiredService<UserManager<TUser>>();

                var user = await userManager.FindByIdAsync(deleteRequest.Id);

                if (user is null)
                {
                    return CreateValidationProblem(NotFoundStatusCode, UserNotFoundMessage);
                }

                var result = await userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return CreateValidationProblem(result);
                }

                return TypedResults.NoContent();
            }).RequireAuthorization("admin");

            routeGroup.MapGet("/check-admin", () => TypedResults.Ok())
                .RequireAuthorization("admin");

            routeGroup.MapPost("/logout", async Task<Results<Ok, UnauthorizedHttpResult>>
                ([FromBody] object empty, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetService<SignInManager<TUser>>();

                if (empty != null)
                {
                    await signInManager!.SignOutAsync();
                    return TypedResults.Ok();
                }

                return TypedResults.Unauthorized();

            }).RequireAuthorization();

            return new IdentityEndpointsConventionBuilder(routeGroup);
        }

        private static async Task<Results<Ok, ValidationProblem>> AddUserAsync<TUser>(
            IRegisterRequest registration, IServiceProvider sp, string roleName)
            where TUser : AppUser, new()
        {
            var userManager = sp.GetRequiredService<UserManager<TUser>>();
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

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

            var isRoleExists = await roleManager.RoleExistsAsync(roleName);

            if (!isRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await userManager.AddToRoleAsync(user, roleName);

            if (roleName == "doctor")
            {
                var doctorInfo = new DoctorInfo();

                var doctorRegistration = (RegisterDoctorRequest)registration;

                doctorInfo.AppUserId = user.Id;
                doctorInfo.PracticeStartDate = doctorRegistration.PracticeStartDate;
                doctorInfo.Specialization = doctorRegistration.Specialization;

                var doctorRepository = sp.GetRequiredService<IDoctorRepository>();

                await doctorRepository.AddAsync(doctorInfo);

                return TypedResults.Ok();
            }
            else
            {
                return TypedResults.Ok();
            }
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

