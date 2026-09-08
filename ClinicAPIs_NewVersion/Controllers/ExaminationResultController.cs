using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationResultController : ControllerBase
    {
        private readonly IExaminationResultService _examinationResultService;

        public ExaminationResultController(
            IExaminationResultService examinationResultService)
        {
            _examinationResultService = examinationResultService;
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}")]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationResult(
            [FromBody] CreateExaminationResultDto createExaminationResultDto)
        {
            var createdExaminationResult =
                await _examinationResultService.CreateExaminationResultAsync(
                    createExaminationResultDto);

            return CreatedAtAction(
                nameof(GetExaminationResult),
                new { id = createdExaminationResult.Id },
                createdExaminationResult);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminationResults()
        {
            var examinationResults =
                await _examinationResultService.GetAllExaminationResultsAsync();

            return Ok(examinationResults);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExaminationResult(int id)
        {
            var examinationResult =
                await _examinationResultService.GetExaminationResultAsync(id);

            return Ok(examinationResult);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]

        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetExaminationResultsByNurseId(
            int nurseId)
        {
            var examinationResults =
                await _examinationResultService
                    .GetExaminationResultsByNurseIdAsync(nurseId);

            return Ok(examinationResults);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetExaminationResultsByMedicalRecordId(
            int medicalRecordId)
        {
            var examinationResults =
                await _examinationResultService
                    .GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId);

            return Ok(examinationResults);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Manager)}")]
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

            return Ok(updatedExaminationResult);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExaminationResult(int id)
        {
            await _examinationResultService.DeleteExaminationResultAsync(id);

            return NoContent();
        }
    }
}