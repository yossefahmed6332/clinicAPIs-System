using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateDoctor(
            [FromBody] CreateDoctorDto createDoctorDto
            )
        {
            var doctor =
                await _doctorService.CreateDoctorAsync(
                    createDoctorDto
                    );

            return Ok(doctor);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors =
                await _doctorService.GetAllDoctorsAsync();

            return Ok(doctors);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor =
                await _doctorService.GetDoctorAsync(id);

            return Ok(doctor);
        }
        [Authorize(Roles = $"{nameof(UserRole.Doctor)}")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateDoctorDto updateDoctorDto)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedDoctor =
                await _doctorService.UpdateDoctorAsync(
                    updateDoctorDto,
                    id);

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

            return Ok(updatedDoctor);
        }
    }
}