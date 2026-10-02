#region used namespaces
using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
#endregion

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationResultController : ControllerBase
    {
        private readonly IExaminationResultService _examinationResultService;
        private readonly IMemoryCache _cache;

        public ExaminationResultController(
            IExaminationResultService examinationResultService,
            IMemoryCache cache)
        {
            _examinationResultService = examinationResultService;
            _cache = cache;
        }

        // POST: api/ExaminationResult
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}")]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationResult(
            [FromBody] CreateExaminationResultDto createExaminationResultDto)
        {
            var createdExaminationResult =
                await _examinationResultService.CreateExaminationResultAsync(
                    createExaminationResultDto);

            _cache.Remove("allExaminationResults");

            return CreatedAtAction(
                nameof(GetExaminationResult),
                new { id = createdExaminationResult.Id },
                createdExaminationResult);
        }

        // GET: api/ExaminationResult
        [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminationResults()
        {
            const string cacheKey = "allExaminationResults";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<ExaminationResultDto>? examinationResults))
            {
                examinationResults =
                    await _examinationResultService
                        .GetAllExaminationResultsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    examinationResults,
                    cacheOptions);
            }

            return Ok(examinationResults);
        }

        // GET: api/ExaminationResult/{id}
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExaminationResult(int id)
        {
            string cacheKey = $"examinationResult:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out ExaminationResultDto? examinationResult))
            {
                examinationResult =
                    await _examinationResultService
                        .GetExaminationResultAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    examinationResult,
                    cacheOptions);
            }

            return Ok(examinationResult);
        }

        // GET: api/ExaminationResult/nurse/{nurseId}
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetExaminationResultsByNurseId(
            int nurseId)
        {
            string cacheKey = $"examinationResults:nurse:{nurseId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<ExaminationResultDto>? examinationResults))
            {
                examinationResults =
                    await _examinationResultService
                        .GetExaminationResultsByNurseIdAsync(nurseId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    examinationResults,
                    cacheOptions);
            }

            return Ok(examinationResults);
        }

        // GET: api/ExaminationResult/medical-record/{medicalRecordId}
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetExaminationResultsByMedicalRecordId(
            int medicalRecordId)
        {
            string cacheKey =
                $"examinationResults:medicalRecord:{medicalRecordId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<ExaminationResultDto>? examinationResults))
            {
                examinationResults =
                    await _examinationResultService
                        .GetExaminationResultsByMedicalRecordIdAsync(
                            medicalRecordId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    examinationResults,
                    cacheOptions);
            }

            return Ok(examinationResults);
        }

        // PUT: api/ExaminationResult/{id}
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExaminationResult(
            int id,
            [FromBody] UpdateExaminationResultDto updateExaminationResultDto)
        {
            var updatedExaminationResult =
                await _examinationResultService
                    .UpdateExaminationResultAsync(
                        updateExaminationResultDto,
                        id);

            _cache.Remove($"examinationResult:{id}");
            _cache.Remove("allExaminationResults");

            return Ok(updatedExaminationResult);
        }

        // DELETE: api/ExaminationResult/{id}
        [Authorize(Roles =$"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExaminationResult(int id)
        {
            await _examinationResultService
                .DeleteExaminationResultAsync(id);

            _cache.Remove($"examinationResult:{id}");
            _cache.Remove("allExaminationResults");

            return NoContent();
        }
    }
}
