using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Data;
using Microsoft.EntityFrameworkCore;
//note: I catch null reference exception in the service layer, so I don't need to catch it here 
namespace clinicAPIsSystem.RepositoryService
{
    public class AppointmentRepository:IAppointmentRepository
    {
        private readonly ClinicDbContext _context;
        public AppointmentRepository(ClinicDbContext context)
        {
            _context = context;
        }
        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
           await _context.TAppointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.TAppointments.ToListAsync();
        }
        public async Task<Appointment?> GetAppointmentAsync(int id)
        {
            return await _context.TAppointments.FindAsync(id);
        }
        public async Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await _context.TAppointments.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForUser(int userId)
        {
            return await _context.TAppointments.Where(a => a.PatientId == userId || a.DoctorId == userId || a.NurseId == userId).ToListAsync();
        }   
        public async Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _context.TAppointments.Where(a => a.DoctorId == doctorId).ToListAsync();
        }
        public async Task<List<Appointment>> GetAppointmentsByNurseIdAsync(int nurseId)
        {
            return await _context.TAppointments.Where(a => a.NurseId == nurseId).ToListAsync();
        }
        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.TAppointments.Where(a => a.PatientId == patientId).ToListAsync();
        }

        //catch null reference exception in the service layer, so I don't need to catch it here
        public async Task<Appointment?> GetAppointmentInTimeRange(DateTime startDate, DateTime endDate, int doctorId,int nurseId)
        {
            return await _context.TAppointments
                .Where(a => a.StartDate < endDate && a.EndDate > startDate && a.DoctorId == doctorId && a.NurseId == nurseId)
                .FirstOrDefaultAsync();
        }
        public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointment)
        {
            _context.TAppointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            _context.TAppointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

    }
}
