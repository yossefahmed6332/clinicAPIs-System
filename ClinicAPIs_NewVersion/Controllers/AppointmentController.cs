using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        [FromBody] CreateAppointmentDto appointment)
    {
        var createdAppointment =
            await _appointmentService.CreateAppointmentAsync(appointment);

        return CreatedAtAction(
            nameof(GetAppointmentById),
            new { id = createdAppointment.Id },
            createdAppointment);
    }

    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        var appointments =
            await _appointmentService.GetAllAppointmentsAsync();

        return Ok(appointments);
    }

    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        var appointment =
            await _appointmentService.GetAppointmentAsync(id);

        return Ok(appointment);
    }

    [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetAppointmentsByStatus(AppointmentStatus status)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByStatusAsync(status);

        return Ok(appointments);
    }
    [Authorize($"{nameof(UserRole.Admin)},  {nameof(UserRole.Receptionist)},{nameof(UserRole.Manager)}")]
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }
    [Authorize($"{nameof(UserRole.Admin)},  {nameof(UserRole.Receptionist)},{nameof(UserRole.Manager)}")]
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
        return Ok(appointments);
    }
    [Authorize($"{nameof(UserRole.Admin)},  {nameof(UserRole.Receptionist)},{nameof(UserRole.Manager)}")]
    [HttpGet("nurse/{nurseId}")]
    public async Task<IActionResult> GetAppointmentsByNurseId(int nurseId)
    {
        var appointments =
            await _appointmentService.GetAppointmentsByNurseIdAsync(nurseId);
        return Ok(appointments);
    }

    [Authorize()]
    [HttpGet("user")]
    public async Task<IActionResult> GetAppointmentsForUser()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var appointments = await _appointmentService.GetAppointmentsForUserByTokens(token);
        return Ok(appointments);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(
        int id,
        [FromBody] UpdateAppointmentDto appointment)
    {
        var updatedAppointment =
            await _appointmentService.UpdateAppointmentAsync(appointment, id);

        return Ok(updatedAppointment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        await _appointmentService.DeleteAppointmentAsync(id);

        return NoContent();
    }
}