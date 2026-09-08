using clinicAPIsSystem.DTOs.VitalSignsDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitalSignsController : ControllerBase
    {
        private readonly IVitalSignsService _vitalSignsService;

        public VitalSignsController(
            IVitalSignsService vitalSignsService)
        {
            _vitalSignsService = vitalSignsService;
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)}")]
        [HttpPost]
        public async Task<IActionResult> CreateVitalSigns(
            [FromBody] CreateVitalSignsDto createVitalSignsDto)
        {
            var createdVitalSigns =
                await _vitalSignsService.CreateVitalSignsAsync(
                    createVitalSignsDto);

            return CreatedAtAction(
                nameof(GetVitalSigns),
                new { id = createdVitalSigns.Id },
                createdVitalSigns);
        }
        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllVitalSigns()
        {
            var vitalSigns =
                await _vitalSignsService.GetAllVitalSignsAsync();

            return Ok(vitalSigns);
        }
        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSigns(int id)
        {
            var vitalSigns =
                await _vitalSignsService.GetVitalSignsAsync(id);

            return Ok(vitalSigns);
        }

        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<IActionResult> GetVitalSignsByMedicalRecordId(
            int medicalRecordId)
        {
            var vitalSigns =
                await _vitalSignsService
                    .GetVitalSignsByMedicalRecordIdAsync(medicalRecordId);

            return Ok(vitalSigns);
        }
        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetVitalSignsByNurseId(
            int nurseId)
        {
            var vitalSigns =
                await _vitalSignsService
                    .GetVitalSignsByNurseIdAsync(nurseId);

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

            return Ok(updatedVitalSigns);
        }
        [Authorize(Roles = $"{nameof(UserRole.Doctor)},{nameof(UserRole.Admin)},{nameof(UserRole.Nurse)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVitalSigns(int id)
        {
            await _vitalSignsService.DeleteVitalSignsAsync(id);

            return NoContent();
        }
    }
}