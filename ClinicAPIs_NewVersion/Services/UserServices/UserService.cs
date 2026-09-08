using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IUserRepositories;
using clinicAPIsSystem.Models.User;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace clinicAPIsSystem.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService( IUserRepository userRepository, IMapper mapper,ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApplicationUserDto?> GetUserAsync(int id)
        {
            _logger
                .LogDebug("Retrieving user with ID {UserId} from the repository.", id);
            return _mapper.Map<ApplicationUserDto>(await _userRepository.GetUserAsync(id));
        }
        public async Task<ApplicationUserDto?> GetUserByEmailAsync(string email)
        {
            _logger
                .LogDebug("Retrieving user with Email {UserEmail} from the repository.", email);
            return _mapper.Map<ApplicationUserDto>(await _userRepository.GetUserByEmailAsync(email));
        }
        public async Task<ApplicationUserDto?> GetUserByUsernameAsync(string username)
        {
            _logger
                .LogDebug("Retrieving user with Username {Username} from the repository.", username);
            return _mapper.Map<ApplicationUserDto>(await _userRepository.GetUserByUsernameAsync(username));
        }
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            _logger
                .LogDebug("Checking if phone number {PhoneNumber} exists in the repository.", phoneNumber);
            return await _userRepository.PhoneNumberExistsAsync(phoneNumber);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            _logger
                .LogDebug("Checking if email {Email} exists in the repository.", email);
            return await _userRepository.EmailExistsAsync(email);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            _logger
                .LogDebug("Checking if username {Username} exists in the repository.", username); 
            return await _userRepository.UsernameExistsAsync(username);
        }

        public async Task ValidateUserCreation(string email, string username, string phoneNumber)
        {
            _logger
                .LogDebug("Validating user creation with Email {Email}, Username {Username}, and Phone Number {PhoneNumber}.", email, username, phoneNumber);
            // Check if the email already exists
            if (await EmailExistsAsync(email))
            {
                _logger.LogWarning("Email {Email} already exists in the repository.", email);
                throw new ArgumentException("Email already exists.");
            }
            //check if user name is exist 
            if (await UsernameExistsAsync(username))
            {
                _logger.LogWarning("Username {Username} already exists in the repository.", username);
                throw new ArgumentException("Username already exists.");
            }
            //check if phone number is exist
            if (await PhoneNumberExistsAsync(phoneNumber))
            {
                _logger.LogWarning("Phone number {PhoneNumber} already exists in the repository.", phoneNumber);
                throw new ArgumentException("Phone number already exists.");
            }
        }
        public async Task ChangePasswordASync(int id, string currentPassword, string newPassword)
        {
            _logger.LogDebug("Changing password for user with ID {UserId}.", id);
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found in the repository.", id);
                throw new ArgumentException("User not found.");
            }
            var result = await _userRepository.ChangePasswordAsync(
                     user,
                     currentPassword,
                      newPassword
                    );

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Failed to change password: {result}");
                throw new ArgumentException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

        }
        public async Task<int> GetIdFromTokensAsync(string token)
        {
            _logger.LogInformation("Getting ID from tokens");
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) {
                _logger.LogWarning("User ID claim not found in token.");
                throw new UnauthorizedAccessException("User ID not found in token.");

            }

            if (!int.TryParse(userIdClaim.Value, out var userId)) { 
                _logger.LogWarning("Invalid User ID in token.");
                throw new UnauthorizedAccessException("Invalid User ID in token.");
            }

            return userId;
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger
                .LogInformation("Delete User with ID {UserId}", id);    
            _logger
                .LogDebug("Retrieving user with ID {UserId} from the repository.", id);
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                _logger
                    .LogWarning("User with ID {UserId} not found in the repository.", id);
                throw new ArgumentException("User not found.");
            }
            await _userRepository.DeleteUserAsync(user);
            _logger
                .LogInformation("User with ID {UserId} deleted successfully.", id);
        }

    }
}
