using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMemoryCache _cache;

        public PrescriptionController(
            IPrescriptionService prescriptionService,
            IMemoryCache cache)
        {
            _prescriptionService = prescriptionService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreatePrescription(
            [FromBody] CreatePrescriptionDto createPrescriptionDto)
        {
            var createdPrescription =
                await _prescriptionService.CreatePrescriptionAsync(
                    createPrescriptionDto);

            _cache.Remove("allPrescriptions");

            return CreatedAtAction(
                nameof(GetPrescription),
                new { id = createdPrescription.Id },
                createdPrescription);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllPrescriptions()
        {
            const string cacheKey = "allPrescriptions";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PrescriptionDto>? prescriptions))
            {
                prescriptions =
                    await _prescriptionService
                        .GetAllPrescriptionsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, prescriptions, cacheOptions);
            }

            return Ok(prescriptions);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescription(int id)
        {
            string cacheKey = $"prescription:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out PrescriptionDto? prescription))
            {
                prescription =
                    await _prescriptionService
                        .GetPrescriptionAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, prescription, cacheOptions);
            }

            return Ok(prescription);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetPrescriptionsByMedicalRecordId(
            int medicalRecordId)
        {
            string cacheKey =
                $"prescriptions:medicalRecord:{medicalRecordId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PrescriptionDto>? prescriptions))
            {
                prescriptions =
                    await _prescriptionService
                        .GetPrescriptionsByMedicalRecordIdAsync(
                            medicalRecordId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, prescriptions, cacheOptions);
            }

            return Ok(prescriptions);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetPrescriptionsByDoctorId(
            int doctorId)
        {
            string cacheKey =
                $"prescriptions:doctor:{doctorId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PrescriptionDto>? prescriptions))
            {
                prescriptions =
                    await _prescriptionService
                        .GetPrescriptionsByDoctorIdAsync(
                            doctorId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, prescriptions, cacheOptions);
            }

            return Ok(prescriptions);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(
            int id,
            [FromBody] UpdatePrescriptionDto updatePrescriptionDto)
        {
            var updatedPrescription =
                await _prescriptionService.UpdatePrescriptionAsync(
                    updatePrescriptionDto,
                    id);

            _cache.Remove($"prescription:{id}");
            _cache.Remove("allPrescriptions");

            return Ok(updatedPrescription);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            await _prescriptionService.DeletePrescriptionAsync(id);

            _cache.Remove($"prescription:{id}");
            _cache.Remove("allPrescriptions");

            return NoContent();
        }
    }
}