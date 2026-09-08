using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace clinicAPIsSystem.Data.Seeder
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public RoleSeeder(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            string[] UserUserRole =
            {
            UserRole.Admin.ToString(),
            UserRole.Doctor.ToString(),
            UserRole.Nurse.ToString(),
            UserRole.Patient.ToString(),
            UserRole.Accountant.ToString(),
            UserRole.Receptionist.ToString(),
            UserRole.Manager.ToString(),
            UserRole.Cleaner.ToString()
            };

            foreach (var role in UserUserRole)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(role: new IdentityRole<int>(role)); 
                }
            }
        }
    }
}
