using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.AccountantDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.NonMedicalStaffServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices.NonMedicalStaffServices
{
    public class AccountantServiceTesting
    {
        private readonly Mock<IAccountantRepository> _accountantRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AccountantService>> _loggerMock;

        private readonly AccountantService _accountantService;

        public AccountantServiceTesting()
        {
            _accountantRepositoryMock = new Mock<IAccountantRepository>();
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AccountantService>>();

            _accountantService = new AccountantService(
                _accountantRepositoryMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateAccountant_HappyPath_ReturnsAccountantDto()
        {
            // Arrange
            var createAccountantDto = new CreateAccountantDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedaccountant",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Accounting",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "ACC123"
            };

            var accountant = new Accountant(
                createAccountantDto.FirstName,
                createAccountantDto.LastName,
                createAccountantDto.UserName,
                createAccountantDto.Email,
                createAccountantDto.PhoneNumber,
                createAccountantDto.SalaryPerHour,
                createAccountantDto.HoursWorked,
                createAccountantDto.ShiftStart,
                createAccountantDto.ShiftEnd,
                createAccountantDto.Degree,
                createAccountantDto.University,
                createAccountantDto.YearsOfExperience,
                createAccountantDto.GraduationYear,
                createAccountantDto.License);

            var accountantDto = new AccountantDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _accountantRepositoryMock
                .Setup(x => x.CreateAccountantAsync(
                    It.IsAny<Accountant>(),
                    createAccountantDto.Password))
                .ReturnsAsync((accountant, true, true, true));

            _mapperMock
                .Setup(x => x.Map<AccountantDto>(accountant))
                .Returns(accountantDto);

            // Act
            var result =
                await _accountantService.CreateAccountantAsync(
                    createAccountantDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountantDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.CreateAccountantAsync(
                    It.IsAny<Accountant>(),
                    createAccountantDto.Password),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(accountant),
                Times.Once);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAccountant_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createAccountantDto = new CreateAccountantDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedaccountant",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Accounting",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "ACC123"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber))
                .ThrowsAsync(
                    new ArgumentException("Email already exists."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _accountantService.CreateAccountantAsync(
                    createAccountantDto));

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.CreateAccountantAsync(
                    It.IsAny<Accountant>(),
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(
                    It.IsAny<Accountant>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAccountant_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createAccountantDto = new CreateAccountantDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedaccountant",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Accounting",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "ACC123"
            };

            var accountant = new Accountant(
                createAccountantDto.FirstName,
                createAccountantDto.LastName,
                createAccountantDto.UserName,
                createAccountantDto.Email,
                createAccountantDto.PhoneNumber,
                createAccountantDto.SalaryPerHour,
                createAccountantDto.HoursWorked,
                createAccountantDto.ShiftStart,
                createAccountantDto.ShiftEnd,
                createAccountantDto.Degree,
                createAccountantDto.University,
                createAccountantDto.YearsOfExperience,
                createAccountantDto.GraduationYear,
                createAccountantDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _accountantRepositoryMock
                .Setup(x => x.CreateAccountantAsync(
                    It.IsAny<Accountant>(),
                    createAccountantDto.Password))
                .ReturnsAsync((accountant, true, false, true));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(accountant.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _accountantService.CreateAccountantAsync(
                        createAccountantDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(accountant.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(
                    It.IsAny<Accountant>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAccountant_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createAccountantDto = new CreateAccountantDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedaccountant",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Accounting",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "ACC123"
            };

            var accountant = new Accountant(
                createAccountantDto.FirstName,
                createAccountantDto.LastName,
                createAccountantDto.UserName,
                createAccountantDto.Email,
                createAccountantDto.PhoneNumber,
                createAccountantDto.SalaryPerHour,
                createAccountantDto.HoursWorked,
                createAccountantDto.ShiftStart,
                createAccountantDto.ShiftEnd,
                createAccountantDto.Degree,
                createAccountantDto.University,
                createAccountantDto.YearsOfExperience,
                createAccountantDto.GraduationYear,
                createAccountantDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAccountantDto.Email,
                    createAccountantDto.UserName,
                    createAccountantDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _accountantRepositoryMock
                .Setup(x => x.CreateAccountantAsync(
                    It.IsAny<Accountant>(),
                    createAccountantDto.Password))
                .ReturnsAsync((accountant, true, true, false));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(accountant.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _accountantService.CreateAccountantAsync(
                        createAccountantDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(accountant.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(
                    It.IsAny<Accountant>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAccountant_ExistingId_ReturnsAccountantDto()
        {
            // Arrange
            var accountantId = 1;

            var accountant = new Mock<Accountant>().Object;
            var accountantDto = new AccountantDto();

            _accountantRepositoryMock
                .Setup(x => x.GetAccountantAsync(accountantId))
                .ReturnsAsync(accountant);

            _mapperMock
                .Setup(x => x.Map<AccountantDto>(accountant))
                .Returns(accountantDto);

            // Act
            var result =
                await _accountantService.GetAccountantAsync(
                    accountantId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountantDto, result);

            _accountantRepositoryMock.Verify(
                x => x.GetAccountantAsync(accountantId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(accountant),
                Times.Once);
        }

        [Fact]
        public async Task GetAccountant_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var accountantId = 999;

            _accountantRepositoryMock
                .Setup(x => x.GetAccountantAsync(accountantId))
                .ReturnsAsync((Accountant?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _accountantService.GetAccountantAsync(
                        accountantId));

            Assert.Equal(
                $"Cannot find accountant with id {accountantId}",
                exception.Message);

            _accountantRepositoryMock.Verify(
                x => x.GetAccountantAsync(accountantId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(
                    It.IsAny<Accountant>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllAccounts_HappyPath_ReturnsListOfAccountantDto()
        {
            // Arrange
            var accountants = new List<Accountant>
            {
                new Mock<Accountant>().Object,
                new Mock<Accountant>().Object
            };

            var accountantDtos = new List<AccountantDto>
            {
                new AccountantDto(),
                new AccountantDto()
            };

            _accountantRepositoryMock
                .Setup(x => x.GetAllAccountantsAsync())
                .ReturnsAsync(accountants);

            _mapperMock
                .Setup(x => x.Map<List<AccountantDto>>(accountants))
                .Returns(accountantDtos);

            // Act
            var result =
                await _accountantService.GetAllAccountsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountantDtos, result);

            _accountantRepositoryMock.Verify(
                x => x.GetAllAccountantsAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<AccountantDto>>(accountants),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAccounts_NoAccountants_ReturnsEmptyList()
        {
            // Arrange
            var accountants = new List<Accountant>();
            var accountantDtos = new List<AccountantDto>();

            _accountantRepositoryMock
                .Setup(x => x.GetAllAccountantsAsync())
                .ReturnsAsync(accountants);

            _mapperMock
                .Setup(x => x.Map<List<AccountantDto>>(accountants))
                .Returns(accountantDtos);

            // Act
            var result =
                await _accountantService.GetAllAccountsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _accountantRepositoryMock.Verify(
                x => x.GetAllAccountantsAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<AccountantDto>>(accountants),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAccountant_ExistingId_ReturnsUpdatedAccountantDto()
        {
            // Arrange
            var accountantId = 1;

            var updateAccountantDto = new UpdateAccountantDto
            {
                FirstName = "Updated",
                LastName = "Accountant",
                UserName = "updatedaccountant",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 180m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Advanced Accounting",
                University = "Ain Shams University",
                YearsOfExperience = 7,
                GraduationYear = 2018,
                License = "ACC999"
            };

            var accountant = new Mock<Accountant>().Object;
            var updatedAccountant = new Mock<Accountant>().Object;
            var accountantDto = new AccountantDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateAccountantDto.Email,
                    updateAccountantDto.UserName,
                    updateAccountantDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _accountantRepositoryMock
                .Setup(x => x.GetAccountantAsync(accountantId))
                .ReturnsAsync(accountant);

            _accountantRepositoryMock
                .Setup(x => x.UpdateAccountantAsync(accountant))
                .ReturnsAsync(updatedAccountant);

            _mapperMock
                .Setup(x => x.Map<AccountantDto>(updatedAccountant))
                .Returns(accountantDto);

            // Act
            var result =
                await _accountantService.UpdateAccountantAsync(
                    updateAccountantDto,
                    accountantId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountantDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateAccountantDto.Email,
                    updateAccountantDto.UserName,
                    updateAccountantDto.PhoneNumber),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.GetAccountantAsync(accountantId),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.UpdateAccountantAsync(accountant),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(updatedAccountant),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAccountant_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var accountantId = 999;

            var updateAccountantDto = new UpdateAccountantDto
            {
                FirstName = "Updated",
                LastName = "Accountant",
                UserName = "updatedaccountant",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 180m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Advanced Accounting",
                University = "Ain Shams University",
                YearsOfExperience = 7,
                GraduationYear = 2018,
                License = "ACC999"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateAccountantDto.Email,
                    updateAccountantDto.UserName,
                    updateAccountantDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _accountantRepositoryMock
                .Setup(x => x.GetAccountantAsync(accountantId))
                .ReturnsAsync((Accountant?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _accountantService.UpdateAccountantAsync(
                        updateAccountantDto,
                        accountantId));

            Assert.Equal(
                $"Cannot find accountant with id {accountantId}",
                exception.Message);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateAccountantDto.Email,
                    updateAccountantDto.UserName,
                    updateAccountantDto.PhoneNumber),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.GetAccountantAsync(accountantId),
                Times.Once);

            _accountantRepositoryMock.Verify(
                x => x.UpdateAccountantAsync(
                    It.IsAny<Accountant>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<AccountantDto>(
                    It.IsAny<Accountant>()),
                Times.Never);
        }
    }
}
