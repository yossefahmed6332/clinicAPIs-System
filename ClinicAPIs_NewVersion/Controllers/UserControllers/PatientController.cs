using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreatePatient(
            [FromBody] CreatePatientDto createPatientDto
            )
        {
            var patient = await _patientService.CreatePatientAsync(
                createPatientDto
                );

            return Ok(patient);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientAsync(id);

            return Ok(patient);
        }
        [Authorize(Roles = (nameof(UserRole.Patient)))]
        // Update current logged-in patient
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdatePatientDto updatePatientDto)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedPatient =
                await _patientService.UpdatePatientAsync(
                    updatePatientDto,
                    id);

            return Ok(updatedPatient);
        }

        // Update specific patient - Admin
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] UpdatePatientDto updatePatientDto)
        {
            var updatedPatient =
                await _patientService.UpdatePatientAsync(
                    updatePatientDto,
                    id);

            return Ok(updatedPatient);
        }
    }
}