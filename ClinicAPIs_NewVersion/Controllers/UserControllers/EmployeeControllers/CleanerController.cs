using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.CleanerDTO;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CleanerController : ControllerBase
    {
        private readonly ICleanerService _cleanerService;
        private readonly IMemoryCache _cache;

        public CleanerController(
            ICleanerService cleanerService,
            IMemoryCache cache)
        {
            _cleanerService = cleanerService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateCleaner(
            [FromBody] CreateCleanerDto createCleanerDto)
        {
            var cleaner =
                await _cleanerService.CreateCleanerAsync(
                    createCleanerDto);

            _cache.Remove("allCleaners");

            return Ok(cleaner);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCleaners()
        {
            const string cacheKey = "allCleaners";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<CleanerDto>? cleaners))
            {
                cleaners =
                    await _cleanerService.GetAllCleanersAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    cleaners,
                    cacheOptions);
            }

            return Ok(cleaners);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCleaner(int id)
        {
            string cacheKey = $"cleaner:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out CleanerDto? cleaner))
            {
                cleaner =
                    await _cleanerService.GetCleanerAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    cleaner,
                    cacheOptions);
            }

            return Ok(cleaner);
        }

        [Authorize(Roles = nameof(UserRole.Cleaner))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateCleanerDto updateCleanerDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedCleaner =
                await _cleanerService.UpdateCleanerAsync(
                    updateCleanerDto,
                    id);

            _cache.Remove($"cleaner:{id}");
            _cache.Remove("allCleaners");

            return Ok(updatedCleaner);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCleaner(
            int id,
            [FromBody] UpdateCleanerDto updateCleanerDto)
        {
            var updatedCleaner =
                await _cleanerService.UpdateCleanerAsync(
                    updateCleanerDto,
                    id);

            _cache.Remove($"cleaner:{id}");
            _cache.Remove("allCleaners");

            return Ok(updatedCleaner);
        }
    }
}

