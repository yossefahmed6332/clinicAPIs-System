using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO;
namespace clinicAPIsSystem.IServices.IUserServices
{
    public interface IUserService
    {
        //validate user 
        public Task ValidateUserCreation (string email, string username, string phoneNumber);
        //get user 
        public Task<ApplicationUserDto?> GetUserAsync(int id);

        public Task<ApplicationUserDto?> GetUserByEmailAsync(string email);

        public Task<ApplicationUserDto?> GetUserByUsernameAsync(string username);

        public Task<int> GetIdFromTokensAsync(string tokens); 
        
        //this is to check if user exists
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);

        public Task<bool> EmailExistsAsync(string email);

        public Task<bool> UsernameExistsAsync(string username);
        public Task ChangePasswordASync(int id, string currentPassword , string newPassword);
        //Delete user 
        public Task DeleteUserAsync (int id);
    }
}
