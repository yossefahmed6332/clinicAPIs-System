using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.MedicalStaffServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices.EmployeeServices.MedicalStaffServices
{
    public class DoctorServiceTesting
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;

        private readonly DoctorService _doctorService;

        public DoctorServiceTesting()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<DoctorService>>();

            _doctorService = new DoctorService(
                _doctorRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateDoctor_HappyPath_ReturnsDoctorDto()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDto
            {
                FirstName = "John",
                LastName = "Smith",
                UserName = "johnsmith",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 200m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "MD",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "DOC123"
            };

            var doctor = new Doctor(
                createDoctorDto.FirstName,
                createDoctorDto.LastName,
                createDoctorDto.UserName,
                createDoctorDto.Email,
                createDoctorDto.PhoneNumber,
                createDoctorDto.SalaryPerHour,
                createDoctorDto.HoursWorked,
                createDoctorDto.ShiftStart,
                createDoctorDto.ShiftEnd,
                createDoctorDto.Degree,
                createDoctorDto.University,
                createDoctorDto.YearsOfExperience,
                createDoctorDto.GraduationYear,
                createDoctorDto.License);

            var doctorDto = new DoctorDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.CreateDoctorAsync(
                    It.IsAny<Doctor>(),
                    createDoctorDto.Password))
                .ReturnsAsync((doctor, true, true, true));

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            // Act
            var result = await _doctorService.CreateDoctorAsync(createDoctorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.CreateDoctorAsync(
                    It.IsAny<Doctor>(),
                    createDoctorDto.Password),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(doctor),
                Times.Once);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateDoctor_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDto
            {
                FirstName = "John",
                LastName = "Smith",
                UserName = "johnsmith",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 200m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "MD",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "DOC123"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber))
                .ThrowsAsync(new ArgumentException("Email already exists."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _doctorService.CreateDoctorAsync(createDoctorDto));

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.CreateDoctorAsync(
                    It.IsAny<Doctor>(),
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateDoctor_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDto
            {
                FirstName = "John",
                LastName = "Smith",
                UserName = "johnsmith",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 200m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "MD",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "DOC123"
            };

            var doctor = new Doctor(
                createDoctorDto.FirstName,
                createDoctorDto.LastName,
                createDoctorDto.UserName,
                createDoctorDto.Email,
                createDoctorDto.PhoneNumber,
                createDoctorDto.SalaryPerHour,
                createDoctorDto.HoursWorked,
                createDoctorDto.ShiftStart,
                createDoctorDto.ShiftEnd,
                createDoctorDto.Degree,
                createDoctorDto.University,
                createDoctorDto.YearsOfExperience,
                createDoctorDto.GraduationYear,
                createDoctorDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.CreateDoctorAsync(
                    It.IsAny<Doctor>(),
                    createDoctorDto.Password))
                .ReturnsAsync((doctor, true, false, true));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(doctor.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _doctorService.CreateDoctorAsync(createDoctorDto));

            Assert.Equal("Cannot create user, try again", exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(doctor.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateDoctor_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDto
            {
                FirstName = "John",
                LastName = "Smith",
                UserName = "johnsmith",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                SalaryPerHour = 200m,
                HoursWorked = 8,
                ShiftStart = new TimeOnly(8, 0),
                ShiftEnd = new TimeOnly(16, 0),
                Degree = "MD",
                University = "Cairo University",
                YearsOfExperience = 5,
                GraduationYear = 2020,
                License = "DOC123"
            };

            var doctor = new Doctor(
                createDoctorDto.FirstName,
                createDoctorDto.LastName,
                createDoctorDto.UserName,
                createDoctorDto.Email,
                createDoctorDto.PhoneNumber,
                createDoctorDto.SalaryPerHour,
                createDoctorDto.HoursWorked,
                createDoctorDto.ShiftStart,
                createDoctorDto.ShiftEnd,
                createDoctorDto.Degree,
                createDoctorDto.University,
                createDoctorDto.YearsOfExperience,
                createDoctorDto.GraduationYear,
                createDoctorDto.License);

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createDoctorDto.Email,
                    createDoctorDto.UserName,
                    createDoctorDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.CreateDoctorAsync(
                    It.IsAny<Doctor>(),
                    createDoctorDto.Password))
                .ReturnsAsync((doctor, true, true, false));

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(doctor.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _doctorService.CreateDoctorAsync(createDoctorDto));

            Assert.Equal("Cannot create user, try again", exception.Message);

            _userServiceMock.Verify(
                x => x.DeleteUserAsync(doctor.Id),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctor_ExistingId_ReturnsDoctorDto()
        {
            // Arrange
            var doctorId = 1;

            var doctor = new Mock<Doctor>().Object;
            var doctorDto = new DoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetDoctorAsync(doctorId))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            // Act
            var result = await _doctorService.GetDoctorAsync(doctorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorDto, result);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorAsync(doctorId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(doctor),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctor_NonExistingId_ThrowsException()
        {
            // Arrange
            var doctorId = 999;

            _doctorRepositoryMock
                .Setup(x => x.GetDoctorAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _doctorService.GetDoctorAsync(doctorId));

            Assert.Equal(
                $"Cannot find doctor with id {doctorId}",
                exception.Message);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorAsync(doctorId),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllDoctors_HappyPath_ReturnsListOfDoctorDto()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Mock<Doctor>().Object,
                new Mock<Doctor>().Object
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto(),
                new DoctorDto()
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllDoctorsAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.GetAllDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorDtos, result);

            _doctorRepositoryMock.Verify(
                x => x.GetAllDoctorsAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<DoctorDto>>(doctors),
                Times.Once);
        }

        [Fact]
        public async Task GetAllDoctors_NoDoctors_ReturnsEmptyList()
        {
            // Arrange
            var doctors = new List<Doctor>();
            var doctorDtos = new List<DoctorDto>();

            _doctorRepositoryMock
                .Setup(x => x.GetAllDoctorsAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.GetAllDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _doctorRepositoryMock.Verify(
                x => x.GetAllDoctorsAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<DoctorDto>>(doctors),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctor_ExistingId_ReturnsUpdatedDoctorDto()
        {
            // Arrange
            var doctorId = 1;

            var updateDoctorDto = new UpdateDoctorDto
            {
                FirstName = "Updated",
                LastName = "Doctor",
                UserName = "updateddoctor",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 250m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "MD",
                University = "Ain Shams University",
                YearsOfExperience = 7,
                GraduationYear = 2018,
                License = "DOC999"
            };

            var doctor = new Mock<Doctor>().Object;
            var updatedDoctor = new Mock<Doctor>().Object;
            var doctorDto = new DoctorDto();

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateDoctorDto.Email,
                    updateDoctorDto.UserName,
                    updateDoctorDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.GetDoctorAsync(doctorId))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateDoctorAsync(doctor))
                .ReturnsAsync(updatedDoctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(updatedDoctor))
                .Returns(doctorDto);

            // Act
            var result = await _doctorService.UpdateDoctorAsync(
                updateDoctorDto,
                doctorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorDto, result);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateDoctorDto.Email,
                    updateDoctorDto.UserName,
                    updateDoctorDto.PhoneNumber),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorAsync(doctorId),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.UpdateDoctorAsync(doctor),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(updatedDoctor),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctor_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var doctorId = 999;

            var updateDoctorDto = new UpdateDoctorDto
            {
                FirstName = "Updated",
                LastName = "Doctor",
                UserName = "updateddoctor",
                Email = "updated@example.com",
                PhoneNumber = "01098765432",
                SalaryPerHour = 250m,
                HoursWorked = 9,
                ShiftStart = new TimeOnly(9, 0),
                ShiftEnd = new TimeOnly(17, 0),
                Degree = "MD",
                University = "Ain Shams University",
                YearsOfExperience = 7,
                GraduationYear = 2018,
                License = "DOC999"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateDoctorDto.Email,
                    updateDoctorDto.UserName,
                    updateDoctorDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.GetDoctorAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _doctorService.UpdateDoctorAsync(
                    updateDoctorDto,
                    doctorId));

            Assert.Equal(
                $"Cannot find doctor with id {doctorId}",
                exception.Message);

            _userServiceMock.Verify(
                x => x.ValidateUserCreation(
                    updateDoctorDto.Email,
                    updateDoctorDto.UserName,
                    updateDoctorDto.PhoneNumber),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorAsync(doctorId),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.UpdateDoctorAsync(It.IsAny<Doctor>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(It.IsAny<Doctor>()),
                Times.Never);
        }
    }
}