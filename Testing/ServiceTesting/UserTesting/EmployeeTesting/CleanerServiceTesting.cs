using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.CleanerDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User.Employee;
using clinicAPIsSystem.Services.UserServices.EmployeeServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices
{
    public class CleanerServiceTesting
    {
        private readonly Mock<ICleanerRepository> _cleanerRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CleanerService>> _loggerMock;

        private readonly CleanerService _cleanerService;

        public CleanerServiceTesting()
        {
            _cleanerRepositoryMock = new Mock<ICleanerRepository>();
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CleanerService>>();

            _cleanerService = new CleanerService(
                _cleanerRepositoryMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }


        // CreateCleanerAsync


        [Fact]
        public async Task CreateCleaner_HappyPath_ReturnsCleanerDto()
        {
            // Arrange
            var createCleanerDto = new CreateCleanerDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.cleaner",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 100,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0)
            };

            var cleaner = new Cleaner(
                createCleanerDto.FirstName,
                createCleanerDto.LastName,
                createCleanerDto.UserName,
                createCleanerDto.Email,
                createCleanerDto.PhoneNumber,
                createCleanerDto.SalaryPerHour,
                createCleanerDto.HoursWorked,
                createCleanerDto.ShiftStart,
                createCleanerDto.ShiftEnd
            );

            var cleanerDto = new CleanerDto();

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createCleanerDto.Email,
                    createCleanerDto.UserName,
                    createCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.CreateCleanerAsync(
                    It.IsAny<Cleaner>(),
                    createCleanerDto.Password))
                .ReturnsAsync((cleaner, true, true, true));

            _mapperMock
                .Setup(m => m.Map<CleanerDto>(cleaner))
                .Returns(cleanerDto);

            // Act
            var result = await _cleanerService.CreateCleanerAsync(
                createCleanerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cleanerDto, result);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createCleanerDto.Email,
                    createCleanerDto.UserName,
                    createCleanerDto.PhoneNumber),
                Times.Once);

            _cleanerRepositoryMock.Verify(
                r => r.CreateCleanerAsync(
                    It.IsAny<Cleaner>(),
                    createCleanerDto.Password),
                Times.Once);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(cleaner),
                Times.Once);
        }

        [Fact]
        public async Task CreateCleaner_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createCleanerDto = new CreateCleanerDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.cleaner",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 100,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0)
            };

            var cleaner = new Cleaner(
                createCleanerDto.FirstName,
                createCleanerDto.LastName,
                createCleanerDto.UserName,
                createCleanerDto.Email,
                createCleanerDto.PhoneNumber,
                createCleanerDto.SalaryPerHour,
                createCleanerDto.HoursWorked,
                createCleanerDto.ShiftStart,
                createCleanerDto.ShiftEnd
            );

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createCleanerDto.Email,
                    createCleanerDto.UserName,
                    createCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.CreateCleanerAsync(
                    It.IsAny<Cleaner>(),
                    createCleanerDto.Password))
                .ReturnsAsync((cleaner, false, false, false));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _cleanerService.CreateCleanerAsync(
                    createCleanerDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(
                    It.IsAny<Cleaner>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateCleaner_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createCleanerDto = new CreateCleanerDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.cleaner",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 100,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0)
            };

            var cleaner = new Cleaner(
                createCleanerDto.FirstName,
                createCleanerDto.LastName,
                createCleanerDto.UserName,
                createCleanerDto.Email,
                createCleanerDto.PhoneNumber,
                createCleanerDto.SalaryPerHour,
                createCleanerDto.HoursWorked,
                createCleanerDto.ShiftStart,
                createCleanerDto.ShiftEnd
            );

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createCleanerDto.Email,
                    createCleanerDto.UserName,
                    createCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.CreateCleanerAsync(
                    It.IsAny<Cleaner>(),
                    createCleanerDto.Password))
                .ReturnsAsync((cleaner, true, false, false));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(cleaner.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _cleanerService.CreateCleanerAsync(
                    createCleanerDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(cleaner.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(
                    It.IsAny<Cleaner>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateCleaner_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createCleanerDto = new CreateCleanerDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.cleaner",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 100,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0)
            };

            var cleaner = new Cleaner(
                createCleanerDto.FirstName,
                createCleanerDto.LastName,
                createCleanerDto.UserName,
                createCleanerDto.Email,
                createCleanerDto.PhoneNumber,
                createCleanerDto.SalaryPerHour,
                createCleanerDto.HoursWorked,
                createCleanerDto.ShiftStart,
                createCleanerDto.ShiftEnd
            );

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createCleanerDto.Email,
                    createCleanerDto.UserName,
                    createCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.CreateCleanerAsync(
                    It.IsAny<Cleaner>(),
                    createCleanerDto.Password))
                .ReturnsAsync((cleaner, true, true, false));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(cleaner.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _cleanerService.CreateCleanerAsync(
                    createCleanerDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(cleaner.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(
                    It.IsAny<Cleaner>()),
                Times.Never);
        }


        // GetAllCleanersAsync


        [Fact]
        public async Task GetAllCleaners_HappyPath_ReturnsListOfCleanerDto()
        {
            // Arrange
            var cleaners = new List<Cleaner>
            {
                new Cleaner(
                    "John",
                    "Doe",
                    "john",
                    "john@example.com",
                    "01012345678",
                    100,
                    8,
                    new TimeOnly(8, 0),
                    new TimeOnly(16, 0)),

                new Cleaner(
                    "Jane",
                    "Doe",
                    "jane",
                    "jane@example.com",
                    "01098765432",
                    120,
                    8,
                    new TimeOnly(9, 0),
                    new TimeOnly(17, 0))
            };

            var cleanerDtos = new List<CleanerDto>
            {
                new CleanerDto(),
                new CleanerDto()
            };

            _cleanerRepositoryMock
                .Setup(r => r.GetAllCleanersAsync())
                .ReturnsAsync(cleaners);

            _mapperMock
                .Setup(m => m.Map<List<CleanerDto>>(cleaners))
                .Returns(cleanerDtos);

            // Act
            var result = await _cleanerService.GetAllCleanersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            _cleanerRepositoryMock.Verify(
                r => r.GetAllCleanersAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<CleanerDto>>(cleaners),
                Times.Once);
        }

        [Fact]
        public async Task GetAllCleaners_NoCleaners_ReturnsEmptyList()
        {
            // Arrange
            var cleaners = new List<Cleaner>();
            var cleanerDtos = new List<CleanerDto>();

            _cleanerRepositoryMock
                .Setup(r => r.GetAllCleanersAsync())
                .ReturnsAsync(cleaners);

            _mapperMock
                .Setup(m => m.Map<List<CleanerDto>>(cleaners))
                .Returns(cleanerDtos);

            // Act
            var result = await _cleanerService.GetAllCleanersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _cleanerRepositoryMock.Verify(
                r => r.GetAllCleanersAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<CleanerDto>>(cleaners),
                Times.Once);
        }


        // GetCleanerAsync


        [Fact]
        public async Task GetCleaner_ExistingId_ReturnsCleanerDto()
        {
            // Arrange
            var cleanerId = 1;

            var cleaner = new Cleaner(
                "John",
                "Doe",
                "john",
                "john@example.com",
                "01012345678",
                100,
                8,
                new TimeOnly(8, 0),
                new TimeOnly(16, 0));

            var cleanerDto = new CleanerDto();

            _cleanerRepositoryMock
                .Setup(r => r.GetCleanerAsync(cleanerId))
                .ReturnsAsync(cleaner);

            _mapperMock
                .Setup(m => m.Map<CleanerDto>(cleaner))
                .Returns(cleanerDto);

            // Act
            var result = await _cleanerService.GetCleanerAsync(cleanerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cleanerDto, result);

            _cleanerRepositoryMock.Verify(
                r => r.GetCleanerAsync(cleanerId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(cleaner),
                Times.Once);
        }

        [Fact]
        public async Task GetCleaner_NonExistingId_ThrowsException()
        {
            // Arrange
            var cleanerId = 999;

            _cleanerRepositoryMock
                .Setup(r => r.GetCleanerAsync(cleanerId))
                .ReturnsAsync((Cleaner?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _cleanerService.GetCleanerAsync(cleanerId));

            Assert.Equal(
                "Cleaner not found",
                exception.Message);

            _cleanerRepositoryMock.Verify(
                r => r.GetCleanerAsync(cleanerId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(
                    It.IsAny<Cleaner>()),
                Times.Never);
        }


        // UpdateCleanerAsync


        [Fact]
        public async Task UpdateCleaner_ExistingId_ReturnsUpdatedCleanerDto()
        {
            // Arrange
            var cleanerId = 1;

            var updateCleanerDto = new UpdateCleanerDto
            {
                FirstName = "John Updated",
                LastName = "Doe Updated",
                UserName = "john.updated",
                Email = "john.updated@example.com",
                PhoneNumber = "01011111111",
                SalaryPerHour = 150,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(18, 0)
            };

            var cleaner = new Cleaner(
                "John",
                "Doe",
                "john",
                "john@example.com",
                "01012345678",
                100,
                8,
                new TimeOnly(8, 0),
                new TimeOnly(16, 0));

            var updatedCleaner = new Cleaner(
                updateCleanerDto.FirstName,
                updateCleanerDto.LastName,
                updateCleanerDto.UserName,
                updateCleanerDto.Email,
                updateCleanerDto.PhoneNumber,
                updateCleanerDto.SalaryPerHour,
                updateCleanerDto.HoursWorked,
                updateCleanerDto.ShiftStart,
                updateCleanerDto.ShiftEnd);

            var cleanerDto = new CleanerDto();

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updateCleanerDto.Email,
                    updateCleanerDto.UserName,
                    updateCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.GetCleanerAsync(cleanerId))
                .ReturnsAsync(cleaner);

            _cleanerRepositoryMock
                .Setup(r => r.UpdateCleanerAsync(cleaner))
                .ReturnsAsync(updatedCleaner);

            _mapperMock
                .Setup(m => m.Map<CleanerDto>(updatedCleaner))
                .Returns(cleanerDto);

            // Act
            var result = await _cleanerService.UpdateCleanerAsync(
                updateCleanerDto,
                cleanerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cleanerDto, result);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updateCleanerDto.Email,
                    updateCleanerDto.UserName,
                    updateCleanerDto.PhoneNumber),
                Times.Once);

            _cleanerRepositoryMock.Verify(
                r => r.GetCleanerAsync(cleanerId),
                Times.Once);

            _cleanerRepositoryMock.Verify(
                r => r.UpdateCleanerAsync(cleaner),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(updatedCleaner),
                Times.Once);
        }

        [Fact]
        public async Task UpdateCleaner_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cleanerId = 999;

            var updateCleanerDto = new UpdateCleanerDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                SalaryPerHour = 100,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0)
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updateCleanerDto.Email,
                    updateCleanerDto.UserName,
                    updateCleanerDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _cleanerRepositoryMock
                .Setup(r => r.GetCleanerAsync(cleanerId))
                .ReturnsAsync((Cleaner?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _cleanerService.UpdateCleanerAsync(
                    updateCleanerDto,
                    cleanerId));

            Assert.Equal(
                $"Cannot find cleaner with id {cleanerId}",
                exception.Message);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updateCleanerDto.Email,
                    updateCleanerDto.UserName,
                    updateCleanerDto.PhoneNumber),
                Times.Once);

            _cleanerRepositoryMock.Verify(
                r => r.GetCleanerAsync(cleanerId),
                Times.Once);

            _cleanerRepositoryMock.Verify(
                r => r.UpdateCleanerAsync(It.IsAny<Cleaner>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<CleanerDto>(
                    It.IsAny<Cleaner>()),
                Times.Never);
        }
    }
}
