using AutoMapper;
using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class MedicalRecordServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMedicalRecordRepository> _medicalRecordRepositoryMock;
        private readonly Mock<ILogger<MedicalRecordService>> _loggerMock;

        private readonly MedicalRecordService _medicalRecordService;

        public MedicalRecordServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _medicalRecordRepositoryMock = new Mock<IMedicalRecordRepository>();
            _loggerMock = new Mock<ILogger<MedicalRecordService>>();

            _medicalRecordService = new MedicalRecordService(
                _mapperMock.Object,
                _medicalRecordRepositoryMock.Object,
                _loggerMock.Object);
        }

        // Create Medical Record

        [Fact]
        public async Task CreateMedicalRecord_HappyPath_ReturnsCreatedMedicalRecord()
        {
            // Arrange

            var createMedicalRecordDto = new CreateMedicalRecordDto
            {
                Height = 180,
                Weight = 75,
                BloodType = "A+",
                PatientId = 1
            };

            var medicalRecord = new MedicalRecord(
                createMedicalRecordDto.Height,
                createMedicalRecordDto.Weight,
                createMedicalRecordDto.BloodType,
                createMedicalRecordDto.PatientId);

            var expectedDto = new MedicalRecordDto
            {
                Id = 1,
                Height = createMedicalRecordDto.Height,
                Weight = createMedicalRecordDto.Weight,
                BloodType = createMedicalRecordDto.BloodType,
                PatientId = createMedicalRecordDto.PatientId
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.CreateMedicalRecordAsync(
                    It.IsAny<MedicalRecord>()))
                .ReturnsAsync(medicalRecord);

            _mapperMock
                .Setup(m => m.Map<MedicalRecordDto>(
                    It.IsAny<MedicalRecord>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _medicalRecordService.CreateMedicalRecordAsync(
                    createMedicalRecordDto);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.CreateMedicalRecordAsync(
                        It.IsAny<MedicalRecord>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        It.IsAny<MedicalRecord>()),
                    Times.Once);
        }

        // Get All Medical Records

        [Fact]
        public async Task GetAllMedicalRecords_HappyPath_ReturnsListOfMedicalRecordDto()
        {
            // Arrange

            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord(
                    180,
                    75,
                    "A+",
                    1),

                new MedicalRecord(
                    170,
                    65,
                    "B+",
                    2)
            };

            var expectedDtos = new List<MedicalRecordDto>
            {
                new MedicalRecordDto
                {
                    Id = 1,
                    Height = medicalRecords[0].Height,
                    Weight = medicalRecords[0].Weight,
                    BloodType = medicalRecords[0].BloodType,
                    PatientId = medicalRecords[0].PatientId
                },

                new MedicalRecordDto
                {
                    Id = 2,
                    Height = medicalRecords[1].Height,
                    Weight = medicalRecords[1].Weight,
                    BloodType = medicalRecords[1].BloodType,
                    PatientId = medicalRecords[1].PatientId
                }
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.GetAllMedicalRecordsAsync())
                .ReturnsAsync(medicalRecords);

            _mapperMock
                .Setup(m => m.Map<List<MedicalRecordDto>>(
                    medicalRecords))
                .Returns(expectedDtos);

            // Act

            var result =
                await _medicalRecordService.GetAllMedicalRecordsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetAllMedicalRecordsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<MedicalRecordDto>>(
                        medicalRecords),
                    Times.Once);
        }

        [Fact]
        public async Task GetAllMedicalRecords_NoRecords_ReturnsEmptyList()
        {
            // Arrange

            var medicalRecords = new List<MedicalRecord>();
            var expectedDtos = new List<MedicalRecordDto>();

            _medicalRecordRepositoryMock
                .Setup(r => r.GetAllMedicalRecordsAsync())
                .ReturnsAsync(medicalRecords);

            _mapperMock
                .Setup(m => m.Map<List<MedicalRecordDto>>(
                    medicalRecords))
                .Returns(expectedDtos);

            // Act

            var result =
                await _medicalRecordService.GetAllMedicalRecordsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetAllMedicalRecordsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<MedicalRecordDto>>(
                    medicalRecords),
                    Times.Once);
        }

        // Get Medical Record By ID

        [Fact]
        public async Task GetMedicalRecord_ExistingId_ReturnsMedicalRecordDto()
        {
            // Arrange

            var id = 1;

            var medicalRecord = new MedicalRecord(
                180,
                75,
                "A+",
                1);

            var expectedDto = new MedicalRecordDto
            {
                Id = id,
                Height = medicalRecord.Height,
                Weight = medicalRecord.Weight,
                BloodType = medicalRecord.BloodType,
                PatientId = medicalRecord.PatientId
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordAsync(id))
                .ReturnsAsync(medicalRecord);

            _mapperMock
                .Setup(m => m.Map<MedicalRecordDto>(
                    medicalRecord))
                .Returns(expectedDto);

            // Act

            var result =
                await _medicalRecordService.GetMedicalRecordAsync(id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        medicalRecord),
                    Times.Once);
        }

        [Fact]
        public async Task GetMedicalRecord_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordAsync(id))
                .ReturnsAsync((MedicalRecord?)null);

            // Act & Assert

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _medicalRecordService.GetMedicalRecordAsync(id));

            Assert.Equal(
                $"Medical record with ID {id} not found.",
                exception.Message);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        It.IsAny<MedicalRecord>()),
                    Times.Never);
        }

        // Get Medical Record By Patient ID

        [Fact]
        public async Task GetMedicalByPatientIdRecord_ExistingPatientId_ReturnsMedicalRecordDto()
        {
            // Arrange

            var patientId = 1;

            var medicalRecord = new MedicalRecord(
                180,
                75,
                "A+",
                patientId);

            var expectedDto = new MedicalRecordDto
            {
                Id = 1,
                Height = medicalRecord.Height,
                Weight = medicalRecord.Weight,
                BloodType = medicalRecord.BloodType,
                PatientId = medicalRecord.PatientId
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordByPatientIdAsync(patientId))
                .ReturnsAsync(medicalRecord);

            _mapperMock
                .Setup(m => m.Map<MedicalRecordDto>(
                    medicalRecord))
                .Returns(expectedDto);

            // Act

            var result =
                await _medicalRecordService.GetMedicalByPatientIdRecord(
                    patientId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordByPatientIdAsync(patientId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        medicalRecord),
                    Times.Once);
        }

        [Fact]
        public async Task GetMedicalByPatientIdRecord_NonExistingPatientId_ThrowsException()
        {
            // Arrange

            var patientId = 999;

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordByPatientIdAsync(patientId))
                .ReturnsAsync((MedicalRecord?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _medicalRecordService.GetMedicalByPatientIdRecord(
                        patientId));

            Assert.Equal(
                $"Medical record with Patient ID {patientId} not found.",
                exception.Message);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordByPatientIdAsync(patientId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        It.IsAny<MedicalRecord>()),
                    Times.Never);
        }

        // Update Medical Record

        [Fact]
        public async Task UpdateMedicalRecord_ExistingId_ReturnsUpdatedMedicalRecordDto()
        {
            // Arrange

            var id = 1;

            var medicalRecord = new MedicalRecord(
                170,
                70,
                "B+",
                1);

            var updateMedicalRecordDto = new UpdateMedicalRecordDto
            {
                Height = 180,
                Weight = 75,
                BloodType = "A+",
                PatientId = 2
            };

            var expectedDto = new MedicalRecordDto
            {
                Id = id,
                Height = updateMedicalRecordDto.Height,
                Weight = updateMedicalRecordDto.Weight,
                BloodType = updateMedicalRecordDto.BloodType,
                PatientId = updateMedicalRecordDto.PatientId
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordAsync(id))
                .ReturnsAsync(medicalRecord);

            _mapperMock
                .Setup(m => m.Map<MedicalRecordDto>(
                    medicalRecord))
                .Returns(expectedDto);

            // Act

            var result =
                await _medicalRecordService.UpdateMedicalRecordAsync(
                    updateMedicalRecordDto,
                    id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        medicalRecord),
                    Times.Once);
        }

        [Fact]
        public async Task UpdateMedicalRecord_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            var updateMedicalRecordDto = new UpdateMedicalRecordDto
            {
                Height = 180,
                Weight = 75,
                BloodType = "A+",
                PatientId = 1
            };

            _medicalRecordRepositoryMock
                .Setup(r => r.GetMedicalRecordAsync(id))
                .ReturnsAsync((MedicalRecord?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _medicalRecordService.UpdateMedicalRecordAsync(
                        updateMedicalRecordDto,
                        id));

            Assert.Equal(
                $"Medical record with ID {id} not found.",
                exception.Message);

            _medicalRecordRepositoryMock
                .Verify(
                    r => r.GetMedicalRecordAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<MedicalRecordDto>(
                        It.IsAny<MedicalRecord>()),
                    Times.Never);
        }
    }
}
