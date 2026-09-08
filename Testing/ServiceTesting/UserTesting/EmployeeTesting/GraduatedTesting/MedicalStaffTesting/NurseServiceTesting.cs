using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.MedicalStaffServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices.MedicalStaffServices
{
    public class NurseServiceTesting
    {
        private readonly Mock<INurseRepository> _nurseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<NurseService>> _loggerMock;

        private readonly NurseService _nurseService;

        public NurseServiceTesting()
        {
            _nurseRepositoryMock = new Mock<INurseRepository>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<NurseService>>();

            _nurseService = new NurseService(
                _nurseRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateNurse_HappyPath_ReturnsNurseDto()
        {
            // Arrange
            var createNurseDto = new CreateNurseDto
            {
                FirstName = "Sarah",
                LastName = "Ahmed",
                UserName = "sarahnurse",
                Email = "sarah@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Nursing",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2021,
                License = "NUR123"
            };

            var nurse = new Nurse(
                createNurseDto.FirstName,
                createNurseDto.LastName,
                createNurseDto.UserName,
                createNurseDto.Email,
                createNurseDto.PhoneNumber,
                createNurseDto.SalaryPerHour,
                createNurseDto.HoursWorked,
                createNurseDto.ShiftStart,
                createNurseDto.ShiftEnd,
                createNurseDto.Degree,
                createNurseDto.University,
                createNurseDto.YearsOfExperience,
                createNurseDto.GraduationYear,
                createNurseDto.License);

            var nurseDto = new NurseDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _nurseRepositoryMock
                .Setup(x => x.CreateNurseAsync(
                    It.IsAny<Nurse>(),
                    createNurseDto.Password))
                .ReturnsAsync((nurse, true, true, true));

            _mapperMock
                .Setup(x => x.Map<NurseDto>(nurse))
                .Returns(nurseDto);

            // Act
            var result = await _nurseService.CreateNurseAsync(createNurseDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nurseDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.CreateNurseAsync(
                    It.IsAny<Nurse>(),
                    createNurseDto.Password),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(nurse),
                Times.Once);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateNurse_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createNurseDto = new CreateNurseDto
            {
                FirstName = "Sarah",
                LastName = "Ahmed",
                UserName = "sarahnurse",
                Email = "sarah@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Nursing",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2021,
                License = "NUR123"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber))
                .ThrowsAsync(
                    new ArgumentException("Email already exists."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _nurseService.CreateNurseAsync(createNurseDto));

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.CreateNurseAsync(
                    It.IsAny<Nurse>(),
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(It.IsAny<Nurse>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateNurse_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createNurseDto = new CreateNurseDto
            {
                FirstName = "Sarah",
                LastName = "Ahmed",
                UserName = "sarahnurse",
                Email = "sarah@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Nursing",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2021,
                License = "NUR123"
            };

            var nurse = new Nurse(
                createNurseDto.FirstName,
                createNurseDto.LastName,
                createNurseDto.UserName,
                createNurseDto.Email,
                createNurseDto.PhoneNumber,
                createNurseDto.SalaryPerHour,
                createNurseDto.HoursWorked,
                createNurseDto.ShiftStart,
                createNurseDto.ShiftEnd,
                createNurseDto.Degree,
                createNurseDto.University,
                createNurseDto.YearsOfExperience,
                createNurseDto.GraduationYear,
                createNurseDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _nurseRepositoryMock
                .Setup(x => x.CreateNurseAsync(
                    It.IsAny<Nurse>(),
                    createNurseDto.Password))
                .ReturnsAsync((nurse, true, false, true));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(nurse.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _nurseService.CreateNurseAsync(createNurseDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(nurse.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(It.IsAny<Nurse>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateNurse_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createNurseDto = new CreateNurseDto
            {
                FirstName = "Sarah",
                LastName = "Ahmed",
                UserName = "sarahnurse",
                Email = "sarah@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 150m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "Nursing",
                University = "Cairo University",
                YearsOfExperience = 4,
                GraduationYear = 2021,
                License = "NUR123"
            };

            var nurse = new Nurse(
                createNurseDto.FirstName,
                createNurseDto.LastName,
                createNurseDto.UserName,
                createNurseDto.Email,
                createNurseDto.PhoneNumber,
                createNurseDto.SalaryPerHour,
                createNurseDto.HoursWorked,
                createNurseDto.ShiftStart,
                createNurseDto.ShiftEnd,
                createNurseDto.Degree,
                createNurseDto.University,
                createNurseDto.YearsOfExperience,
                createNurseDto.GraduationYear,
                createNurseDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createNurseDto.Email,
                    createNurseDto.UserName,
                    createNurseDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _nurseRepositoryMock
                .Setup(x => x.CreateNurseAsync(
                    It.IsAny<Nurse>(),
                    createNurseDto.Password))
                .ReturnsAsync((nurse, true, true, false));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(nurse.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _nurseService.CreateNurseAsync(createNurseDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(nurse.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(It.IsAny<Nurse>()),
                Times.Never);
        }

        [Fact]
        public async Task GetNurse_ExistingId_ReturnsNurseDto()
        {
            // Arrange
            var nurseId = 1;

            var nurse = new Mock<Nurse>().Object;
            var nurseDto = new NurseDto();

            _nurseRepositoryMock
                .Setup(x => x.GetNurseAsync(nurseId))
                .ReturnsAsync(nurse);

            _mapperMock
                .Setup(x => x.Map<NurseDto>(nurse))
                .Returns(nurseDto);

            // Act
            var result = await _nurseService.GetNurseAsync(nurseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nurseDto, result);

            _nurseRepositoryMock.Verify(
                x => x.GetNurseAsync(nurseId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(nurse),
                Times.Once);
        }

        [Fact]
        public async Task GetNurse_NonExistingId_ThrowsException()
        {
            // Arrange
            var nurseId = 999;

            _nurseRepositoryMock
                .Setup(x => x.GetNurseAsync(nurseId))
                .ReturnsAsync((Nurse?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _nurseService.GetNurseAsync(nurseId));

            Assert.Equal("Nurse not found", exception.Message);

            _nurseRepositoryMock.Verify(
                x => x.GetNurseAsync(nurseId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(It.IsAny<Nurse>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllNurses_HappyPath_ReturnsListOfNurseDto()
        {
            // Arrange
            var nurses = new List<Nurse>
            {
                new Mock<Nurse>().Object,
                new Mock<Nurse>().Object
            };

            var nurseDtos = new List<NurseDto>
            {
                new NurseDto(),
                new NurseDto()
            };

            _nurseRepositoryMock
                .Setup(x => x.GetAllNursesAsync())
                .ReturnsAsync(nurses);

            _mapperMock
                .Setup(x => x.Map<List<NurseDto>>(nurses))
                .Returns(nurseDtos);

            // Act
            var result = await _nurseService.GetAllNursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nurseDtos, result);

            _nurseRepositoryMock.Verify(
                x => x.GetAllNursesAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<NurseDto>>(nurses),
                Times.Once);
        }

        [Fact]
        public async Task GetAllNurses_NoNurses_ReturnsEmptyList()
        {
            // Arrange
            var nurses = new List<Nurse>();
            var nurseDtos = new List<NurseDto>();

            _nurseRepositoryMock
                .Setup(x => x.GetAllNursesAsync())
                .ReturnsAsync(nurses);

            _mapperMock
                .Setup(x => x.Map<List<NurseDto>>(nurses))
                .Returns(nurseDtos);

            // Act
            var result = await _nurseService.GetAllNursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _nurseRepositoryMock.Verify(
                x => x.GetAllNursesAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<NurseDto>>(nurses),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNurse_ExistingId_ReturnsUpdatedNurseDto()
        {
            // Arrange
            var nurseId = 1;

            var updateNurseDto = new UpdateNurseDto
            {
                FirstName = "Updated",
                LastName = "Nurse",
                UserName = "updatednurse",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 180m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Advanced Nursing",
                University = "Ain Shams University",
                YearsOfExperience = 6,
                GraduationYear = 2019,
                License = "NUR999"
            };

            var nurse = new Mock<Nurse>().Object;
            var updatedNurse = new Mock<Nurse>().Object;
            var nurseDto = new NurseDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateNurseDto.Email,
                    updateNurseDto.UserName,
                    updateNurseDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _nurseRepositoryMock
                .Setup(x => x.GetNurseAsync(nurseId))
                .ReturnsAsync(nurse);

            _nurseRepositoryMock
                .Setup(x => x.UpdateNurseAsync(nurse))
                .ReturnsAsync(updatedNurse);

            _mapperMock
                .Setup(x => x.Map<NurseDto>(updatedNurse))
                .Returns(nurseDto);

            // Act
            var result = await _nurseService.UpdateNurseAsync(
                updateNurseDto,
                nurseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nurseDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateNurseDto.Email,
                    updateNurseDto.UserName,
                    updateNurseDto.PhoneNumber),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.GetNurseAsync(nurseId),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.UpdateNurseAsync(nurse),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(updatedNurse),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNurse_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nurseId = 999;

            var updateNurseDto = new UpdateNurseDto
            {
                FirstName = "Updated",
                LastName = "Nurse",
                UserName = "updatednurse",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 180m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "Advanced Nursing",
                University = "Ain Shams University",
                YearsOfExperience = 6,
                GraduationYear = 2019,
                License = "NUR999"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateNurseDto.Email,
                    updateNurseDto.UserName,
                    updateNurseDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _nurseRepositoryMock
                .Setup(x => x.GetNurseAsync(nurseId))
                .ReturnsAsync((Nurse?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _nurseService.UpdateNurseAsync(
                    updateNurseDto,
                    nurseId));

            Assert.Equal(
                $"Cannot find nurse with id {nurseId}",
                exception.Message);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateNurseDto.Email,
                    updateNurseDto.UserName,
                    updateNurseDto.PhoneNumber),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.GetNurseAsync(nurseId),
                Times.Once);

            _nurseRepositoryMock.Verify(
                x => x.UpdateNurseAsync(It.IsAny<Nurse>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<NurseDto>(It.IsAny<Nurse>()),
                Times.Never);
        }
    }
}