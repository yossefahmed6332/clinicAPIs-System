using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace clinicAPIsSystem.RepositoryService.UserRepository
{
    public class AdminRepository:IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClinicDbContext _context; 
        public AdminRepository (UserManager<ApplicationUser> userManager, ClinicDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<(
            Admin admin,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateAdminAsync(
                Admin admin,
                string password)
        {
            var addUserRes = await _userManager.CreateAsync(admin);

            if (!addUserRes.Succeeded)
            {
                return (admin, false, false, false);
            }

            var addPasswordRes =
                await _userManager.AddPasswordAsync(admin, password);

            if (!addPasswordRes.Succeeded)
            {
                return (admin, true, false, false);
            }

            var addRoleRes =
                await _userManager.AddToRoleAsync(
                    admin,
                    UserRole.Admin.ToString());

            return (
                admin,
                true,
                true,
                addRoleRes.Succeeded);
        }

        public async Task<List<Admin>> GetAllAdminsAsync()
        {
            return await _context.TAdmins.ToListAsync(); 
        }

        public async Task<Admin?> GetAdminAsync(int id)
        {
            return await  _context.TAdmins.FirstOrDefaultAsync(a=>a.Id==id);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
              _context.TAdmins.Update(admin); 
            await _context.SaveChangesAsync();
            return admin; 
        }

        public async Task DeleteAdminAsync(Admin admin)
        {
             _context.TAdmins.Remove(admin);
            await _context.SaveChangesAsync();


        }


    }
}
