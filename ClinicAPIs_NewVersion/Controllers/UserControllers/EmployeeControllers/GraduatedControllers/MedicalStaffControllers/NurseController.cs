using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateNurse(
            [FromBody] CreateNurseDto createNurseDto
            )
        {
            var nurse =
                await _nurseService.CreateNurseAsync(
                    createNurseDto
                    );

            return Ok(nurse);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNurses()
        {
            var nurses =
                await _nurseService.GetAllNursesAsync();

            return Ok(nurses);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)},{nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNurse(int id)
        {
            var nurse =
                await _nurseService.GetNurseAsync(id);

            return Ok(nurse);
        }
        [Authorize(Roles = nameof(UserRole.Nurse))]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyAccount(
            [FromBody] UpdateNurseDto updateNurseDto)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            var updatedNurse =
                await _nurseService.UpdateNurseAsync(
                    updateNurseDto,
                    id);

            return Ok(updatedNurse);
        }
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNurse(
            int id,
            [FromBody] UpdateNurseDto updateNurseDto)
        {
            var updatedNurse =
                await _nurseService.UpdateNurseAsync(
                    updateNurseDto,
                    id);

            return Ok(updatedNurse);
        }
    }
}