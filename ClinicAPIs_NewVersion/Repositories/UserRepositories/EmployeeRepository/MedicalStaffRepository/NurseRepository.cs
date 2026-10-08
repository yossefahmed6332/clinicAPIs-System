using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.RepositoryService.UserRepository.MedicalStaffRepository
{
    public class NurseRepository : INurseRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context;

        public NurseRepository(
            UserManager<ApplicationUser> userManager,
            ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(
            Nurse nurse,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateNurseAsync(
                Nurse nurse,
                string password)
        {
            var addUserRes =
                await _userManager.CreateAsync(nurse);

            if (!addUserRes.Succeeded)
                return (nurse, false, false, false);

            var addPasswordRes =
                await _userManager.AddPasswordAsync(
                    nurse,
                    password);

            if (!addPasswordRes.Succeeded)
                return (nurse, true, false, false);

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    nurse,
                    UserRole.Nurse.ToString());

            return (
                nurse,
                true,
                true,
                addRoleRes.Succeeded);
        }

        public async Task<List<Nurse>> GetAllNursesAsync()
        {
            return await _context.TNurses.AsNoTracking().ToListAsync();
        }

        public async Task<Nurse?> GetNurseAsync(int id)
        {
            return await _context.TNurses.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == id);
        }

        public async Task<Nurse> UpdateNurseAsync(Nurse nurse)
        {
            _context.TNurses.Update(nurse);

            await _context.SaveChangesAsync();

            return nurse;
        }

        public async Task DeleteNurseAsync(Nurse nurse)
        {
            _context.TNurses.Remove(nurse);

            await _context.SaveChangesAsync();
        }
    }
}