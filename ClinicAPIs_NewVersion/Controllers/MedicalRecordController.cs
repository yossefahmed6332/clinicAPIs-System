using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IMemoryCache _cache;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService,
            IMemoryCache cache)
        {
            _medicalRecordService = medicalRecordService;
            _cache = cache;
        }

        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            const string cacheKey = "allMedicalRecords";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<MedicalRecordDto>? medicalRecords))
            {
                medicalRecords =
                    await _medicalRecordService
                        .GetAllMedicalRecordsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, medicalRecords, cacheOptions);
            }

            return Ok(medicalRecords);
        }

        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecord(int id)
        {
            string cacheKey = $"medicalRecord:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out MedicalRecordDto? medicalRecord))
            {
                medicalRecord =
                    await _medicalRecordService
                        .GetMedicalRecordAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, medicalRecord, cacheOptions);
            }

            return Ok(medicalRecord);
        }

        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetMedicalRecordByPatientId(
            int patientId)
        {
            string cacheKey = $"medicalRecord:patient:{patientId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out MedicalRecordDto? medicalRecord))
            {
                medicalRecord =
                    await _medicalRecordService
                        .GetMedicalByPatientIdRecord(patientId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, medicalRecord, cacheOptions);
            }

            return Ok(medicalRecord);
        }

        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(
            int id,
            [FromBody] UpdateMedicalRecordDto medicalRecord)
        {
            var updatedMedicalRecord =
                await _medicalRecordService.UpdateMedicalRecordAsync(
                    medicalRecord,
                    id);

            _cache.Remove($"medicalRecord:{id}");
            _cache.Remove("allMedicalRecords");

            return Ok(updatedMedicalRecord);
        }
    }
}