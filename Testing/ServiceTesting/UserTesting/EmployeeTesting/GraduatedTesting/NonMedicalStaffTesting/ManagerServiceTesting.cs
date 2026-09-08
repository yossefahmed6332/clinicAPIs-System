using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ManagerDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.NonMedicalStaffServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices.NonMedicalStaffServices
{
    public class ManagerServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IManagerRepository> _managerRepositoryMock;
        private readonly Mock<ILogger<ManagerService>> _loggerMock;

        private readonly ManagerService _managerService;

        public ManagerServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _managerRepositoryMock = new Mock<IManagerRepository>();
            _loggerMock = new Mock<ILogger<ManagerService>>();

            _managerService = new ManagerService(
                _mapperMock.Object,
                _userServiceMock.Object,
                _managerRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateManager_HappyPath_ReturnsManagerDto()
        {
            // Arrange
            var createManagerDto = new CreateManagerDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedmanager",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Business Administration",
                University = "Cairo University",
                YearsOfExperience = 8,
                GraduationYear = 2017,
                License = "MNG123"
            };

            var manager = new Manager(
                createManagerDto.FirstName,
                createManagerDto.LastName,
                createManagerDto.UserName,
                createManagerDto.Email,
                createManagerDto.PhoneNumber,
                createManagerDto.SalaryPerHour,
                createManagerDto.HoursWorked,
                createManagerDto.ShiftStart,
                createManagerDto.ShiftEnd,
                createManagerDto.Degree,
                createManagerDto.University,
                createManagerDto.YearsOfExperience,
                createManagerDto.GraduationYear,
                createManagerDto.License);

            var managerDto = new ManagerDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _managerRepositoryMock
                .Setup(x => x.CreateManagerAsync(
                    It.IsAny<Manager>(),
                    createManagerDto.Password))
                .ReturnsAsync((manager, true, true, true));

            _mapperMock
                .Setup(x => x.Map<ManagerDto>(manager))
                .Returns(managerDto);

            // Act
            var result =
                await _managerService.CreateManagerAsync(
                    createManagerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(managerDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.CreateManagerAsync(
                    It.IsAny<Manager>(),
                    createManagerDto.Password),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(manager),
                Times.Once);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateManager_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createManagerDto = new CreateManagerDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedmanager",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Business Administration",
                University = "Cairo University",
                YearsOfExperience = 8,
                GraduationYear = 2017,
                License = "MNG123"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber))
                .ThrowsAsync(
                    new ArgumentException("Email already exists."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _managerService.CreateManagerAsync(
                    createManagerDto));

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.CreateManagerAsync(
                    It.IsAny<Manager>(),
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(
                    It.IsAny<Manager>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateManager_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createManagerDto = new CreateManagerDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedmanager",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Business Administration",
                University = "Cairo University",
                YearsOfExperience = 8,
                GraduationYear = 2017,
                License = "MNG123"
            };

            var manager = new Manager(
                createManagerDto.FirstName,
                createManagerDto.LastName,
                createManagerDto.UserName,
                createManagerDto.Email,
                createManagerDto.PhoneNumber,
                createManagerDto.SalaryPerHour,
                createManagerDto.HoursWorked,
                createManagerDto.ShiftStart,
                createManagerDto.ShiftEnd,
                createManagerDto.Degree,
                createManagerDto.University,
                createManagerDto.YearsOfExperience,
                createManagerDto.GraduationYear,
                createManagerDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _managerRepositoryMock
                .Setup(x => x.CreateManagerAsync(
                    It.IsAny<Manager>(),
                    createManagerDto.Password))
                .ReturnsAsync((manager, true, false, true));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(manager.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _managerService.CreateManagerAsync(
                        createManagerDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(manager.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(
                    It.IsAny<Manager>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateManager_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createManagerDto = new CreateManagerDto
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                UserName = "ahmedmanager",
                Email = "ahmed@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Business Administration",
                University = "Cairo University",
                YearsOfExperience = 8,
                GraduationYear = 2017,
                License = "MNG123"
            };

            var manager = new Manager(
                createManagerDto.FirstName,
                createManagerDto.LastName,
                createManagerDto.UserName,
                createManagerDto.Email,
                createManagerDto.PhoneNumber,
                createManagerDto.SalaryPerHour,
                createManagerDto.HoursWorked,
                createManagerDto.ShiftStart,
                createManagerDto.ShiftEnd,
                createManagerDto.Degree,
                createManagerDto.University,
                createManagerDto.YearsOfExperience,
                createManagerDto.GraduationYear,
                createManagerDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createManagerDto.Email,
                    createManagerDto.UserName,
                    createManagerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _managerRepositoryMock
                .Setup(x => x.CreateManagerAsync(
                    It.IsAny<Manager>(),
                    createManagerDto.Password))
                .ReturnsAsync((manager, true, true, false));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(manager.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _managerService.CreateManagerAsync(
                        createManagerDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(manager.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(
                    It.IsAny<Manager>()),
                Times.Never);
        }

        [Fact]
        public async Task GetManager_ExistingId_ReturnsManagerDto()
        {
            // Arrange
            var managerId = 1;

            var manager = new Mock<Manager>().Object;
            var managerDto = new ManagerDto();

            _managerRepositoryMock
                .Setup(x => x.GetManagerAsync(managerId))
                .ReturnsAsync(manager);

            _mapperMock
                .Setup(x => x.Map<ManagerDto>(manager))
                .Returns(managerDto);

            // Act
            var result =
                await _managerService.GetManagerAsync(managerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(managerDto, result);

            _managerRepositoryMock.Verify(
                x => x.GetManagerAsync(managerId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(manager),
                Times.Once);
        }

        [Fact]
        public async Task GetManager_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var managerId = 999;

            _managerRepositoryMock
                .Setup(x => x.GetManagerAsync(managerId))
                .ReturnsAsync((Manager?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _managerService.GetManagerAsync(managerId));

            Assert.Equal(
                $"Cannot find user with ID{managerId}",
                exception.Message);

            _managerRepositoryMock.Verify(
                x => x.GetManagerAsync(managerId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(
                    It.IsAny<Manager>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllManagers_HappyPath_ReturnsListOfManagerDto()
        {
            // Arrange
            var managers = new List<Manager>
            {
                new Mock<Manager>().Object,
                new Mock<Manager>().Object
            };

            var managerDtos = new List<ManagerDto>
            {
                new ManagerDto(),
                new ManagerDto()
            };

            _managerRepositoryMock
                .Setup(x => x.GetAllManagersAsync())
                .ReturnsAsync(managers);

            _mapperMock
                .Setup(x => x.Map<List<ManagerDto>>(managers))
                .Returns(managerDtos);

            // Act
            var result =
                await _managerService.GetAllManagersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(managerDtos, result);

            _managerRepositoryMock.Verify(
                x => x.GetAllManagersAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<ManagerDto>>(managers),
                Times.Once);
        }

        [Fact]
        public async Task GetAllManagers_NoManagers_ReturnsEmptyList()
        {
            // Arrange
            var managers = new List<Manager>();
            var managerDtos = new List<ManagerDto>();

            _managerRepositoryMock
                .Setup(x => x.GetAllManagersAsync())
                .ReturnsAsync(managers);

            _mapperMock
                .Setup(x => x.Map<List<ManagerDto>>(managers))
                .Returns(managerDtos);

            // Act
            var result =
                await _managerService.GetAllManagersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _managerRepositoryMock.Verify(
                x => x.GetAllManagersAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<ManagerDto>>(managers),
                Times.Once);
        }

        [Fact]
        public async Task UpdateManager_ExistingId_ReturnsUpdatedManagerDto()
        {
            // Arrange
            var managerId = 1;

            var updateManagerDto = new UpdateManagerDto
            {
                FirstName = "Updated",
                LastName = "Manager",
                UserName = "updatedmanager",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 300m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "MBA",
                University = "Ain Shams University",
                YearsOfExperience = 10,
                GraduationYear = 2015,
                License = "MNG999"
            };

            var manager = new Mock<Manager>().Object;
            var updatedManager = new Mock<Manager>().Object;
            var managerDto = new ManagerDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateManagerDto.Email,
                    updateManagerDto.UserName,
                    updateManagerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _managerRepositoryMock
                .Setup(x => x.GetManagerAsync(managerId))
                .ReturnsAsync(manager);

            _managerRepositoryMock
                .Setup(x => x.UpdateManagerAsync(manager))
                .ReturnsAsync(updatedManager);

            _mapperMock
                .Setup(x => x.Map<ManagerDto>(updatedManager))
                .Returns(managerDto);

            // Act
            var result =
                await _managerService.UpdateManagerAsync(
                    updateManagerDto,
                    managerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(managerDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateManagerDto.Email,
                    updateManagerDto.UserName,
                    updateManagerDto.PhoneNumber),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.GetManagerAsync(managerId),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.UpdateManagerAsync(manager),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(updatedManager),
                Times.Once);
        }

        [Fact]
        public async Task UpdateManager_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var managerId = 999;

            var updateManagerDto = new UpdateManagerDto
            {
                FirstName = "Updated",
                LastName = "Manager",
                UserName = "updatedmanager",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 300m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "MBA",
                University = "Ain Shams University",
                YearsOfExperience = 10,
                GraduationYear = 2015,
                License = "MNG999"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateManagerDto.Email,
                    updateManagerDto.UserName,
                    updateManagerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _managerRepositoryMock
                .Setup(x => x.GetManagerAsync(managerId))
                .ReturnsAsync((Manager?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _managerService.UpdateManagerAsync(
                        updateManagerDto,
                        managerId));

            Assert.Equal(
                $"User with ID{managerId} not found",
                exception.Message);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateManagerDto.Email,
                    updateManagerDto.UserName,
                    updateManagerDto.PhoneNumber),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.GetManagerAsync(managerId),
                Times.Once);

            _managerRepositoryMock.Verify(
                x => x.UpdateManagerAsync(
                    It.IsAny<Manager>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<ManagerDto>(
                    It.IsAny<Manager>()),
                Times.Never);
        }
    }
}
