using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IService
{
    public interface IAppointmentService
    {
        public Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto appointment);
        public Task<List<AppointmentDto>> GetAllAppointmentsAsync();
        public Task<AppointmentDto> GetAppointmentAsync(int id);
        public Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
        public Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        public Task<List<AppointmentDto>> GetAppointmentsByNurseIdAsync(int nurseId);
        public Task<List<AppointmentDto>> GetAppointmentsForUserByTokens(string token);
        public Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status);
        public Task<AppointmentDto> UpdateAppointmentAsync(UpdateAppointmentDto appointment,int id);
        public Task DeleteAppointmentAsync(int id);
    }
}
