using AutoMapper;
using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Services.UserServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClinicAPIsTestProject.Services.UserServices
{
    public class PatientServiceTesting
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMedicalRecordService> _medicalRecordServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<PatientService>> _loggerMock;

        private readonly PatientService _patientService;

        public PatientServiceTesting()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _userServiceMock = new Mock<IUserService>();
            _medicalRecordServiceMock = new Mock<IMedicalRecordService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<PatientService>>();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _userServiceMock.Object,
                _medicalRecordServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreatePatient_HappyPath_ReturnsPatientDto()
        {
            // Arrange
            var createPatientDto = new CreatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                createMedicalRecordDto = new CreateMedicalRecordDto
                {
                    Height = 180,
                    Weight = 80,
                    BloodType = "A+",
                    PatientId = 0
                }
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = 1
            };

            var patient = new Patient(
                medicalRecordDto.Id,
                createPatientDto.FirstName,
                createPatientDto.LastName,
                createPatientDto.UserName,
                createPatientDto.Email,
                createPatientDto.PhoneNumber);

            var patientDto = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                UserName = patient.UserName!,
                Email = patient.Email!,
                PhoneNumber = patient.PhoneNumber!
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createPatientDto.Email,
                    createPatientDto.UserName,
                    createPatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _medicalRecordServiceMock
                .Setup(m => m.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto))
                .ReturnsAsync(medicalRecordDto);

            _patientRepositoryMock
                .Setup(r => r.CreatePatientAsync(
                    It.IsAny<Patient>(),
                    createPatientDto.Password))
                .ReturnsAsync((patient, true, true, true));

            _mapperMock
                .Setup(m => m.Map<PatientDto>(patient))
                .Returns(patientDto);

            // Act
            var result = await _patientService.CreatePatientAsync(createPatientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientDto.Id, result.Id);
            Assert.Equal(patientDto.Email, result.Email);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    createPatientDto.Email,
                    createPatientDto.UserName,
                    createPatientDto.PhoneNumber),
                Times.Once);

            _medicalRecordServiceMock.Verify(
                m => m.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto),
                Times.Once);

            _patientRepositoryMock.Verify(
                r => r.CreatePatientAsync(
                    It.IsAny<Patient>(),
                    createPatientDto.Password),
                Times.Once);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task CreatePatient_UserCreationFails_ThrowsException()
        {
            // Arrange
            var createPatientDto = new CreatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                createMedicalRecordDto = new CreateMedicalRecordDto
                {
                    Height = 180,
                    Weight = 80,
                    BloodType = "A+",
                    PatientId = 0
                }
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = 1
            };

            var patient = new Patient(
                medicalRecordDto.Id,
                createPatientDto.FirstName,
                createPatientDto.LastName,
                createPatientDto.UserName,
                createPatientDto.Email,
                createPatientDto.PhoneNumber);

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createPatientDto.Email,
                    createPatientDto.UserName,
                    createPatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _medicalRecordServiceMock
                .Setup(m => m.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto))
                .ReturnsAsync(medicalRecordDto);

            _patientRepositoryMock
                .Setup(r => r.CreatePatientAsync(
                    It.IsAny<Patient>(),
                    createPatientDto.Password))
                .ReturnsAsync((patient, false, false, false));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _patientService.CreatePatientAsync(createPatientDto));

            Assert.Equal("Cannot create user, try again", exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(It.IsAny<int>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task CreatePatient_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createPatientDto = new CreatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                createMedicalRecordDto = new CreateMedicalRecordDto
                {
                    Height = 180,
                    Weight = 80,
                    BloodType = "A+",
                    PatientId = 0
                }
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = 1
            };

            var patient = new Patient(
                medicalRecordDto.Id,
                createPatientDto.FirstName,
                createPatientDto.LastName,
                createPatientDto.UserName,
                createPatientDto.Email,
                createPatientDto.PhoneNumber);

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createPatientDto.Email,
                    createPatientDto.UserName,
                    createPatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _medicalRecordServiceMock
                .Setup(m => m.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto))
                .ReturnsAsync(medicalRecordDto);

            _patientRepositoryMock
                .Setup(r => r.CreatePatientAsync(
                    It.IsAny<Patient>(),
                    createPatientDto.Password))
                .ReturnsAsync((patient, true, false, false));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(patient.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _patientService.CreatePatientAsync(createPatientDto));

            Assert.Equal("Cannot create user, try again", exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(patient.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task CreatePatient_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange
            var createPatientDto = new CreatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john.doe",
                Email = "john@example.com",
                PhoneNumber = "01012345678",
                Password = "Password123!",
                createMedicalRecordDto = new CreateMedicalRecordDto
                {
                    Height = 180,
                    Weight = 80,
                    BloodType = "A+",
                    PatientId = 0
                }
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = 1
            };

            var patient = new Patient(
                medicalRecordDto.Id,
                createPatientDto.FirstName,
                createPatientDto.LastName,
                createPatientDto.UserName,
                createPatientDto.Email,
                createPatientDto.PhoneNumber);

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    createPatientDto.Email,
                    createPatientDto.UserName,
                    createPatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _medicalRecordServiceMock
                .Setup(m => m.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto))
                .ReturnsAsync(medicalRecordDto);

            _patientRepositoryMock
                .Setup(r => r.CreatePatientAsync(
                    It.IsAny<Patient>(),
                    createPatientDto.Password))
                .ReturnsAsync((patient, true, true, false));

            _userServiceMock
                .Setup(u => u.DeleteUserAsync(patient.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _patientService.CreatePatientAsync(createPatientDto));

            Assert.Equal("Cannot create user, try again", exception.Message);

            _userServiceMock.Verify(
                u => u.DeleteUserAsync(patient.Id),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAllPatients_HappyPath_ReturnsListOfPatientDto()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient(1, "John", "Doe", "john", "john@example.com", "01012345678"),
                new Patient(2, "Jane", "Doe", "jane", "jane@example.com", "01098765432")
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto { Id = 1 },
                new PatientDto { Id = 2 }
            };

            _patientRepositoryMock
                .Setup(r => r.GetAllPatientsAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(m => m.Map<List<PatientDto>>(patients))
                .Returns(patientDtos);

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            _patientRepositoryMock.Verify(
                r => r.GetAllPatientsAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetAllPatients_NoPatients_ReturnsEmptyList()
        {
            // Arrange
            var patients = new List<Patient>();
            var patientDtos = new List<PatientDto>();

            _patientRepositoryMock
                .Setup(r => r.GetAllPatientsAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(m => m.Map<List<PatientDto>>(patients))
                .Returns(patientDtos);

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _patientRepositoryMock.Verify(
                r => r.GetAllPatientsAsync(),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<List<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetPatient_ExistingId_ReturnsPatientDto()
        {
            // Arrange
            var patientId = 1;

            var patient = new Patient(
                1,
                "John",
                "Doe",
                "john",
                "john@example.com",
                "01012345678");

            var patientDto = new PatientDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            };

            _patientRepositoryMock
                .Setup(r => r.GetPatientAsync(patientId))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(m => m.Map<PatientDto>(patient))
                .Returns(patientDto);

            // Act
            var result = await _patientService.GetPatientAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.Id);

            _patientRepositoryMock.Verify(
                r => r.GetPatientAsync(patientId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task GetPatient_NonExistingId_ThrowsException()
        {
            // Arrange
            var patientId = 999;

            _patientRepositoryMock
                .Setup(r => r.GetPatientAsync(patientId))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _patientService.GetPatientAsync(patientId));

            Assert.Equal(
                $"Cannot find user with ID{patientId}",
                exception.Message);

            _patientRepositoryMock.Verify(
                r => r.GetPatientAsync(patientId),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatient_ExistingId_ReturnsUpdatedPatientDto()
        {
            // Arrange
            var patientId = 1;

            var updatePatientDto = new UpdatePatientDto
            {
                FirstName = "John Updated",
                LastName = "Doe Updated",
                UserName = "john.updated",
                Email = "john.updated@example.com",
                PhoneNumber = "01011111111"
            };

            var patient = new Patient(
                1,
                "John",
                "Doe",
                "john",
                "john@example.com",
                "01012345678");

            var updatedPatient = new Patient(
                1,
                updatePatientDto.FirstName,
                updatePatientDto.LastName,
                updatePatientDto.UserName,
                updatePatientDto.Email,
                updatePatientDto.PhoneNumber);

            var patientDto = new PatientDto
            {
                Id = 1,
                FirstName = updatePatientDto.FirstName,
                LastName = updatePatientDto.LastName,
                UserName = updatePatientDto.UserName,
                Email = updatePatientDto.Email,
                PhoneNumber = updatePatientDto.PhoneNumber
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updatePatientDto.Email,
                    updatePatientDto.UserName,
                    updatePatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(r => r.GetPatientAsync(patientId))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(r => r.UpdatePatientAsync(patient))
                .ReturnsAsync(updatedPatient);

            _mapperMock
                .Setup(m => m.Map<PatientDto>(updatedPatient))
                .Returns(patientDto);

            // Act
            var result = await _patientService.UpdatePatientAsync(
                updatePatientDto,
                patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientDto.Id, result.Id);
            Assert.Equal(patientDto.Email, result.Email);
            Assert.Equal(patientDto.FirstName, result.FirstName);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updatePatientDto.Email,
                    updatePatientDto.UserName,
                    updatePatientDto.PhoneNumber),
                Times.Once);

            _patientRepositoryMock.Verify(
                r => r.GetPatientAsync(patientId),
                Times.Once);

            _patientRepositoryMock.Verify(
                r => r.UpdatePatientAsync(patient),
                Times.Once);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(updatedPatient),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatient_NonExistingId_ThrowsException()
        {
            // Arrange
            var patientId = 999;

            var updatePatientDto = new UpdatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "john",
                Email = "john@example.com",
                PhoneNumber = "01012345678"
            };

            _userServiceMock
                .Setup(u => u.ValidateUserCreation(
                    updatePatientDto.Email,
                    updatePatientDto.UserName,
                    updatePatientDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(r => r.GetPatientAsync(patientId))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _patientService.UpdatePatientAsync(
                    updatePatientDto,
                    patientId));

            Assert.Equal(
                $"Cannot find user with ID{patientId}",
                exception.Message);

            _userServiceMock.Verify(
                u => u.ValidateUserCreation(
                    updatePatientDto.Email,
                    updatePatientDto.UserName,
                    updatePatientDto.PhoneNumber),
                Times.Once);

            _patientRepositoryMock.Verify(
                r => r.GetPatientAsync(patientId),
                Times.Once);

            _patientRepositoryMock.Verify(
                r => r.UpdatePatientAsync(It.IsAny<Patient>()),
                Times.Never);

            _mapperMock.Verify(
                m => m.Map<PatientDto>(It.IsAny<Patient>()),
                Times.Never);
        }
    }
}