using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            var medicalRecords =
                await _medicalRecordService.GetAllMedicalRecordsAsync();

            return Ok(medicalRecords);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecord(int id)
        {
            var medicalRecord =
                await _medicalRecordService.GetMedicalRecord(id);

            return Ok(medicalRecord);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetMedicalRecordByPatientId(
            int patientId)
        {
            var medicalRecord =
                await _medicalRecordService
                    .GetMedicalByPatientIdRecord(patientId);

            return Ok(medicalRecord);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(
            int id,
            [FromBody] UpdateMedicalRecordDto medicalRecord)
        {
            var updatedMedicalRecord =
                await _medicalRecordService.UpdateMedicalRecordAsync(
                    medicalRecord,
                    id);

            return Ok(updatedMedicalRecord);
        }
    }
}