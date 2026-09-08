using AutoMapper;
using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IServices.IUserServices;

namespace clinicAPIsSystem.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IUserService userService,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }


        // CREATE


        public async Task<AppointmentDto> CreateAppointmentAsync(
            CreateAppointmentDto dto)
        {
            _logger.LogInformation(
                "Creating appointment for Patient {PatientId}, Doctor {DoctorId}, Nurse {NurseId}",
                dto.PatientId,
                dto.DoctorId,
                dto.NurseId);


            // Check if appointment is in the past
            if (dto.StartDate < DateTime.Now)
            {
                _logger.LogWarning(
                    "Appointment rejected because StartDate {StartDate} is in the past",
                    dto.StartDate);

                throw new ArgumentException(
                    "Appointment cannot be created in the past");
            }


            // Check doctor/nurse availability
            var existingAppointment =
                await _appointmentRepository.GetAppointmentInTimeRange(
                    dto.StartDate,
                    dto.EndDate,
                    dto.DoctorId,
                    dto.NurseId);

            if (existingAppointment != null)
            {
                _logger.LogWarning(
                    "Appointment rejected because Doctor {DoctorId} or Nurse {NurseId} is unavailable from {StartDate} to {EndDate}",
                    dto.DoctorId,
                    dto.NurseId,
                    dto.StartDate,
                    dto.EndDate);

                throw new ArgumentException(
                    "Doctor or Nurse is not available in the given time range");
            }


            // Create appointment
            var appointment = new Appointment(
                dto.StartDate,
                dto.EndDate,
                dto.PatientId,
                dto.NurseId,
                dto.DoctorId);


            // Save appointment
            appointment =
                await _appointmentRepository.CreateAppointmentAsync(
                    appointment);


            _logger.LogInformation(
                "Appointment {AppointmentId} created successfully for Patient {PatientId}",
                appointment.Id,
                appointment.PatientId);


            return _mapper.Map<AppointmentDto>(appointment);
        }



        // GET ALL


        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            _logger.LogDebug(
                "Retrieving all appointments");

            var appointments =
                await _appointmentRepository
                    .GetAllAppointmentsAsync();

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // GET BY ID


        public async Task<AppointmentDto> GetAppointmentAsync(int id)
        {
            _logger.LogDebug(
                "Retrieving appointment {AppointmentId}",
                id);

            var appointment =
                await _appointmentRepository
                    .GetAppointmentAsync(id);


            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} was not found",
                    id);

                throw new KeyNotFoundException(
                    $"Appointment with ID {id} not found.");
            }


            return _mapper.Map<AppointmentDto>(appointment);
        }



        // GET BY DOCTOR


        public async Task<List<AppointmentDto>>
            GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            _logger.LogDebug(
                "Retrieving appointments for Doctor {DoctorId}",
                doctorId);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByDoctorIdAsync(doctorId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // GET BY PATIENT


        public async Task<List<AppointmentDto>>
            GetAppointmentsByPatientIdAsync(int patientId)
        {
            _logger.LogDebug(
                "Retrieving appointments for Patient {PatientId}",
                patientId);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByPatientIdAsync(patientId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // GET BY NURSE


        public async Task<List<AppointmentDto>>
            GetAppointmentsByNurseIdAsync(int nurseId)
        {
            _logger.LogDebug(
                "Retrieving appointments for Nurse {NurseId}",
                nurseId);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByNurseIdAsync(nurseId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // GET BY STATUS


        public async Task<List<AppointmentDto>>
            GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            _logger.LogDebug(
                "Retrieving appointments with status {Status}",
                status);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByStatusAsync(status);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // GET FOR CURRENT USER


        public async Task<List<AppointmentDto>>
            GetAppointmentsForUserByTokens(string token)
        {
            _logger.LogDebug(
                "Retrieving appointments for authenticated user");

            // Do NOT log the token itself
            var userId =
                await _userService.GetIdFromTokensAsync(token);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsForUser(userId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }



        // UPDATE


        public async Task<AppointmentDto> UpdateAppointmentAsync(
            UpdateAppointmentDto dto,
            int id)
        {
            _logger.LogInformation(
                "Updating appointment {AppointmentId}",
                id);


            var appointment =
                await _appointmentRepository
                    .GetAppointmentAsync(id);


            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} was not found for update",
                    id);

                throw new KeyNotFoundException(
                    $"Appointment with ID {id} not found.");
            }


            // Check if important appointment data changed
            if (dto.StartDate != appointment.StartDate ||
                dto.EndDate != appointment.EndDate ||
                dto.DoctorId != appointment.DoctorId ||
                dto.NurseId != appointment.NurseId)
            {
                // Check if new date is in the past
                if (dto.StartDate < DateTime.Now)
                {
                    _logger.LogWarning(
                        "Update rejected for appointment {AppointmentId} because StartDate {StartDate} is in the past",
                        id,
                        dto.StartDate);

                    throw new ArgumentException(
                        "Appointment cannot be created in the past");
                }


                // Check doctor/nurse availability
                var existingAppointment =
                    await _appointmentRepository
                        .GetAppointmentInTimeRange(
                            dto.StartDate,
                            dto.EndDate,
                            dto.DoctorId,
                            dto.NurseId);


                if (existingAppointment != null)
                {
                    _logger.LogWarning(
                        "Update rejected for appointment {AppointmentId} because Doctor {DoctorId} or Nurse {NurseId} is unavailable",
                        id,
                        dto.DoctorId,
                        dto.NurseId);

                    throw new ArgumentException(
                        "Doctor or Nurse is not available in the given time range");
                }
            }


            appointment.Update(
                dto.StartDate,
                dto.EndDate,
                dto.Status,
                dto.NurseId,
                dto.PatientId,
                dto.DoctorId);


            appointment =
                await _appointmentRepository
                    .UpdateAppointmentAsync(appointment);


            _logger.LogInformation(
                "Appointment {AppointmentId} updated successfully",
                id);


            return _mapper.Map<AppointmentDto>(appointment);
        }



        // DELETE


        public async Task DeleteAppointmentAsync(int id)
        {
            _logger.LogInformation(
                "Deleting appointment {AppointmentId}",
                id);


            var appointment =
                await _appointmentRepository
                    .GetAppointmentAsync(id);


            if (appointment == null)
            {
                _logger.LogWarning(
                    "Appointment {AppointmentId} was not found for deletion",
                    id);

                throw new KeyNotFoundException(
                    $"Appointment with ID {id} not found.");
            }


            await _appointmentRepository
                .DeleteAppointmentAsync(appointment);


            _logger.LogInformation(
                "Appointment {AppointmentId} deleted successfully",
                id);
        }
    }
}