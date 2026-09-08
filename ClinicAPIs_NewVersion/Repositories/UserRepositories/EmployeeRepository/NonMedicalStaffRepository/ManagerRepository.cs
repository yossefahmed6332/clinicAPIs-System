using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.RepositoryService.UserRepository.NonMedicalStaffRepository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context;

        public ManagerRepository(
            UserManager<ApplicationUser> userManager,
            ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(
            Manager manager,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateManagerAsync(
                Manager manager,
                string password)
        {
            var addUserRes =
                await _userManager.CreateAsync(manager);

            if (!addUserRes.Succeeded)
                return (manager, false, false, false);

            var addPasswordRes =
                await _userManager.AddPasswordAsync(
                    manager,
                    password);

            if (!addPasswordRes.Succeeded)
                return (manager, true, false, false);

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    manager,
                    UserRole.Manager.ToString());

            return (
                manager,
                true,
                true,
                addRoleRes.Succeeded);
        }

        public async Task<List<Manager>> GetAllManagersAsync()
        {
            return await _context.TManagers.ToListAsync();
        }

        public async Task<Manager?> GetManagerAsync(int id)
        {
            return await _context.TManagers.FindAsync(id);
        }

        public async Task<Manager> UpdateManagerAsync(
            Manager manager)
        {
            _context.TManagers.Update(manager);

            await _context.SaveChangesAsync();

            return manager;
        }

        public async Task DeleteManagerAsync(
            Manager manager)
        {
            _context.TManagers.Remove(manager);

            await _context.SaveChangesAsync();
        }
    }
}