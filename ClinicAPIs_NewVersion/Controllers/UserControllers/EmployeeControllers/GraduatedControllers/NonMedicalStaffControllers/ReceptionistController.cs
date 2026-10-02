
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ReceptionistDTO;
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
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;
        private readonly IMemoryCache _cache;

        public ReceptionistController(
            IReceptionistService receptionistService,
            IMemoryCache cache)
        {
            _receptionistService = receptionistService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateReceptionist(
            [FromBody] CreateReceptionistDto createReceptionistDto)
        {
            var receptionist =
                await _receptionistService.CreateReceptionistAsync(
                    createReceptionistDto);

            _cache.Remove("allReceptionists");

            return Ok(receptionist);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReceptionist(int id)
        {
            string cacheKey = $"receptionist:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out ReceptionistDto? receptionist))
            {
                receptionist =
                    await _receptionistService
                        .GetReceptionistAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    receptionist,
                    cacheOptions);
            }

            return Ok(receptionist);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReceptionists()
        {
            const string cacheKey = "allReceptionists";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<ReceptionistDto>? receptionists))
            {
                receptionists =
                    await _receptionistService
                        .GetAllReceptionistsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    receptionists,
                    cacheOptions);
            }

            return Ok(receptionists);
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateReceptionistDto updateReceptionistDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedReceptionist =
                await _receptionistService.UpdateReceptionistAsync(
                    updateReceptionistDto,
                    id);

            _cache.Remove($"receptionist:{id}");
            _cache.Remove("allReceptionists");

            return Ok(updatedReceptionist);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReceptionist(
            int id,
            [FromBody] UpdateReceptionistDto updateReceptionistDto)
        {
            var updatedReceptionist =
                await _receptionistService.UpdateReceptionistAsync(
                    updateReceptionistDto,
                    id);

            _cache.Remove($"receptionist:{id}");
            _cache.Remove("allReceptionists");

            return Ok(updatedReceptionist);
        }
    }
}

