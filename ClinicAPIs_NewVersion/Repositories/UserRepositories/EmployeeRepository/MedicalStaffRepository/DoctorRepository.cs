using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.RepositoryService.UserRepository.MedicalStaffRepository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context;

        public DoctorRepository(
            UserManager<ApplicationUser> userManager,
            ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(
            Doctor doctor,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateDoctorAsync(
                Doctor doctor,
                string password)
        {
            var addUserRes =
                await _userManager.CreateAsync(doctor);

            if (!addUserRes.Succeeded)
                return (doctor, false, false, false);

            var addPasswordRes =
                await _userManager.AddPasswordAsync(
                    doctor,
                    password);

            if (!addPasswordRes.Succeeded)
                return (doctor, true, false, false);

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    doctor,
                    UserRole.Doctor.ToString());

            return (
                doctor,
                true,
                true,
                addRoleRes.Succeeded);
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.TDoctors.ToListAsync();
        }

        public async Task<Doctor?> GetDoctorAsync(int id)
        {
            return await _context.TDoctors.FindAsync(id);
        }

        public async Task<(
            ICollection<Appointment> appointments,
            ICollection<Prescription> prescriptions)>
            GetDoctorWithDetailsAsync(int id)
        {
            var appointments = await _context.TAppointments
                .Where(a => a.DoctorId == id)
                .ToListAsync();

            var prescriptions = await _context.TPrescriptions
                .Where(p => p.DoctorId == id)
                .ToListAsync();

            return (appointments, prescriptions);
        }

        public async Task<Doctor> UpdateDoctorAsync(
            Doctor doctor)
        {
            _context.TDoctors.Update(doctor);

            await _context.SaveChangesAsync();

            return doctor;
        }


    }
}