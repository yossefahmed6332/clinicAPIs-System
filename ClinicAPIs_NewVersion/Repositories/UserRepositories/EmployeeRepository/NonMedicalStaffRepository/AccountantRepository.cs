using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace clinicAPIsSystem.RepositoryService.UserRepository.NonMedicalStaffRepository
    {
        public class AccountantRepository : IAccountantRepository
        {
        private readonly UserManager<ApplicationUser> _userManager;
            private readonly ClinicDbContext _context;

            public AccountantRepository(
                UserManager<ApplicationUser> userManager,
                ClinicDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            public async Task<(
                Accountant accountant,
                bool addUserRes,
                bool addPasswordRes,
                bool addRoleRes)> CreateAccountantAsync(
                    Accountant accountant,
                    string password)
            {
                var addUserRes =
                    await _userManager.CreateAsync(accountant);

                if (!addUserRes.Succeeded)
                    return (accountant, false, false, false);

                var addPasswordRes =
                    await _userManager.AddPasswordAsync(
                        accountant,
                        password);

                if (!addPasswordRes.Succeeded)
                    return (accountant, true, false, false);

                var addRoleRes =
                    await _userManager.AddToRoleAsync(
                        accountant,
                        UserRole.Accountant.ToString());

                return (
                    accountant,
                    true,
                    true,
                    addRoleRes.Succeeded);
            }

            public async Task<List<Accountant>> GetAllAccountantsAsync()
            {
                return await _context.TAccountants.ToListAsync();
            }

            public async Task<Accountant?> GetAccountantAsync(int id)
            {
                return await _context.TAccountants.FindAsync(id);
            }

            public async Task<Accountant> UpdateAccountantAsync(
                Accountant accountant)
            {
                _context.TAccountants.Update(accountant);

                await _context.SaveChangesAsync();

                return accountant;
            }

            public async Task DeleteAccountantAsync(
                Accountant accountant)
            {
                _context.TAccountants.Remove(accountant);

                await _context.SaveChangesAsync();
            }
        }
    }
