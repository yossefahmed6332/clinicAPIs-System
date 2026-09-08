using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace clinicAPIsSystem.Data.Seeder
{
    public  class AdminSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SeedAsync()
        {
             string email = "admin@clinic.com";

            var admin = await _userManager.FindByEmailAsync(email);

            if (admin != null)
                return;

            admin = new Admin("System ","Administrator","admin", "admin@clinic.com","0122222222");
          

            var result = await _userManager.CreateAsync(admin, "Admin@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            }
        }



    }
}
