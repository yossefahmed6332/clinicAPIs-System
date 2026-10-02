
using clinicAPIsSystem.DTOs.UserDTOs.AdminDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMemoryCache _cache;

        public AdminController(
            IAdminService adminService,
            IMemoryCache cache)
        {
            _adminService = adminService;
            _cache = cache;
        }

        [Authorize(Roles = "No body can create admin")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateAdmin(
            [FromBody] CreateAdminDto admin)
        {
            var createdAdmin =
                await _adminService.CreateAdminAsync(admin);

            _cache.Remove("allAdmins");

            return Ok(createdAdmin);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            const string cacheKey = "allAdmins";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<AdminDto>? admins))
            {
                admins =
                    await _adminService.GetAllAdminsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, admins, cacheOptions);
            }

            return Ok(admins);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            string cacheKey = $"admin:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out AdminDto? admin))
            {
                admin =
                    await _adminService.GetAdminAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, admin, cacheOptions);
            }

            return Ok(admin);
        }

        // Update current logged-in admin
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateAdminDto admin)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedAdmin =
                await _adminService.UpdateAdminAsync(admin, id);

            _cache.Remove($"admin:{id}");
            _cache.Remove("allAdmins");

            return Ok(updatedAdmin);
        }

        // Update specific admin - intended for Admin
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(
            int id,
            [FromBody] UpdateAdminDto admin)
        {
            var updatedAdmin =
                await _adminService.UpdateAdminAsync(admin, id);

            _cache.Remove($"admin:{id}");
            _cache.Remove("allAdmins");

            return Ok(updatedAdmin);
        }
    }
}
