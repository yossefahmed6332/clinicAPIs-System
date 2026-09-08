using AutoMapper;
using clinicAPIsSystem.DTOs.VitalSignsDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class VitalSignsServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IVitalSignsRepository> _vitalSignsRepositoryMock;
        private readonly Mock<ILogger<VitalSignsService>> _loggerMock;

        private readonly VitalSignsService _vitalSignsService;

        public VitalSignsServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _vitalSignsRepositoryMock = new Mock<IVitalSignsRepository>();
            _loggerMock = new Mock<ILogger<VitalSignsService>>();

            _vitalSignsService = new VitalSignsService(
                _vitalSignsRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        // Create Vital Signs

        [Fact]
        public async Task CreateVitalSigns_HappyPath_ReturnsCreatedVitalSigns()
        {
            // Arrange

            var createVitalSignsDto = new CreateVitalSignsDto
            {
                BloodPressureSystolic = 120,
                BloodPressureDiastolic = 80,
                HeartRate = 72,
                Temperature = 36.6m,
                OxygenSaturation = 98,
                RecordedAt = DateTime.Now,
                NurseId = 1,
                MedicalRecordId = 2
            };

            var vitalSigns = new VitalSigns(
                createVitalSignsDto.BloodPressureSystolic,
                createVitalSignsDto.BloodPressureDiastolic,
                createVitalSignsDto.HeartRate,
                createVitalSignsDto.Temperature,
                createVitalSignsDto.OxygenSaturation,
                createVitalSignsDto.RecordedAt,
                createVitalSignsDto.NurseId,
                createVitalSignsDto.MedicalRecordId);

            var expectedDto = new VitalSignsDto
            {
                Id = 1,
                BloodPressureSystolic = createVitalSignsDto.BloodPressureSystolic,
                BloodPressureDiastolic = createVitalSignsDto.BloodPressureDiastolic,
                HeartRate = createVitalSignsDto.HeartRate,
                Temperature = createVitalSignsDto.Temperature,
                OxygenSaturation = createVitalSignsDto.OxygenSaturation,
                RecordedAt = createVitalSignsDto.RecordedAt,
                NurseId = createVitalSignsDto.NurseId,
                MedicalRecordId = createVitalSignsDto.MedicalRecordId
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.CreateVitalSignsAsync(
                    It.IsAny<VitalSigns>()))
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<VitalSignsDto>(
                    It.IsAny<VitalSigns>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _vitalSignsService.CreateVitalSignsAsync(
                    createVitalSignsDto);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.CreateVitalSignsAsync(
                        It.IsAny<VitalSigns>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        It.IsAny<VitalSigns>()),
                    Times.Once);
        }

        // Get All Vital Signs

        [Fact]
        public async Task GetAllVitalSigns_HappyPath_ReturnsListOfVitalSignsDto()
        {
            // Arrange

            var vitalSigns = new List<VitalSigns>
            {
                new VitalSigns(
                    120,
                    80,
                    72,
                    36.6m,
                    98,
                    DateTime.Now,
                    1,
                    2),

                new VitalSigns(
                    130,
                    85,
                    78,
                    37.0m,
                    97,
                    DateTime.Now.AddDays(1),
                    2,
                    3)
            };

            var expectedDtos = new List<VitalSignsDto>
            {
                new VitalSignsDto
                {
                    Id = 1,
                    BloodPressureSystolic = vitalSigns[0].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[0].BloodPressureDiastolic,
                    HeartRate = vitalSigns[0].HeartRate,
                    Temperature = vitalSigns[0].Temperature,
                    OxygenSaturation = vitalSigns[0].OxygenSaturation,
                    RecordedAt = vitalSigns[0].RecordedAt,
                    NurseId = vitalSigns[0].NurseId,
                    MedicalRecordId = vitalSigns[0].MedicalRecordId
                },

                new VitalSignsDto
                {
                    Id = 2,
                    BloodPressureSystolic = vitalSigns[1].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[1].BloodPressureDiastolic,
                    HeartRate = vitalSigns[1].HeartRate,
                    Temperature = vitalSigns[1].Temperature,
                    OxygenSaturation = vitalSigns[1].OxygenSaturation,
                    RecordedAt = vitalSigns[1].RecordedAt,
                    NurseId = vitalSigns[1].NurseId,
                    MedicalRecordId = vitalSigns[1].MedicalRecordId
                }
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetAllVitalSignsAsync())
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<List<VitalSignsDto>>(
                    vitalSigns))
                .Returns(expectedDtos);

            // Act

            var result =
                await _vitalSignsService.GetAllVitalSignsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetAllVitalSignsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<VitalSignsDto>>(
                        vitalSigns),
                    Times.Once);
        }

        [Fact]
        public async Task GetAllVitalSigns_NoVitalSigns_ReturnsEmptyList()
        {
            // Arrange

            var vitalSigns = new List<VitalSigns>();
            var expectedDtos = new List<VitalSignsDto>();

            _vitalSignsRepositoryMock
                .Setup(r => r.GetAllVitalSignsAsync())
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<List<VitalSignsDto>>(
                    vitalSigns))
                .Returns(expectedDtos);

            // Act

            var result =
                await _vitalSignsService.GetAllVitalSignsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetAllVitalSignsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<VitalSignsDto>>(
                        vitalSigns),
                    Times.Once);
        }

        // Get Vital Signs By ID

        [Fact]
        public async Task GetVitalSigns_ExistingId_ReturnsVitalSignsDto()
        {
            // Arrange

            var id = 1;

            var vitalSigns = new VitalSigns(
                120,
                80,
                72,
                36.6m,
                98,
                DateTime.Now,
                1,
                2);

            var expectedDto = new VitalSignsDto
            {
                Id = id,
                BloodPressureSystolic = vitalSigns.BloodPressureSystolic,
                BloodPressureDiastolic = vitalSigns.BloodPressureDiastolic,
                HeartRate = vitalSigns.HeartRate,
                Temperature = vitalSigns.Temperature,
                OxygenSaturation = vitalSigns.OxygenSaturation,
                RecordedAt = vitalSigns.RecordedAt,
                NurseId = vitalSigns.NurseId,
                MedicalRecordId = vitalSigns.MedicalRecordId
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<VitalSignsDto>(
                    vitalSigns))
                .Returns(expectedDto);

            // Act

            var result =
                await _vitalSignsService.GetVitalSignsAsync(id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        vitalSigns),
                    Times.Once);
        }

        [Fact]
        public async Task GetVitalSigns_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync((VitalSigns?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _vitalSignsService.GetVitalSignsAsync(id));

            Assert.Equal(
                $"Vital signs with ID {id} not found.",
                exception.Message);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        It.IsAny<VitalSigns>()),
                    Times.Never);
        }

        // Get Vital Signs By Medical Record ID

        [Fact]
        public async Task GetVitalSignsByMedicalRecordId_HappyPath_ReturnsListOfVitalSignsDto()
        {
            // Arrange

            var medicalRecordId = 2;

            var vitalSigns = new List<VitalSigns>
            {
                new VitalSigns(
                    120,
                    80,
                    72,
                    36.6m,
                    98,
                    DateTime.Now,
                    1,
                    medicalRecordId),

                new VitalSigns(
                    125,
                    82,
                    75,
                    36.8m,
                    97,
                    DateTime.Now.AddDays(1),
                    2,
                    medicalRecordId)
            };

            var expectedDtos = new List<VitalSignsDto>
            {
                new VitalSignsDto
                {
                    Id = 1,
                    BloodPressureSystolic = vitalSigns[0].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[0].BloodPressureDiastolic,
                    HeartRate = vitalSigns[0].HeartRate,
                    Temperature = vitalSigns[0].Temperature,
                    OxygenSaturation = vitalSigns[0].OxygenSaturation,
                    RecordedAt = vitalSigns[0].RecordedAt,
                    NurseId = vitalSigns[0].NurseId,
                    MedicalRecordId = vitalSigns[0].MedicalRecordId
                },

                new VitalSignsDto
                {
                    Id = 2,
                    BloodPressureSystolic = vitalSigns[1].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[1].BloodPressureDiastolic,
                    HeartRate = vitalSigns[1].HeartRate,
                    Temperature = vitalSigns[1].Temperature,
                    OxygenSaturation = vitalSigns[1].OxygenSaturation,
                    RecordedAt = vitalSigns[1].RecordedAt,
                    NurseId = vitalSigns[1].NurseId,
                    MedicalRecordId = vitalSigns[1].MedicalRecordId
                }
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsByMedicalRecordIdAsync(
                    medicalRecordId))
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<List<VitalSignsDto>>(
                    vitalSigns))
                .Returns(expectedDtos);

            // Act

            var result =
                await _vitalSignsService
                    .GetVitalSignsByMedicalRecordIdAsync(
                        medicalRecordId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsByMedicalRecordIdAsync(
                        medicalRecordId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<VitalSignsDto>>(
                        vitalSigns),
                    Times.Once);
        }

        // Get Vital Signs By Nurse ID

        [Fact]
        public async Task GetVitalSignsByNurseId_HappyPath_ReturnsListOfVitalSignsDto()
        {
            // Arrange

            var nurseId = 1;

            var vitalSigns = new List<VitalSigns>
            {
                new VitalSigns(
                    120,
                    80,
                    72,
                    36.6m,
                    98,
                    DateTime.Now,
                    nurseId,
                    2),

                new VitalSigns(
                    130,
                    85,
                    78,
                    37.0m,
                    97,
                    DateTime.Now.AddDays(1),
                    nurseId,
                    3)
            };

            var expectedDtos = new List<VitalSignsDto>
            {
                new VitalSignsDto
                {
                    Id = 1,
                    BloodPressureSystolic = vitalSigns[0].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[0].BloodPressureDiastolic,
                    HeartRate = vitalSigns[0].HeartRate,
                    Temperature = vitalSigns[0].Temperature,
                    OxygenSaturation = vitalSigns[0].OxygenSaturation,
                    RecordedAt = vitalSigns[0].RecordedAt,
                    NurseId = vitalSigns[0].NurseId,
                    MedicalRecordId = vitalSigns[0].MedicalRecordId
                },

                new VitalSignsDto
                {
                    Id = 2,
                    BloodPressureSystolic = vitalSigns[1].BloodPressureSystolic,
                    BloodPressureDiastolic = vitalSigns[1].BloodPressureDiastolic,
                    HeartRate = vitalSigns[1].HeartRate,
                    Temperature = vitalSigns[1].Temperature,
                    OxygenSaturation = vitalSigns[1].OxygenSaturation,
                    RecordedAt = vitalSigns[1].RecordedAt,
                    NurseId = vitalSigns[1].NurseId,
                    MedicalRecordId = vitalSigns[1].MedicalRecordId
                }
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsByNurseIdAsync(nurseId))
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<List<VitalSignsDto>>(
                    vitalSigns))
                .Returns(expectedDtos);

            // Act

            var result =
                await _vitalSignsService
                    .GetVitalSignsByNurseIdAsync(nurseId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsByNurseIdAsync(nurseId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<VitalSignsDto>>(
                        vitalSigns),
                    Times.Once);
        }

        // Update Vital Signs

        [Fact]
        public async Task UpdateVitalSigns_ExistingId_ReturnsUpdatedVitalSignsDto()
        {
            // Arrange

            var id = 1;

            var vitalSigns = new VitalSigns(
                120,
                80,
                72,
                36.6m,
                98,
                DateTime.Now,
                1,
                2);

            var updateVitalSignsDto = new UpdateVitalSignsDto
            {
                BloodPressureSystolic = 130,
                BloodPressureDiastolic = 85,
                HeartRate = 78,
                Temperature = 37.0m,
                OxygenSaturation = 97
            };

            var expectedDto = new VitalSignsDto
            {
                Id = id,
                BloodPressureSystolic =
                    updateVitalSignsDto.BloodPressureSystolic,

                BloodPressureDiastolic =
                    updateVitalSignsDto.BloodPressureDiastolic,

                HeartRate =
                    updateVitalSignsDto.HeartRate,

                Temperature =
                    updateVitalSignsDto.Temperature,

                OxygenSaturation =
                    updateVitalSignsDto.OxygenSaturation,

                RecordedAt = vitalSigns.RecordedAt,
                NurseId = vitalSigns.NurseId,
                MedicalRecordId = vitalSigns.MedicalRecordId
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync(vitalSigns);

            _vitalSignsRepositoryMock
                .Setup(r => r.UpdateVitalSignsAsync(
                    It.IsAny<VitalSigns>()))
                .ReturnsAsync(vitalSigns);

            _mapperMock
                .Setup(m => m.Map<VitalSignsDto>(
                    It.IsAny<VitalSigns>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _vitalSignsService.UpdateVitalSignsAsync(
                    updateVitalSignsDto,
                    id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.UpdateVitalSignsAsync(
                        It.IsAny<VitalSigns>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        It.IsAny<VitalSigns>()),
                    Times.Once);
        }

        [Fact]
        public async Task UpdateVitalSigns_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            var updateVitalSignsDto = new UpdateVitalSignsDto
            {
                BloodPressureSystolic = 130,
                BloodPressureDiastolic = 85,
                HeartRate = 78,
                Temperature = 37.0m,
                OxygenSaturation = 97
            };

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync((VitalSigns?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _vitalSignsService.UpdateVitalSignsAsync(
                        updateVitalSignsDto,
                        id));

            Assert.Equal(
                $"Vital signs with ID {id} not found.",
                exception.Message);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.UpdateVitalSignsAsync(
                        It.IsAny<VitalSigns>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        It.IsAny<VitalSigns>()),
                    Times.Never);
        }

        // Delete Vital Signs

        [Fact]
        public async Task DeleteVitalSigns_ExistingId_DeletesVitalSignsSuccessfully()
        {
            // Arrange

            var id = 1;

            var vitalSigns = new VitalSigns(
                120,
                80,
                72,
                36.6m,
                98,
                DateTime.Now,
                1,
                2);

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync(vitalSigns);

            _vitalSignsRepositoryMock
                .Setup(r => r.DeleteVitalSignsAsync(
                    vitalSigns))
                .Returns(Task.CompletedTask);

            // Act

            await _vitalSignsService.DeleteVitalSignsAsync(id);

            // Assert

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.DeleteVitalSignsAsync(
                        vitalSigns),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<VitalSignsDto>(
                        It.IsAny<VitalSigns>()),
                    Times.Never);
        }

        [Fact]
        public async Task DeleteVitalSigns_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _vitalSignsRepositoryMock
                .Setup(r => r.GetVitalSignsAsync(id))
                .ReturnsAsync((VitalSigns?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _vitalSignsService.DeleteVitalSignsAsync(id));

            Assert.Equal(
                $"Vital signs with ID {id} not found.",
                exception.Message);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.GetVitalSignsAsync(id),
                    Times.Once);

            _vitalSignsRepositoryMock
                .Verify(
                    r => r.DeleteVitalSignsAsync(
                        It.IsAny<VitalSigns>()),
                    Times.Never);
        }
    }
}
