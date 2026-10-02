using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;
        private readonly IMemoryCache _cache;

        public NurseController(
            INurseService nurseService,
            IMemoryCache cache)
        {
            _nurseService = nurseService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateNurse(
            [FromBody] CreateNurseDto createNurseDto)
        {
            var nurse =
                await _nurseService.CreateNurseAsync(
                    createNurseDto);

            _cache.Remove("allNurses");

            return Ok(nurse);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNurses()
        {
            const string cacheKey = "allNurses";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<NurseDto>? nurses))
            {
                nurses =
                    await _nurseService.GetAllNursesAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    nurses,
                    cacheOptions);
            }

            return Ok(nurses);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurse(int id)
        {
            string cacheKey = $"nurse:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out NurseDto? nurse))
            {
                nurse =
                    await _nurseService.GetNurseAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    nurse,
                    cacheOptions);
            }

            return Ok(nurse);
        }

        [Authorize(Roles = nameof(UserRole.Nurse))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateNurseDto updateNurseDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedNurse =
                await _nurseService.UpdateNurseAsync(
                    updateNurseDto,
                    id);

            _cache.Remove($"nurse:{id}");
            _cache.Remove("allNurses");

            return Ok(updatedNurse);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNurse(
            int id,
            [FromBody] UpdateNurseDto updateNurseDto)
        {
            var updatedNurse =
                await _nurseService.UpdateNurseAsync(
                    updateNurseDto,
                    id);

            _cache.Remove($"nurse:{id}");
            _cache.Remove("allNurses");

            return Ok(updatedNurse);
        }
    }
}
