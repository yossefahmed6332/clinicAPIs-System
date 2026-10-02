using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.AccountantDTO;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly IAccountantService _accountantService;
        private readonly IMemoryCache _cache;

        public AccountantController(
            IAccountantService accountantService,
            IMemoryCache cache)
        {
            _accountantService = accountantService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateAccountant(
            [FromBody] CreateAccountantDto createAccountantDto)
        {
            var accountant =
                await _accountantService.CreateAccountantAsync(
                    createAccountantDto);

            _cache.Remove("allAccountants");

            return Ok(accountant);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccountants()
        {
            const string cacheKey = "allAccountants";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<AccountantDto>? accountants))
            {
                accountants =
                    await _accountantService.GetAllAccountsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    accountants,
                    cacheOptions);
            }

            return Ok(accountants);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountant(int id)
        {
            string cacheKey = $"accountant:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out AccountantDto? accountant))
            {
                accountant =
                    await _accountantService.GetAccountantAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    accountant,
                    cacheOptions);
            }

            return Ok(accountant);
        }

        [Authorize(Roles = nameof(UserRole.Accountant))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateAccountantDto updateAccountantDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedAccountant =
                await _accountantService.UpdateAccountantAsync(
                    updateAccountantDto,
                    id);

            _cache.Remove($"accountant:{id}");
            _cache.Remove("allAccountants");

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

            _cache.Remove($"accountant:{id}");
            _cache.Remove("allAccountants");

            return Ok(updatedAccountant);
        }
    }
}
