using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace clinicAPIsSystem.RepositoryService.UserRepository.EmployeeRepository
{
    public class CleanerRepository:ICleanerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context; 
        public CleanerRepository (UserManager<ApplicationUser> userManager, ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<(
            Cleaner cleaner,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateCleanerAsync(
                Cleaner cleaner,
                string password)
        {
            var addUserRes = await _userManager.CreateAsync(cleaner);

            if (!addUserRes.Succeeded)
            {
                return (cleaner, false, false, false);
            }

            var addPasswordRes =
                await _userManager.AddPasswordAsync(cleaner, password);

            if (!addPasswordRes.Succeeded)
            {
                return (cleaner, true, false, false);
            }

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    cleaner,
                    UserRole.Cleaner.ToString());

            return (
                cleaner,
                true,
                true,
                addRoleRes.Succeeded);
        }

         
        public async Task<List<Cleaner>> GetAllCleanersAsync()
        {
            return await _context.TCleaners.ToListAsync(); 
        }

        public async Task<Cleaner?> GetCleanerAsync(int id)
        {
            return await _context.TCleaners.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Cleaner> UpdateCleanerAsync (Cleaner cleaner)
        {
            _context.TCleaners.Update(cleaner);
            await _context.SaveChangesAsync();

            return cleaner;
        }

        public async Task DeleteCleanerAsync(Cleaner cleaner)
        {
             _context.TCleaners.Remove(cleaner);
            await _context.SaveChangesAsync();
        }
    }
}
