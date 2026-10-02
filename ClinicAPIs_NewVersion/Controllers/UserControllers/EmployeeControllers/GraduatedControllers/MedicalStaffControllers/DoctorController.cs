
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;
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
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IMemoryCache _cache;

        public DoctorController(
            IDoctorService doctorService,
            IMemoryCache cache)
        {
            _doctorService = doctorService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateDoctor(
            [FromBody] CreateDoctorDto createDoctorDto)
        {
            var doctor =
                await _doctorService.CreateDoctorAsync(
                    createDoctorDto);

            _cache.Remove("allDoctors");

            return Ok(doctor);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            const string cacheKey = "allDoctors";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<DoctorDto>? doctors))
            {
                doctors =
                    await _doctorService.GetAllDoctorsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    doctors,
                    cacheOptions);
            }

            return Ok(doctors);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            string cacheKey = $"doctor:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out DoctorDto? doctor))
            {
                doctor =
                    await _doctorService.GetDoctorAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    doctor,
                    cacheOptions);
            }

            return Ok(doctor);
        }

        [Authorize(Roles = nameof(UserRole.Doctor))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateDoctorDto updateDoctorDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedDoctor =
                await _doctorService.UpdateDoctorAsync(
                    updateDoctorDto,
                    id);

            _cache.Remove($"doctor:{id}");
            _cache.Remove("allDoctors");

            return Ok(updatedDoctor);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] UpdateDoctorDto updateDoctorDto)
        {
            var updatedDoctor =
                await _doctorService.UpdateDoctorAsync(
                    updateDoctorDto,
                    id);

            _cache.Remove($"doctor:{id}");
            _cache.Remove("allDoctors");

            return Ok(updatedDoctor);
        }
    }
}   