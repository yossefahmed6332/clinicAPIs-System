using AutoMapper;
using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class PrescriptionServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPrescriptionRepository> _prescriptionRepositoryMock;
        private readonly Mock<ILogger<PrescriptionService>> _loggerMock;

        private readonly PrescriptionService _prescriptionService;

        public PrescriptionServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _prescriptionRepositoryMock = new Mock<IPrescriptionRepository>();
            _loggerMock = new Mock<ILogger<PrescriptionService>>();

            _prescriptionService = new PrescriptionService(
                _prescriptionRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        // Create Prescription

        [Fact]
        public async Task CreatePrescription_HappyPath_ReturnsCreatedPrescription()
        {
            // Arrange

            var createPrescriptionDto = new CreatePrescriptionDto
            {
                MedicalName = "Amoxicillin",
                Dosage = "500mg",
                Frequency = "Twice Daily",
                Duration = "7 Days",
                Instructions = "Take after meal",
                Diagnosis = "Bacterial Infection",
                Date = DateTime.Now,
                DoctorId = 1,
                medicalRecordId = 2
            };

            var prescription = new Prescription(
                createPrescriptionDto.MedicalName,
                createPrescriptionDto.Dosage,
                createPrescriptionDto.Frequency,
                createPrescriptionDto.Duration,
                createPrescriptionDto.Instructions,
                createPrescriptionDto.Diagnosis,
                createPrescriptionDto.Date,
                createPrescriptionDto.DoctorId,
                createPrescriptionDto.medicalRecordId);

            var expectedDto = new PrescriptionDto
            {
                Id = 1,
                MedicalName = createPrescriptionDto.MedicalName,
                Dosage = createPrescriptionDto.Dosage,
                Frequency = createPrescriptionDto.Frequency,
                Duration = createPrescriptionDto.Duration,
                Instructions = createPrescriptionDto.Instructions,
                Diagnosis = createPrescriptionDto.Diagnosis,
                Date = createPrescriptionDto.Date,
                DoctorId = createPrescriptionDto.DoctorId,
                medicalRecordId = createPrescriptionDto.medicalRecordId
            };

            _prescriptionRepositoryMock
                .Setup(r => r.CreatePrescriptionAsync(
                    It.IsAny<Prescription>()))
                .ReturnsAsync(prescription);

            _mapperMock
                .Setup(m => m.Map<PrescriptionDto>(
                    It.IsAny<Prescription>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _prescriptionService.CreatePrescriptionAsync(
                    createPrescriptionDto);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.CreatePrescriptionAsync(
                        It.IsAny<Prescription>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        It.IsAny<Prescription>()),
                    Times.Once);
        }

        // Get All Prescriptions

        [Fact]
        public async Task GetAllPrescriptions_HappyPath_ReturnsListOfPrescriptionDto()
        {
            // Arrange

            var prescriptions = new List<Prescription>
            {
                new Prescription(
                    "Amoxicillin",
                    "500mg",
                    "Twice Daily",
                    "7 Days",
                    "Take after meal",
                    "Bacterial Infection",
                    DateTime.Now,
                    1,
                    2),

                new Prescription(
                    "Ibuprofen",
                    "400mg",
                    "Once Daily",
                    "5 Days",
                    "Take after meal",
                    "Pain",
                    DateTime.Now.AddDays(1),
                    2,
                    3)
            };

            var expectedDtos = new List<PrescriptionDto>
            {
                new PrescriptionDto
                {
                    Id = 1,
                    MedicalName = prescriptions[0].MedicalName!,
                    Dosage = prescriptions[0].Dosage!,
                    Frequency = prescriptions[0].Frequency!,
                    Duration = prescriptions[0].Duration!,
                    Instructions = prescriptions[0].Instructions!,
                    Diagnosis = prescriptions[0].Diagnosis!,
                    Date = prescriptions[0].Date,
                    DoctorId = prescriptions[0].DoctorId,
                    medicalRecordId = prescriptions[0].MedicalRecordId
                },

                new PrescriptionDto
                {
                    Id = 2,
                    MedicalName = prescriptions[1].MedicalName!,
                    Dosage = prescriptions[1].Dosage!,
                    Frequency = prescriptions[1].Frequency!,
                    Duration = prescriptions[1].Duration!,
                    Instructions = prescriptions[1].Instructions!,
                    Diagnosis = prescriptions[1].Diagnosis!,
                    Date = prescriptions[1].Date,
                    DoctorId = prescriptions[1].DoctorId,
                    medicalRecordId = prescriptions[1].MedicalRecordId
                }
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetAllPrescriptionsAsync())
                .ReturnsAsync(prescriptions);

            _mapperMock
                .Setup(m => m.Map<List<PrescriptionDto>>(
                    prescriptions))
                .Returns(expectedDtos);

            // Act

            var result =
                await _prescriptionService.GetAllPrescriptionsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetAllPrescriptionsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PrescriptionDto>>(
                        prescriptions),
                    Times.Once);
        }

        [Fact]
        public async Task GetAllPrescriptions_NoPrescriptions_ReturnsEmptyList()
        {
            // Arrange

            var prescriptions = new List<Prescription>();
            var expectedDtos = new List<PrescriptionDto>();

            _prescriptionRepositoryMock
                .Setup(r => r.GetAllPrescriptionsAsync())
                .ReturnsAsync(prescriptions);

            _mapperMock
                .Setup(m => m.Map<List<PrescriptionDto>>(
                    prescriptions))
                .Returns(expectedDtos);

            // Act

            var result =
                await _prescriptionService.GetAllPrescriptionsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetAllPrescriptionsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PrescriptionDto>>(
                        prescriptions),
                    Times.Once);
        }

        // Get Prescription By ID

        [Fact]
        public async Task GetPrescription_ExistingId_ReturnsPrescriptionDto()
        {
            // Arrange

            var id = 1;

            var prescription = new Prescription(
                "Amoxicillin",
                "500mg",
                "Twice Daily",
                "7 Days",
                "Take after meal",
                "Bacterial Infection",
                DateTime.Now,
                1,
                2);

            var expectedDto = new PrescriptionDto
            {
                Id = id,
                MedicalName = prescription.MedicalName!,
                Dosage = prescription.Dosage!,
                Frequency = prescription.Frequency!,
                Duration = prescription.Duration!,
                Instructions = prescription.Instructions!,
                Diagnosis = prescription.Diagnosis!,
                Date = prescription.Date,
                DoctorId = prescription.DoctorId,
                medicalRecordId = prescription.MedicalRecordId
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync(prescription);

            _mapperMock
                .Setup(m => m.Map<PrescriptionDto>(
                    prescription))
                .Returns(expectedDto);

            // Act

            var result =
                await _prescriptionService.GetPrescriptionAsync(id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        prescription),
                    Times.Once);
        }

        [Fact]
        public async Task GetPrescription_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync((Prescription?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _prescriptionService.GetPrescriptionAsync(id));

            Assert.Equal(
                $"Prescription with ID{id} does not exist",
                exception.Message);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        It.IsAny<Prescription>()),
                    Times.Never);
        }

        // Get Prescriptions By Medical Record ID

        [Fact]
        public async Task GetPrescriptionsByMedicalRecordId_HappyPath_ReturnsListOfPrescriptionDto()
        {
            // Arrange

            var medicalRecordId = 2;

            var prescriptions = new List<Prescription>
            {
                new Prescription(
                    "Amoxicillin",
                    "500mg",
                    "Twice Daily",
                    "7 Days",
                    "Take after meal",
                    "Bacterial Infection",
                    DateTime.Now,
                    1,
                    medicalRecordId),

                new Prescription(
                    "Ibuprofen",
                    "400mg",
                    "Once Daily",
                    "5 Days",
                    "Take after meal",
                    "Pain",
                    DateTime.Now.AddDays(1),
                    2,
                    medicalRecordId)
            };

            var expectedDtos = new List<PrescriptionDto>
            {
                new PrescriptionDto
                {
                    Id = 1,
                    MedicalName = prescriptions[0].MedicalName!,
                    Dosage = prescriptions[0].Dosage!,
                    Frequency = prescriptions[0].Frequency!,
                    Duration = prescriptions[0].Duration!,
                    Instructions = prescriptions[0].Instructions!,
                    Diagnosis = prescriptions[0].Diagnosis!,
                    Date = prescriptions[0].Date,
                    DoctorId = prescriptions[0].DoctorId,
                    medicalRecordId = prescriptions[0].MedicalRecordId
                },

                new PrescriptionDto
                {
                    Id = 2,
                    MedicalName = prescriptions[1].MedicalName!,
                    Dosage = prescriptions[1].Dosage!,
                    Frequency = prescriptions[1].Frequency!,
                    Duration = prescriptions[1].Duration!,
                    Instructions = prescriptions[1].Instructions!,
                    Diagnosis = prescriptions[1].Diagnosis!,
                    Date = prescriptions[1].Date,
                    DoctorId = prescriptions[1].DoctorId,
                    medicalRecordId = prescriptions[1].MedicalRecordId
                }
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionsByMedicalRecordIdAsync(
                    medicalRecordId))
                .ReturnsAsync(prescriptions);

            _mapperMock
                .Setup(m => m.Map<List<PrescriptionDto>>(
                    prescriptions))
                .Returns(expectedDtos);

            // Act

            var result =
                await _prescriptionService
                    .GetPrescriptionsByMedicalRecordIdAsync(
                        medicalRecordId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionsByMedicalRecordIdAsync(
                        medicalRecordId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PrescriptionDto>>(
                        prescriptions),
                    Times.Once);
        }

        // Get Prescriptions By Doctor ID

        [Fact]
        public async Task GetPrescriptionsByDoctorId_HappyPath_ReturnsListOfPrescriptionDto()
        {
            // Arrange

            var doctorId = 1;

            var prescriptions = new List<Prescription>
            {
                new Prescription(
                    "Amoxicillin",
                    "500mg",
                    "Twice Daily",
                    "7 Days",
                    "Take after meal",
                    "Bacterial Infection",
                    DateTime.Now,
                    doctorId,
                    2),

                new Prescription(
                    "Ibuprofen",
                    "400mg",
                    "Once Daily",
                    "5 Days",
                    "Take after meal",
                    "Pain",
                    DateTime.Now.AddDays(1),
                    doctorId,
                    3)
            };

            var expectedDtos = new List<PrescriptionDto>
            {
                new PrescriptionDto
                {
                    Id = 1,
                    MedicalName = prescriptions[0].MedicalName!,
                    Dosage = prescriptions[0].Dosage!,
                    Frequency = prescriptions[0].Frequency!,
                    Duration = prescriptions[0].Duration!,
                    Instructions = prescriptions[0].Instructions!,
                    Diagnosis = prescriptions[0].Diagnosis!,
                    Date = prescriptions[0].Date,
                    DoctorId = prescriptions[0].DoctorId,
                    medicalRecordId = prescriptions[0].MedicalRecordId
                },

                new PrescriptionDto
                {
                    Id = 2,
                    MedicalName = prescriptions[1].MedicalName!,
                    Dosage = prescriptions[1].Dosage!,
                    Frequency = prescriptions[1].Frequency!,
                    Duration = prescriptions[1].Duration!,
                    Instructions = prescriptions[1].Instructions!,
                    Diagnosis = prescriptions[1].Diagnosis!,
                    Date = prescriptions[1].Date,
                    DoctorId = prescriptions[1].DoctorId,
                    medicalRecordId = prescriptions[1].MedicalRecordId
                }
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionsByDoctorIdAsync(doctorId))
                .ReturnsAsync(prescriptions);

            _mapperMock
                .Setup(m => m.Map<List<PrescriptionDto>>(
                    prescriptions))
                .Returns(expectedDtos);

            // Act

            var result =
                await _prescriptionService
                    .GetPrescriptionsByDoctorIdAsync(doctorId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionsByDoctorIdAsync(doctorId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PrescriptionDto>>(
                        prescriptions),
                    Times.Once);
        }

        // Update Prescription

        [Fact]
        public async Task UpdatePrescription_ExistingId_ReturnsUpdatedPrescriptionDto()
        {
            // Arrange

            var id = 1;

            var prescription = new Prescription(
                "Amoxicillin",
                "500mg",
                "Twice Daily",
                "7 Days",
                "Take after meal",
                "Bacterial Infection",
                DateTime.Now,
                1,
                2);

            var updatePrescriptionDto = new UpdatePrescriptionDto
            {
                MedicalName = "Ibuprofen",
                Dosage = "400mg",
                Frequency = "Once Daily",
                Duration = "5 Days",
                Instructions = "Take after meal",
                Diagnosis = "Pain"
            };

            var expectedDto = new PrescriptionDto
            {
                Id = id,
                MedicalName = updatePrescriptionDto.MedicalName,
                Dosage = updatePrescriptionDto.Dosage,
                Frequency = updatePrescriptionDto.Frequency,
                Duration = updatePrescriptionDto.Duration,
                Instructions = updatePrescriptionDto.Instructions,
                Diagnosis = updatePrescriptionDto.Diagnosis,
                Date = prescription.Date,
                DoctorId = prescription.DoctorId,
                medicalRecordId = prescription.MedicalRecordId
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync(prescription);

            _prescriptionRepositoryMock
                .Setup(r => r.UpdatePrescriptionAsync(
                    It.IsAny<Prescription>()))
                .ReturnsAsync(prescription);

            _mapperMock
                .Setup(m => m.Map<PrescriptionDto>(
                    It.IsAny<Prescription>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _prescriptionService.UpdatePrescriptionAsync(
                    updatePrescriptionDto,
                    id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.UpdatePrescriptionAsync(
                        It.IsAny<Prescription>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        It.IsAny<Prescription>()),
                    Times.Once);
        }

        [Fact]
        public async Task UpdatePrescription_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            var updatePrescriptionDto = new UpdatePrescriptionDto
            {
                MedicalName = "Ibuprofen",
                Dosage = "400mg",
                Frequency = "Once Daily",
                Duration = "5 Days",
                Instructions = "Take after meal",
                Diagnosis = "Pain"
            };

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync((Prescription?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _prescriptionService.UpdatePrescriptionAsync(
                        updatePrescriptionDto,
                        id));

            Assert.Equal(
                $"Prescription with ID {id} does not exist",
                exception.Message);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.UpdatePrescriptionAsync(
                        It.IsAny<Prescription>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        It.IsAny<Prescription>()),
                    Times.Never);
        }

        // Delete Prescription

        [Fact]
        public async Task DeletePrescription_ExistingId_DeletesPrescriptionSuccessfully()
        {
            // Arrange

            var id = 1;

            var prescription = new Prescription(
                "Amoxicillin",
                "500mg",
                "Twice Daily",
                "7 Days",
                "Take after meal",
                "Bacterial Infection",
                DateTime.Now,
                1,
                2);

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync(prescription);

            _prescriptionRepositoryMock
                .Setup(r => r.DeletePrescriptionAsync(
                    prescription))
                .Returns(Task.CompletedTask);

            // Act

            await _prescriptionService.DeletePrescriptionAsync(id);

            // Assert

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.DeletePrescriptionAsync(
                        prescription),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PrescriptionDto>(
                        It.IsAny<Prescription>()),
                    Times.Never);
        }

        [Fact]
        public async Task DeletePrescription_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _prescriptionRepositoryMock
                .Setup(r => r.GetPrescriptionAsync(id))
                .ReturnsAsync((Prescription?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _prescriptionService.DeletePrescriptionAsync(id));

            Assert.Equal(
                $"User with ID {id} not found",
                exception.Message);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.GetPrescriptionAsync(id),
                    Times.Once);

            _prescriptionRepositoryMock
                .Verify(
                    r => r.DeletePrescriptionAsync(
                        It.IsAny<Prescription>()),
                    Times.Never);
        }
    }
}
