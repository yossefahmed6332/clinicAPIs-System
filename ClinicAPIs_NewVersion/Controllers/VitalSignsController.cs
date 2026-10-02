using clinicAPIsSystem.DTOs.VitalSignsDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        private readonly IVitalSignsService _vitalSignsService;
        private readonly IMemoryCache _cache;

        public VitalSignsController(
            IVitalSignsService vitalSignsService,
            IMemoryCache cache)
        {
            _vitalSignsService = vitalSignsService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)}")]
        [HttpPost]
        public async Task<IActionResult> CreateVitalSigns(
            [FromBody] CreateVitalSignsDto createVitalSignsDto)
        {
            var createdVitalSigns =
                await _vitalSignsService.CreateVitalSignsAsync(
                    createVitalSignsDto);

            _cache.Remove("allVitalSigns");

            return CreatedAtAction(
                nameof(GetVitalSigns),
                new { id = createdVitalSigns.Id },
                createdVitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllVitalSigns()
        {
            const string cacheKey = "allVitalSigns";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<VitalSignsDto>? vitalSigns))
            {
                vitalSigns =
                    await _vitalSignsService
                        .GetAllVitalSignsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, vitalSigns, cacheOptions);
            }

            return Ok(vitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSigns(int id)
        {
            string cacheKey = $"vitalSigns:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out VitalSignsDto? vitalSigns))
            {
                vitalSigns =
                    await _vitalSignsService
                        .GetVitalSignsAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, vitalSigns, cacheOptions);
            }

            return Ok(vitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetVitalSignsByMedicalRecordId(
            int medicalRecordId)
        {
            string cacheKey =
                $"vitalSigns:medicalRecord:{medicalRecordId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<VitalSignsDto>? vitalSigns))
            {
                vitalSigns =
                    await _vitalSignsService
                        .GetVitalSignsByMedicalRecordIdAsync(
                            medicalRecordId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, vitalSigns, cacheOptions);
            }

            return Ok(vitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetVitalSignsByNurseId(
            int nurseId)
        {
            string cacheKey =
                $"vitalSigns:nurse:{nurseId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<VitalSignsDto>? vitalSigns))
            {
                vitalSigns =
                    await _vitalSignsService
                        .GetVitalSignsByNurseIdAsync(
                            nurseId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, vitalSigns, cacheOptions);
            }

            return Ok(vitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVitalSigns(
            int id,
            [FromBody] UpdateVitalSignsDto updateVitalSignsDto)
        {
            var updatedVitalSigns =
                await _vitalSignsService.UpdateVitalSignsAsync(
                    updateVitalSignsDto,
                    id);

            _cache.Remove($"vitalSigns:{id}");
            _cache.Remove("allVitalSigns");

            return Ok(updatedVitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVitalSigns(int id)
        {
            await _vitalSignsService.DeleteVitalSignsAsync(id);

            _cache.Remove($"vitalSigns:{id}");
            _cache.Remove("allVitalSigns");

            return NoContent();
        }
    }
}