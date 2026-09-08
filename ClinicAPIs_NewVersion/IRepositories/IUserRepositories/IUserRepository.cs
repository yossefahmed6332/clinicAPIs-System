using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
namespace clinicAPIsSystem.IUserRepositories
{
   
    public interface IUserRepository
    {
        //get user 
        public Task<ApplicationUser?> GetUserAsync(int id);

        public Task<ApplicationUser?> GetUserByEmailAsync(string email);

        public Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        //this is to check if user exists
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);

        public Task<bool> EmailExistsAsync(string email);

        public Task<bool> UsernameExistsAsync(string username);
        public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        //delete user 
        public Task DeleteUserAsync (ApplicationUser user);
        
    }
}
