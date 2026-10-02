#region Used name spaces    
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
#endregion

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMemoryCache _cache;

        public PatientController(
            IPatientService patientService,
            IMemoryCache cache)
        {
            _patientService = patientService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreatePatient(
            [FromBody] CreatePatientDto createPatientDto)
        {
            var patient =
                await _patientService.CreatePatientAsync(
                    createPatientDto);

            _cache.Remove("allPatients");

            return Ok(patient);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPatients()
        {
            const string cacheKey = "allPatients";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PatientDto>? patients))
            {
                patients =
                    await _patientService.GetAllPatientsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(
                    cacheKey,
                    patients,
                    cacheOptions);
            }

            return Ok(patients);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            string cacheKey = $"patient:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out PatientDto? patient))
            {
                patient =
                    await _patientService.GetPatientAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(
                    cacheKey,
                    patient,
                    cacheOptions);
            }

            return Ok(patient);
        }

        [Authorize(Roles = nameof(UserRole.Patient))]
        // Update current logged-in patient
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdatePatientDto updatePatientDto)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedPatient =
                await _patientService.UpdatePatientAsync(
                    updatePatientDto,
                    id);

            _cache.Remove($"patient:{id}");
            _cache.Remove("allPatients");

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

            _cache.Remove($"patient:{id}");
            _cache.Remove("allPatients");

            return Ok(updatedPatient);
        }
    }
}
