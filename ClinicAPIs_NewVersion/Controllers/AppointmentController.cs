
#region used namespaces
using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
#endregion

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMemoryCache _cache;

    public AppointmentController(
        IAppointmentService appointmentService,
        IMemoryCache cache)
    {
        _appointmentService = appointmentService;
        _cache = cache;
    }

    // POST: api/Appointment
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}")]
    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        [FromBody] CreateAppointmentDto appointment)
    {
        var createdAppointment =
            await _appointmentService.CreateAppointmentAsync(appointment);

        _cache.Remove("allAppointments");

        return CreatedAtAction(
            nameof(GetAppointmentById),
            new { id = createdAppointment.Id },
            createdAppointment);
    }

    // GET: api/Appointments
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Nurse)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        const string cacheKey = "allAppointments";

        if (!_cache.TryGetValue(
                cacheKey,
                out List<AppointmentDto>? appointments))
        {
            appointments =
                await _appointmentService.GetAllAppointmentsAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, appointments, cacheOptions);
        }

        return Ok(appointments);
    }

    //GET: api/Appointments/{id}
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        string cacheKey = $"appointment:{id}";

        if (!_cache.TryGetValue(
                cacheKey,
                out AppointmentDto? appointment))
        {
            appointment =
                await _appointmentService.GetAppointmentAsync(id);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(6));

            _cache.Set(cacheKey, appointment, cacheOptions);
        }

        return Ok(appointment);
    }

    // GET: api/Appointments/status/{status}
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Doctor)}, {nameof(UserRole.Receptionist)}")]
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetAppointmentsByStatus(
        AppointmentStatus status)
    {
        string cacheKey = $"appointments:status:{status}";

        if (!_cache.TryGetValue(
                cacheKey,
                out List<AppointmentDto>? appointments))
        {
            appointments =
                await _appointmentService
                    .GetAppointmentsByStatusAsync(status);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, appointments, cacheOptions);
        }

        return Ok(appointments);
    }


    // GET: api/Appointments/doctor/{doctorId}
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
    {
        string cacheKey = $"appointments:doctor:{doctorId}";

        if (!_cache.TryGetValue(
                cacheKey,
                out List<AppointmentDto>? appointments))
        {
            appointments =
                await _appointmentService
                    .GetAppointmentsByDoctorIdAsync(doctorId);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, appointments, cacheOptions);
        }

        return Ok(appointments);
    }

    // GET: api/Appointments/patient/{patientId}
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
    {
        string cacheKey = $"appointments:patient:{patientId}";

        if (!_cache.TryGetValue(
                cacheKey,
                out List<AppointmentDto>? appointments))
        {
            appointments =
                await _appointmentService
                    .GetAppointmentsByPatientIdAsync(patientId);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, appointments, cacheOptions);
        }

        return Ok(appointments);
    }
    // GET: api/Appointments/nurse/{nurseId}
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
    [HttpGet("nurse/{nurseId}")]
    public async Task<IActionResult> GetAppointmentsByNurseId(int nurseId)
    {
        string cacheKey = $"appointments:nurse:{nurseId}";

        if (!_cache.TryGetValue(
                cacheKey,
                out List<AppointmentDto>? appointments))
        {
            appointments =
                await _appointmentService
                    .GetAppointmentsByNurseIdAsync(nurseId);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, appointments, cacheOptions);
        }

        return Ok(appointments);
    }


    // GET: api/Appointments/user
    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetAppointmentsForUser()
    {
        var token = Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");

        var appointments =
            await _appointmentService
                .GetAppointmentsForUserByTokens(token);

        return Ok(appointments);
    }
    //PUT: api/Appointments/{id}
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(
        int id,
        [FromBody] UpdateAppointmentDto appointment)
    {
        var updatedAppointment =
            await _appointmentService
                .UpdateAppointmentAsync(appointment, id);

        _cache.Remove($"appointment:{id}");
        _cache.Remove("allAppointments");

        return Ok(updatedAppointment);
    }
    //Delete Appointment
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Receptionist)}, {nameof(UserRole.Manager)}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        await _appointmentService.DeleteAppointmentAsync(id);

        _cache.Remove($"appointment:{id}");
        _cache.Remove("allAppointments");

        return NoContent();
    }
}

