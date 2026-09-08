using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IAppointmentRepository
    {
        public Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        public Task<List<Appointment>> GetAllAppointmentsAsync();
        public Task<Appointment?> GetAppointmentAsync(int id);
        public Task<List<Appointment>> GetAppointmentsByStatusAsync( AppointmentStatus status);
        public Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        public Task<List<Appointment>> GetAppointmentsByNurseIdAsync(int nurseId);
        public Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        public Task<List<Appointment>> GetAppointmentsForUser(int userId);
        public Task<Appointment?> GetAppointmentInTimeRange(DateTime startDate, DateTime endDate, int doctorId,int nurseId);
        public Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
        public Task DeleteAppointmentAsync(Appointment appointment);

    }
}
