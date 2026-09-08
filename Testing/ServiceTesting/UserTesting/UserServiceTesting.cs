using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IUserRepositories;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Services.UserServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClinicAPIsTestProject.Services.UserServices
{
    public class UserServiceTesting
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;

        private readonly UserService _userService;

        public UserServiceTesting()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }


        // GetUserAsync


        [Fact]
        public async Task GetUser_ExistingId_ReturnsApplicationUserDto()
        {
            // Arrange
            var userId = 1;

            var user = new ApplicationUser();

            var userDto = new Mock<ApplicationUserDto>().Object;

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _userService.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto, result);

            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(user),
                Times.Once);
        }

        [Fact]
        public async Task GetUser_NonExistingId_ReturnsNull()
        {
            // Arrange
            var userId = 999;

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUserDto)null!);

            // Act
            var result = await _userService.GetUserAsync(userId);

            // Assert
            Assert.Null(result);

            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(
                    It.IsAny<ApplicationUser>()),
                Times.Once);
        }


        // GetUserByEmailAsync


        [Fact]
        public async Task GetUserByEmail_ExistingEmail_ReturnsApplicationUserDto()
        {
            // Arrange
            var email = "john@example.com";

            var user = new ApplicationUser();

            var userDto = new Mock<ApplicationUserDto>().Object;

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(email))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _userService.GetUserByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto, result);

            _userRepositoryMock.Verify(
                r => r.GetUserByEmailAsync(email),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(user),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByEmail_NonExistingEmail_ReturnsNull()
        {
            // Arrange
            var email = "notfound@example.com";

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(email))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(
                    It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUserDto)null!);

            // Act
            var result = await _userService.GetUserByEmailAsync(email);

            // Assert
            Assert.Null(result);

            _userRepositoryMock.Verify(
                r => r.GetUserByEmailAsync(email),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(
                    It.IsAny<ApplicationUser>()),
                Times.Once);
        }


        // GetUserByUsernameAsync


        [Fact]
        public async Task GetUserByUsername_ExistingUsername_ReturnsApplicationUserDto()
        {
            // Arrange
            var username = "john";

            var user = new ApplicationUser();

            var userDto = new Mock<ApplicationUserDto>().Object;

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(user))
                .Returns(userDto);

            // Act
            var result = await _userService.GetUserByUsernameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto, result);

            _userRepositoryMock.Verify(
                r => r.GetUserByUsernameAsync(username),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(user),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByUsername_NonExistingUsername_ReturnsNull()
        {
            // Arrange
            var username = "unknown";

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(m => m.Map<ApplicationUserDto>(
                    It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUserDto)null!);

            // Act
            var result = await _userService.GetUserByUsernameAsync(username);

            // Assert
            Assert.Null(result);

            _userRepositoryMock.Verify(
                r => r.GetUserByUsernameAsync(username),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ApplicationUserDto>(
                    It.IsAny<ApplicationUser>()),
                Times.Once);
        }


        // PhoneNumberExistsAsync


        [Fact]
        public async Task PhoneNumberExists_ExistingPhoneNumber_ReturnsTrue()
        {
            // Arrange
            var phoneNumber = "01012345678";

            _userRepositoryMock
                .Setup(r => r.PhoneNumberExistsAsync(phoneNumber))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.PhoneNumberExistsAsync(phoneNumber);

            // Assert
            Assert.True(result);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Once);
        }

        [Fact]
        public async Task PhoneNumberExists_NonExistingPhoneNumber_ReturnsFalse()
        {
            // Arrange
            var phoneNumber = "01099999999";

            _userRepositoryMock
                .Setup(r => r.PhoneNumberExistsAsync(phoneNumber))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.PhoneNumberExistsAsync(phoneNumber);

            // Assert
            Assert.False(result);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Once);
        }


        // EmailExistsAsync


        [Fact]
        public async Task EmailExists_ExistingEmail_ReturnsTrue()
        {
            // Arrange
            var email = "john@example.com";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.EmailExistsAsync(email);

            // Assert
            Assert.True(result);

            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);
        }

        [Fact]
        public async Task EmailExists_NonExistingEmail_ReturnsFalse()
        {
            // Arrange
            var email = "notfound@example.com";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.EmailExistsAsync(email);

            // Assert
            Assert.False(result);

            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);
        }


        // UsernameExistsAsync


        [Fact]
        public async Task UsernameExists_ExistingUsername_ReturnsTrue()
        {
            // Arrange
            var username = "john";

            _userRepositoryMock
                .Setup(r => r.UsernameExistsAsync(username))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.UsernameExistsAsync(username);

            // Assert
            Assert.True(result);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Once);
        }

        [Fact]
        public async Task UsernameExists_NonExistingUsername_ReturnsFalse()
        {
            // Arrange
            var username = "unknown";

            _userRepositoryMock
                .Setup(r => r.UsernameExistsAsync(username))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.UsernameExistsAsync(username);

            // Assert
            Assert.False(result);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Once);
        }


        // ValidateUserCreation


        [Fact]
        public async Task ValidateUserCreation_AllDataAvailable_CompletesSuccessfully()
        {
            // Arrange
            var email = "john@example.com";
            var username = "john";
            var phoneNumber = "01012345678";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.UsernameExistsAsync(username))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.PhoneNumberExistsAsync(phoneNumber))
                .ReturnsAsync(false);

            // Act
            await _userService.ValidateUserCreation(
                email,
                username,
                phoneNumber);

            // Assert
            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Once);
        }

        [Fact]
        public async Task ValidateUserCreation_EmailExists_ThrowsArgumentException()
        {
            // Arrange
            var email = "john@example.com";
            var username = "john";
            var phoneNumber = "01012345678";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.ValidateUserCreation(
                    email,
                    username,
                    phoneNumber));

            Assert.Equal(
                "Email already exists.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Never);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Never);
        }

        [Fact]
        public async Task ValidateUserCreation_UsernameExists_ThrowsArgumentException()
        {
            // Arrange
            var email = "john@example.com";
            var username = "john";
            var phoneNumber = "01012345678";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.UsernameExistsAsync(username))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.ValidateUserCreation(
                    email,
                    username,
                    phoneNumber));

            Assert.Equal(
                "Username already exists.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Never);
        }

        [Fact]
        public async Task ValidateUserCreation_PhoneNumberExists_ThrowsArgumentException()
        {
            // Arrange
            var email = "john@example.com";
            var username = "john";
            var phoneNumber = "01012345678";

            _userRepositoryMock
                .Setup(r => r.EmailExistsAsync(email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.UsernameExistsAsync(username))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(r => r.PhoneNumberExistsAsync(phoneNumber))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.ValidateUserCreation(
                    email,
                    username,
                    phoneNumber));

            Assert.Equal(
                "Phone number already exists.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.EmailExistsAsync(email),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.UsernameExistsAsync(username),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.PhoneNumberExistsAsync(phoneNumber),
                Times.Once);
        }


        // ChangePasswordASync


        [Fact]
        public async Task ChangePassword_ExistingUserAndSuccessfulResult_CompletesSuccessfully()
        {
            // Arrange
            var userId = 1;
            var currentPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            var user = new ApplicationUser();

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(r => r.ChangePasswordAsync(
                    user,
                    currentPassword,
                    newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.ChangePasswordASync(
                userId,
                currentPassword,
                newPassword);

            // Assert
            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.ChangePasswordAsync(
                    user,
                    currentPassword,
                    newPassword),
                Times.Once);
        }

        [Fact]
        public async Task ChangePassword_NonExistingUser_ThrowsArgumentException()
        {
            // Arrange
            var userId = 999;
            var currentPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.ChangePasswordASync(
                    userId,
                    currentPassword,
                    newPassword));

            Assert.Equal(
                "User not found.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.ChangePasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ChangePassword_FailedIdentityResult_ThrowsArgumentException()
        {
            // Arrange
            var userId = 1;
            var currentPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            var user = new ApplicationUser();

            var identityResult = IdentityResult.Failed(
                new IdentityError
                {
                    Description = "Incorrect password."
                },
                new IdentityError
                {
                    Description = "Password is too weak."
                });

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(r => r.ChangePasswordAsync(
                    user,
                    currentPassword,
                    newPassword))
                .ReturnsAsync(identityResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.ChangePasswordASync(
                    userId,
                    currentPassword,
                    newPassword));

            Assert.Equal(
                "Incorrect password., Password is too weak.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.ChangePasswordAsync(
                    user,
                    currentPassword,
                    newPassword),
                Times.Once);
        }


        // GetIdFromTokensAsync


        [Fact]
        public async Task GetIdFromTokens_ValidToken_ReturnsUserId()
        {
            // Arrange
            var userId = 25;

            var token = CreateJwtToken(
                ClaimTypes.NameIdentifier,
                userId.ToString());

            // Act
            var result = await _userService.GetIdFromTokensAsync(token);

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public async Task GetIdFromTokens_TokenWithoutUserIdClaim_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var token = CreateJwtToken(
                ClaimTypes.Email,
                "john@example.com");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _userService.GetIdFromTokensAsync(token));

            Assert.Equal(
                "User ID not found in token.",
                exception.Message);
        }

        [Fact]
        public async Task GetIdFromTokens_InvalidUserIdClaim_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var token = CreateJwtToken(
                ClaimTypes.NameIdentifier,
                "invalid-id");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _userService.GetIdFromTokensAsync(token));

            Assert.Equal(
                "Invalid User ID in token.",
                exception.Message);
        }


        // DeleteUserAsync


        [Fact]
        public async Task DeleteUser_ExistingUser_DeletesSuccessfully()
        {
            // Arrange
            var userId = 1;

            var user = new ApplicationUser();

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(r => r.DeleteUserAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.DeleteUserAsync(user),
                Times.Once);
        }

        [Fact]
        public async Task DeleteUser_NonExistingUser_ThrowsArgumentException()
        {
            // Arrange
            var userId = 999;

            _userRepositoryMock
                .Setup(r => r.GetUserAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _userService.DeleteUserAsync(userId));

            Assert.Equal(
                "User not found.",
                exception.Message);

            _userRepositoryMock.Verify(
                r => r.GetUserAsync(userId),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.DeleteUserAsync(
                    It.IsAny<ApplicationUser>()),
                Times.Never);
        }


        // Helper


        private static string CreateJwtToken(
            string claimType,
            string claimValue)
        {
            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(claimType, claimValue)
                });

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
