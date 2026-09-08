using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.RepositoryService.UserRepository
{
    public class PatientRepository:IPatientRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context; 
        public PatientRepository(UserManager<ApplicationUser> userManager,ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(
      Patient patient,
      bool addUserRes,
      bool addPasswordRes,
      bool addRoleRes)> CreatePatientAsync(
          Patient patient,
          string password)
        {
            var addUserRes = await _userManager.CreateAsync(patient);

            if (!addUserRes.Succeeded)
                return (patient, false, false, false);

            var addPasswordRes =
                await _userManager.AddPasswordAsync(patient, password);

            if (!addPasswordRes.Succeeded)
                return (patient, true, false, false);

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    patient,
                    UserRole.Patient.ToString());

            return (
                patient,
                true,
                true,
                addRoleRes.Succeeded);
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {

            return await _context.TPatients.ToListAsync();
        }

        public async Task<Patient?> GetPatientAsync(int id)
        {
            return await _context.TPatients.FindAsync(id);
        }


        public async Task<Patient> UpdatePatientAsync(Patient patient)
        {
            _context.TPatients.Update(patient);
           await _context.SaveChangesAsync();
            return patient;
        }

        public async Task DeletePatientAsync(Patient patient)
        {
            _context.TPatients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
