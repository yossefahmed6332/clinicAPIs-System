using clinicAPIsSystem.IUserRepositories;
using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.Repositories.UserRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> GetUserAsync(int id)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email ==email);
        }
        public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(usrnm=>usrnm.UserName == username);
        }
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return (await _userManager.FindByEmailAsync(email)) != null;
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return (await _userManager.FindByNameAsync(username)) != null;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user,string currentPassword,  string newPassword )
        {
            var result = await _userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword
            );

            return result; 
        }

        public async Task DeleteUserAsync(ApplicationUser user) {
            await _userManager.DeleteAsync(user); 
        }

    }
}
