using MedicalCenter.Api.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCenter.Api.Services
{
    public interface IUserService
    {
        Task<IActionResult> AddUserAsync(IRegisterRequest registration, string roleName);
    }
}
