using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ManagerDTO;
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
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IMemoryCache _cache;

        public ManagerController(
            IManagerService managerService,
            IMemoryCache cache)
        {
            _managerService = managerService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateManager(
            [FromBody] CreateManagerDto createManagerDto)
        {
            var manager =
                await _managerService.CreateManagerAsync(
                    createManagerDto);

            _cache.Remove("allManagers");

            return Ok(manager);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllManagers()
        {
            const string cacheKey = "allManagers";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<ManagerDto>? managers))
            {
                managers =
                    await _managerService.GetAllManagersAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    managers,
                    cacheOptions);
            }

            return Ok(managers);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManager(int id)
        {
            string cacheKey = $"manager:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out ManagerDto? manager))
            {
                manager =
                    await _managerService.GetManagerAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    manager,
                    cacheOptions);
            }

            return Ok(manager);
        }

        [Authorize(Roles = nameof(UserRole.Manager))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateManagerDto updateManagerDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedManager =
                await _managerService.UpdateManagerAsync(
                    updateManagerDto,
                    id);

            _cache.Remove($"manager:{id}");
            _cache.Remove("allManagers");

            return Ok(updatedManager);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManager(
            int id,
            [FromBody] UpdateManagerDto updateManagerDto)
        {
            var updatedManager =
                await _managerService.UpdateManagerAsync(
                    updateManagerDto,
                    id);

            _cache.Remove($"manager:{id}");
            _cache.Remove("allManagers");

            return Ok(updatedManager);
        }
    }
}
