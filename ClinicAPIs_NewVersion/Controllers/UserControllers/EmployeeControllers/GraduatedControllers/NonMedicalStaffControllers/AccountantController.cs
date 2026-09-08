using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.AccountantDTO;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly IAccountantService _accountantService;

        public AccountantController(IAccountantService accountantService)
        {
            _accountantService = accountantService;
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateAccountant(
            [FromBody] CreateAccountantDto createAccountantDto
            )
        {
            var accountant =
                await _accountantService.CreateAccountantAsync(
                    createAccountantDto
                    );

            return Ok(accountant);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccountants()
        {
            var accountants =
                await _accountantService.GetAllAccountsAsync();

            return Ok(accountants);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountant(int id)
        {
            var accountant =
                await _accountantService.GetAccountantAsync(id);

            return Ok(accountant);
        }
        [Authorize(Roles = nameof(UserRole.Accountant))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateAccountantDto updateAccountantDto)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedAccountant =
                await _accountantService.UpdateAccountantAsync(
                    updateAccountantDto,
                    id);

            return Ok(updatedAccountant);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountant(
            int id,
            [FromBody] UpdateAccountantDto updateAccountantDto)
        {
            var updatedAccountant =
                await _accountantService.UpdateAccountantAsync(
                    updateAccountantDto,
                    id);

            return Ok(updatedAccountant);
        }
    }
}