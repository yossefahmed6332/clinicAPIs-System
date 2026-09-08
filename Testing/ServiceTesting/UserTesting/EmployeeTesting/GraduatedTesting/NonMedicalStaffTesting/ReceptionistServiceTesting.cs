using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ReceptionistDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.NonMedicalStaffServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices.NonMedicalStaffServices
{
    public class ReceptionistServiceTesting
    {
        private readonly Mock<IReceptionistRepository> _receptionistRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ReceptionistService>> _loggerMock;
        private readonly ReceptionistService _receptionistService;

        public ReceptionistServiceTesting()
        {
            _receptionistRepositoryMock = new Mock<IReceptionistRepository>();
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ReceptionistService>>();

            _receptionistService = new ReceptionistService(
                _receptionistRepositoryMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateReceptionist_HappyPath_ReturnsReceptionistDto()
        {
            // Arrange
            var createDto = new CreateReceptionistDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 3,
                GraduationYear = 2023,
                License = "REC-123",
                Password = "Password123!"
            };

            var receptionist = new Mock<Receptionist>().Object;

            var receptionistDto = new ReceptionistDto();

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _receptionistRepositoryMock
                .Setup(r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password))
                .ReturnsAsync((
                    receptionist: receptionist,
                    addUserRes: true,
                    addPasswordRes: true,
                    addRoleRes: true));

            _mapperMock
                .Setup(m => m.Map<ReceptionistDto>(receptionist))
                .Returns(receptionistDto);

            // Act
            var result = await _receptionistService.CreateReceptionistAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receptionistDto, result);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(receptionist),
                Times.Once);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateReceptionist_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createDto = new CreateReceptionistDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 3,
                GraduationYear = 2023,
                License = "REC-123",
                Password = "Password123!"
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber))
                .ThrowsAsync(new ArgumentException("Email already exists."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _receptionistService.CreateReceptionistAsync(createDto));

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(
                    It.IsAny<Receptionist>()),
                Times.Never);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateReceptionist_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createDto = new CreateReceptionistDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 3,
                GraduationYear = 2023,
                License = "REC-123",
                Password = "Password123!"
            };

            var receptionist = new Mock<Receptionist>().Object;

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _receptionistRepositoryMock
                .Setup(r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password))
                .ReturnsAsync((
                    receptionist: receptionist,
                    addUserRes: true,
                    addPasswordRes: false,
                    addRoleRes: true));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(receptionist.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _receptionistService.CreateReceptionistAsync(createDto));

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password),
                Times.Once);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(receptionist.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(
                    It.IsAny<Receptionist>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateReceptionist_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createDto = new CreateReceptionistDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                SalaryPerHour = 250m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 3,
                GraduationYear = 2023,
                License = "REC-123",
                Password = "Password123!"
            };

            var receptionist = new Mock<Receptionist>().Object;

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _receptionistRepositoryMock
                .Setup(r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password))
                .ReturnsAsync((
                    receptionist: receptionist,
                    addUserRes: true,
                    addPasswordRes: true,
                    addRoleRes: false));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(receptionist.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _receptionistService.CreateReceptionistAsync(createDto));

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createDto.Email,
                    createDto.UserName,
                    createDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.CreateReceptionistAsync(
                    It.IsAny<Receptionist>(),
                    createDto.Password),
                Times.Once);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(receptionist.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(
                    It.IsAny<Receptionist>()),
                Times.Never);
        }

        [Fact]
        public async Task GetReceptionist_ExistingId_ReturnsReceptionistDto()
        {
            // Arrange
            var receptionistId = 1;

            var receptionist = new Mock<Receptionist>().Object;
            var receptionistDto = new ReceptionistDto();

            _receptionistRepositoryMock
                .Setup(r => r.GetReceptionistAsync(receptionistId))
                .ReturnsAsync(receptionist);

            _mapperMock
                .Setup(m => m.Map<ReceptionistDto>(receptionist))
                .Returns(receptionistDto);

            // Act
            var result =
                await _receptionistService.GetReceptionistAsync(receptionistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receptionistDto, result);

            _receptionistRepositoryMock.Verify(
                r => r.GetReceptionistAsync(receptionistId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(receptionist),
                Times.Once);
        }

        [Fact]
        public async Task GetReceptionist_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var receptionistId = 999;

            _receptionistRepositoryMock
                .Setup(r => r.GetReceptionistAsync(receptionistId))
                .ReturnsAsync((Receptionist?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _receptionistService.GetReceptionistAsync(receptionistId));

            Assert.Equal(
                $"Cannot find receptionist with id {receptionistId}",
                exception.Message);

            _receptionistRepositoryMock.Verify(
                r => r.GetReceptionistAsync(receptionistId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(
                    It.IsAny<Receptionist>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllReceptionists_HappyPath_ReturnsListOfReceptionistDto()
        {
            // Arrange
            var receptionists = new List<Receptionist>
            {
                new Mock<Receptionist>().Object,
                new Mock<Receptionist>().Object
            };

            var receptionistDtos = new List<ReceptionistDto>
            {
                new ReceptionistDto(),
                new ReceptionistDto()
            };

            _receptionistRepositoryMock
                .Setup(r => r.GetAllReceptionistsAsync())
                .ReturnsAsync(receptionists);

            _mapperMock
                .Setup(m => m.Map<List<ReceptionistDto>>(receptionists))
                .Returns(receptionistDtos);

            // Act
            var result =
                await _receptionistService.GetAllReceptionistsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receptionistDtos, result);
            Assert.Equal(2, result.Count);

            _receptionistRepositoryMock.Verify(
                r => r.GetAllReceptionistsAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<ReceptionistDto>>(receptionists),
                Times.Once);
        }

        [Fact]
        public async Task GetAllReceptionists_NoReceptionists_ReturnsEmptyList()
        {
            // Arrange
            var receptionists = new List<Receptionist>();
            var receptionistDtos = new List<ReceptionistDto>();

            _receptionistRepositoryMock
                .Setup(r => r.GetAllReceptionistsAsync())
                .ReturnsAsync(receptionists);

            _mapperMock
                .Setup(m => m.Map<List<ReceptionistDto>>(receptionists))
                .Returns(receptionistDtos);

            // Act
            var result =
                await _receptionistService.GetAllReceptionistsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _receptionistRepositoryMock.Verify(
                r => r.GetAllReceptionistsAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<ReceptionistDto>>(receptionists),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReceptionist_ExistingId_ReturnsUpdatedReceptionistDto()
        {
            // Arrange
            var receptionistId = 1;

            var updateDto = new UpdateReceptionistDto
            {
                FirstName = "John",
                LastName = "Updated",
                UserName = "john.updated",
                Email = "john.updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 300m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2022,
                License = "REC-456"
            };

            var receptionist = new Mock<Receptionist>().Object;
            var updatedReceptionist = new Mock<Receptionist>().Object;
            var receptionistDto = new ReceptionistDto();

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updateDto.Email,
                    updateDto.UserName,
                    updateDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _receptionistRepositoryMock
                .Setup(r => r.GetReceptionistAsync(receptionistId))
                .ReturnsAsync(receptionist);

            _receptionistRepositoryMock
                .Setup(r => r.UpdateReceptionistAsync(receptionist))
                .ReturnsAsync(updatedReceptionist);

            _mapperMock
                .Setup(m => m.Map<ReceptionistDto>(updatedReceptionist))
                .Returns(receptionistDto);

            // Act
            var result =
                await _receptionistService.UpdateReceptionistAsync(
                    updateDto,
                    receptionistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receptionistDto, result);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updateDto.Email,
                    updateDto.UserName,
                    updateDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.GetReceptionistAsync(receptionistId),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.UpdateReceptionistAsync(receptionist),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(updatedReceptionist),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReceptionist_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var receptionistId = 999;

            var updateDto = new UpdateReceptionistDto
            {
                FirstName = "John",
                LastName = "Updated",
                UserName = "john.updated",
                Email = "john.updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 300m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Bachelor",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2022,
                License = "REC-456"
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updateDto.Email,
                    updateDto.UserName,
                    updateDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _receptionistRepositoryMock
                .Setup(r => r.GetReceptionistAsync(receptionistId))
                .ReturnsAsync((Receptionist?)null);

            // Act & Assert
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _receptionistService.UpdateReceptionistAsync(
                        updateDto,
                        receptionistId));

            Assert.Equal(
                $"Cannot find receptionist with id {receptionistId}",
                exception.Message);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updateDto.Email,
                    updateDto.UserName,
                    updateDto.PhoneNumber),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.GetReceptionistAsync(receptionistId),
                Times.Once);

            _receptionistRepositoryMock.Verify(
                r => r.UpdateReceptionistAsync(
                    It.IsAny<Receptionist>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<ReceptionistDto>(
                    It.IsAny<Receptionist>()),
                Times.Never);
        }
    }
}