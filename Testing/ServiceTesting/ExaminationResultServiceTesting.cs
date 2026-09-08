using AutoMapper;
using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class ExaminationResultServiceTesting
    {
        private readonly IExaminationResultService _examinationResultService;
        private readonly Mock<IMapper> _IMapperMock;
        private readonly Mock<IMedicalRecordService> _IMedicalRecordServiceMock;
        private readonly Mock<INurseService> _INurseServiceMock;
        private readonly Mock<IExaminationResultRepository> _examinationResultRepositoryMock;
        private readonly Mock<ILogger<ExaminationResultService>> _ILoggerMock;

        public ExaminationResultServiceTesting()
        {
            _IMapperMock = new Mock<IMapper>();
            _IMedicalRecordServiceMock = new Mock<IMedicalRecordService>();
            _INurseServiceMock = new Mock<INurseService>();
            _ILoggerMock = new Mock<ILogger<ExaminationResultService>>();
            _examinationResultRepositoryMock = new Mock<IExaminationResultRepository>();

            _examinationResultService = new ExaminationResultService(
                _IMapperMock.Object,
                _examinationResultRepositoryMock.Object,
                _ILoggerMock.Object,
                _IMedicalRecordServiceMock.Object,
                _INurseServiceMock.Object
            );
        }


        // Create Examination Result

        [Fact]
        public async Task CreateExaminationResult_HappyPath_ReturnsExaminationResultDto()
        {
            // Arrange
            var createExaminationResultDto = new CreateExaminationResultDto
            {
                TestType = "Blood Test",
                ResultValue = "Positive",
                Unit = "mg/dL",
                NormalRange = "0-100",
                Note = "Sample note",
                RecordedAt = DateTime.Now,
                NurseId = 1,
                MedicalRecordId = 1
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = createExaminationResultDto.MedicalRecordId
            };

            var nurseDto = new NurseDto
            {
                Id = createExaminationResultDto.NurseId
            };

            var examinationResult = new ExaminationResult(
                createExaminationResultDto.TestType,
                createExaminationResultDto.ResultValue,
                createExaminationResultDto.Unit,
                createExaminationResultDto.NormalRange,
                createExaminationResultDto.Note,
                createExaminationResultDto.RecordedAt,
                createExaminationResultDto.NurseId,
                createExaminationResultDto.MedicalRecordId
            );

            var expectedExaminationResultDto = new ExaminationResultDto
            {
                Id = 1,
                TestType = createExaminationResultDto.TestType,
                ResultValue = createExaminationResultDto.ResultValue,
                Unit = createExaminationResultDto.Unit,
                NormalRange = createExaminationResultDto.NormalRange,
                Note = createExaminationResultDto.Note,
                RecordedAt = createExaminationResultDto.RecordedAt,
                NurseId = createExaminationResultDto.NurseId,
                MedicalRecordId = createExaminationResultDto.MedicalRecordId
            };

            _IMedicalRecordServiceMock
                .Setup(s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId))
                .ReturnsAsync(medicalRecordDto);

            _INurseServiceMock
                .Setup(s => s.GetNurseAsync(createExaminationResultDto.NurseId))
                .ReturnsAsync(nurseDto);

            _examinationResultRepositoryMock
                .Setup(r => r.CreateExaminationResultAsync(It.IsAny<ExaminationResult>()))
                .ReturnsAsync(examinationResult);

            _IMapperMock
                .Setup(m => m.Map<ExaminationResultDto>(It.IsAny<ExaminationResult>()))
                .Returns(expectedExaminationResultDto);

            // Act
            var result = await _examinationResultService
                .CreateExaminationResultAsync(createExaminationResultDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExaminationResultDto, result);

            _IMedicalRecordServiceMock
                .Verify(
                    s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId),
                    Times.Once);

            _INurseServiceMock
                .Verify(
                    s => s.GetNurseAsync(createExaminationResultDto.NurseId),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.CreateExaminationResultAsync(It.IsAny<ExaminationResult>()),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(It.IsAny<ExaminationResult>()),
                    Times.Once);
        }


        [Fact]
        public async Task CreateExaminationResult_MedicalRecordIdNotFound_ThrowsException()
        {
            // Arrange
            var createExaminationResultDto = new CreateExaminationResultDto
            {
                TestType = "Blood Test",
                ResultValue = "Positive",
                Unit = "mg/dL",
                NormalRange = "0-100",
                Note = "Sample note",
                RecordedAt = DateTime.Now,
                NurseId = 1,
                MedicalRecordId = 999
            };

            _IMedicalRecordServiceMock
                .Setup(s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId))
                .ReturnsAsync((MedicalRecordDto)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _examinationResultService
                    .CreateExaminationResultAsync(createExaminationResultDto));

            Assert.Equal(
                $"Medical Record with ID {createExaminationResultDto.MedicalRecordId} not found.",
                exception.Message);

            _IMedicalRecordServiceMock
                .Verify(
                    s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId),
                    Times.Once);

            _INurseServiceMock
                .Verify(
                    s => s.GetNurseAsync(It.IsAny<int>()),
                    Times.Never);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.CreateExaminationResultAsync(It.IsAny<ExaminationResult>()),
                    Times.Never);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(It.IsAny<ExaminationResult>()),
                    Times.Never);
        }


        [Fact]
        public async Task CreateExaminationResult_NurseIdNotFound_ThrowsException()
        {
            // Arrange
            var createExaminationResultDto = new CreateExaminationResultDto
            {
                TestType = "Blood Test",
                ResultValue = "Positive",
                Unit = "mg/dL",
                NormalRange = "0-100",
                Note = "Sample note",
                RecordedAt = DateTime.Now,
                NurseId = 999,
                MedicalRecordId = 1
            };

            var medicalRecordDto = new MedicalRecordDto
            {
                Id = createExaminationResultDto.MedicalRecordId
            };

            _IMedicalRecordServiceMock
                .Setup(s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId))
                .ReturnsAsync(medicalRecordDto);

            _INurseServiceMock
                .Setup(s => s.GetNurseAsync(createExaminationResultDto.NurseId))
                .ReturnsAsync((NurseDto)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _examinationResultService
                    .CreateExaminationResultAsync(createExaminationResultDto));

            Assert.Equal(
                $"Nurse with ID {createExaminationResultDto.NurseId} not found.",
                exception.Message);

            _IMedicalRecordServiceMock
                .Verify(
                    s => s.GetMedicalRecordAsync(createExaminationResultDto.MedicalRecordId),
                    Times.Once);

            _INurseServiceMock
                .Verify(
                    s => s.GetNurseAsync(createExaminationResultDto.NurseId),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.CreateExaminationResultAsync(It.IsAny<ExaminationResult>()),
                    Times.Never);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(It.IsAny<ExaminationResult>()),
                    Times.Never);
        }


        // Get All Examination Results

        [Fact]
        public async Task GetAllExaminationResults_HappyPath_ReturnsListOfExaminationResultDto()
        {
            // Arrange
            var examinationResults = new List<ExaminationResult>
            {
                new ExaminationResult(
                    "Blood Test",
                    "Positive",
                    "mg/dL",
                    "0-100",
                    "Sample note",
                    DateTime.Now,
                    1,
                    1),

                new ExaminationResult(
                    "Urine Test",
                    "Negative",
                    "mg/dL",
                    "0-50",
                    "Sample note 2",
                    DateTime.Now,
                    2,
                    2)
            };

            var expectedExaminationResultDtos = new List<ExaminationResultDto>
            {
                new ExaminationResultDto
                {
                    Id = 1,
                    TestType = examinationResults[0].TestType!,
                    ResultValue = examinationResults[0].ResultValue!,
                    Unit = examinationResults[0].Unit!,
                    NormalRange = examinationResults[0].NormalRange!,
                    Note = examinationResults[0].Notes!,
                    RecordedAt = examinationResults[0].RecordedAt,
                    NurseId = 1,
                    MedicalRecordId = 1
                },

                new ExaminationResultDto
                {
                    Id = 2,
                    TestType = examinationResults[1].TestType!,
                    ResultValue = examinationResults[1].ResultValue!,
                    Unit = examinationResults[1].Unit!,
                    NormalRange = examinationResults[1].NormalRange!,
                    Note = examinationResults[1].Notes!,
                    RecordedAt = examinationResults[1].RecordedAt,
                    NurseId = 2,
                    MedicalRecordId = 2
                }
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetAllExaminationResultsAsync())
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetAllExaminationResultsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExaminationResultDtos.Count, result.Count);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetAllExaminationResultsAsync(),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);
        }


        [Fact]
        public async Task GetAllExaminationResults_NoResults_ReturnsEmptyList()
        {
            // Arrange
            var examinationResults = new List<ExaminationResult>();
            var expectedExaminationResultDtos = new List<ExaminationResultDto>();

            _examinationResultRepositoryMock
                .Setup(r => r.GetAllExaminationResultsAsync())
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetAllExaminationResultsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetAllExaminationResultsAsync(),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);
        }


        // Get Examination Result By ID

        [Fact]
        public async Task GetExaminationResult_ExistingId_ReturnsExaminationResultDto()
        {
            // Arrange
            var id = 1;

            var examinationResult = new ExaminationResult(
                "Blood Test",
                "Positive",
                "mg/dL",
                "0-100",
                "Sample note",
                DateTime.Now,
                1,
                1);

            var expectedExaminationResultDto = new ExaminationResultDto
            {
                Id = 1,
                TestType = examinationResult.TestType!,
                ResultValue = examinationResult.ResultValue!,
                Unit = examinationResult.Unit!,
                NormalRange = examinationResult.NormalRange!,
                Note = examinationResult.Notes!,
                RecordedAt = examinationResult.RecordedAt,
                NurseId = 1,
                MedicalRecordId = 1
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync(examinationResult);

            _IMapperMock
                .Setup(m => m.Map<ExaminationResultDto>(examinationResult))
                .Returns(expectedExaminationResultDto);

            // Act
            var result = await _examinationResultService
                .GetExaminationResultAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExaminationResultDto.Id, result.Id);
            Assert.Equal(expectedExaminationResultDto.TestType, result.TestType);
            Assert.Equal(expectedExaminationResultDto.ResultValue, result.ResultValue);
            Assert.Equal(expectedExaminationResultDto.Unit, result.Unit);
            Assert.Equal(expectedExaminationResultDto.NormalRange, result.NormalRange);
            Assert.Equal(expectedExaminationResultDto.Note, result.Note);
            Assert.Equal(expectedExaminationResultDto.RecordedAt, result.RecordedAt);
            Assert.Equal(expectedExaminationResultDto.NurseId, result.NurseId);
            Assert.Equal(expectedExaminationResultDto.MedicalRecordId, result.MedicalRecordId);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(examinationResult),
                    Times.Once);
        }


        [Fact]
        public async Task GetExaminationResult_NonExistingId_ThrowsException()
        {
            // Arrange
            var id = 999;

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync((ExaminationResult)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _examinationResultService.GetExaminationResultAsync(id));

            Assert.Equal(
                $"Examination Result with ID {id} not found.",
                exception.Message);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(
                        It.IsAny<ExaminationResult>()),
                    Times.Never);
        }


        // Get Examination Results By Nurse ID

        [Fact]
        public async Task GetExaminationResultsByNurseId_ExistingNurse_ReturnsListOfExaminationResultDto()
        {
            // Arrange
            var nurseId = 1;

            var examinationResults = new List<ExaminationResult>
            {
                new ExaminationResult(
                    "Blood Test",
                    "Positive",
                    "mg/dL",
                    "0-100",
                    "Sample note",
                    DateTime.Now,
                    nurseId,
                    1),

                new ExaminationResult(
                    "Urine Test",
                    "Negative",
                    "mg/dL",
                    "0-50",
                    "Sample note 2",
                    DateTime.Now,
                    nurseId,
                    2)
            };

            var expectedExaminationResultDtos = new List<ExaminationResultDto>
            {
                new ExaminationResultDto
                {
                    Id = 1,
                    TestType = examinationResults[0].TestType!,
                    ResultValue = examinationResults[0].ResultValue!,
                    Unit = examinationResults[0].Unit!,
                    NormalRange = examinationResults[0].NormalRange!,
                    Note = examinationResults[0].Notes!,
                    RecordedAt = examinationResults[0].RecordedAt,
                    NurseId = nurseId,
                    MedicalRecordId = 1
                },

                new ExaminationResultDto
                {
                    Id = 2,
                    TestType = examinationResults[1].TestType!,
                    ResultValue = examinationResults[1].ResultValue!,
                    Unit = examinationResults[1].Unit!,
                    NormalRange = examinationResults[1].NormalRange!,
                    Note = examinationResults[1].Notes!,
                    RecordedAt = examinationResults[1].RecordedAt,
                    NurseId = nurseId,
                    MedicalRecordId = 2
                }
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultsByNurseIdAsync(nurseId))
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetExaminationResultsByNurseIdAsync(nurseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExaminationResultDtos.Count, result.Count);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultsByNurseIdAsync(nurseId),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);

            _INurseServiceMock
                .Verify(
                    s => s.GetNurseAsync(It.IsAny<int>()),
                    Times.Never);
        }


        [Fact]
        public async Task GetExaminationResultsByNurseId_NoResults_ReturnsEmptyList()
        {
            // Arrange
            var nurseId = 999;

            var examinationResults = new List<ExaminationResult>();
            var expectedExaminationResultDtos = new List<ExaminationResultDto>();

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultsByNurseIdAsync(nurseId))
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetExaminationResultsByNurseIdAsync(nurseId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultsByNurseIdAsync(nurseId),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);

            _INurseServiceMock
                .Verify(
                    s => s.GetNurseAsync(It.IsAny<int>()),
                    Times.Never);
        }


        // Get Examination Results By Medical Record ID

        [Fact]
        public async Task GetExaminationResultsByMedicalRecordId_ExistingMedicalRecord_ReturnsListOfExaminationResultDto()
        {
            // Arrange
            var medicalRecordId = 1;

            var examinationResults = new List<ExaminationResult>
            {
                new ExaminationResult(
                    "Blood Test",
                    "Positive",
                    "mg/dL",
                    "0-100",
                    "Sample note",
                    DateTime.Now,
                    1,
                    medicalRecordId),

                new ExaminationResult(
                    "Urine Test",
                    "Negative",
                    "mg/dL",
                    "0-50",
                    "Sample note 2",
                    DateTime.Now,
                    2,
                    medicalRecordId)
            };

            var expectedExaminationResultDtos = new List<ExaminationResultDto>
            {
                new ExaminationResultDto
                {
                    Id = 1,
                    TestType = examinationResults[0].TestType!,
                    ResultValue = examinationResults[0].ResultValue!,
                    Unit = examinationResults[0].Unit!,
                    NormalRange = examinationResults[0].NormalRange!,
                    Note = examinationResults[0].Notes!,
                    RecordedAt = examinationResults[0].RecordedAt,
                    NurseId = 1,
                    MedicalRecordId = medicalRecordId
                },

                new ExaminationResultDto
                {
                    Id = 2,
                    TestType = examinationResults[1].TestType!,
                    ResultValue = examinationResults[1].ResultValue!,
                    Unit = examinationResults[1].Unit!,
                    NormalRange = examinationResults[1].NormalRange!,
                    Note = examinationResults[1].Notes!,
                    RecordedAt = examinationResults[1].RecordedAt,
                    NurseId = 2,
                    MedicalRecordId = medicalRecordId
                }
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId))
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExaminationResultDtos.Count, result.Count);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);

            _IMedicalRecordServiceMock
                .Verify(
                    s => s.GetMedicalRecordAsync(It.IsAny<int>()),
                    Times.Never);
        }


        [Fact]
        public async Task GetExaminationResultsByMedicalRecordId_NoResults_ReturnsEmptyList()
        {
            // Arrange
            var medicalRecordId = 999;

            var examinationResults = new List<ExaminationResult>();
            var expectedExaminationResultDtos = new List<ExaminationResultDto>();

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId))
                .ReturnsAsync(examinationResults);

            _IMapperMock
                .Setup(m => m.Map<List<ExaminationResultDto>>(
                    It.IsAny<List<ExaminationResult>>()))
                .Returns(expectedExaminationResultDtos);

            // Act
            var result = await _examinationResultService
                .GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<List<ExaminationResultDto>>(
                        It.IsAny<List<ExaminationResult>>()),
                    Times.Once);

            _IMedicalRecordServiceMock
                .Verify(
                    s => s.GetMedicalRecordAsync(It.IsAny<int>()),
                    Times.Never);
        }


        // Update Examination Result

        [Fact]
        public async Task UpdateExaminationResult_HappyPath_ReturnsUpdatedExaminationResultDto()
        {
            // Arrange
            var id = 1;

            var updateExaminationResultDto = new UpdateExaminationResultDto
            {
                TestType = "Updated Blood Test",
                ResultValue = "Negative",
                Unit = "mg/dL",
                NormalRange = "0-100",
                Note = "Updated note"
            };

            var examinationResult = new ExaminationResult(
                "Blood Test",
                "Positive",
                "mg/dL",
                "0-100",
                "Old note",
                DateTime.Now,
                1,
                1);

            var updatedExaminationResultDto = new ExaminationResultDto
            {
                Id = id,
                TestType = updateExaminationResultDto.TestType,
                ResultValue = updateExaminationResultDto.ResultValue,
                Unit = updateExaminationResultDto.Unit,
                NormalRange = updateExaminationResultDto.NormalRange,
                Note = updateExaminationResultDto.Note,
                RecordedAt = examinationResult.RecordedAt,
                NurseId = 1,
                MedicalRecordId = 1
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync(examinationResult);

            _examinationResultRepositoryMock
                .Setup(r => r.UpdateExaminationResultAsync(examinationResult))
                .ReturnsAsync(examinationResult);

            _IMapperMock
                .Setup(m => m.Map<ExaminationResultDto>(examinationResult))
                .Returns(updatedExaminationResultDto);

            // Act
            var result = await _examinationResultService
                .UpdateExaminationResultAsync(updateExaminationResultDto, id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedExaminationResultDto, result);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.UpdateExaminationResultAsync(examinationResult),
                    Times.Once);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(examinationResult),
                    Times.Once);
        }


        [Fact]
        public async Task UpdateExaminationResult_NonExistingId_ThrowsException()
        {
            // Arrange
            var id = 999;

            var updateExaminationResultDto = new UpdateExaminationResultDto
            {
                TestType = "Updated Blood Test",
                ResultValue = "Negative",
                Unit = "mg/dL",
                NormalRange = "0-100",
                Note = "Updated note"
            };

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync((ExaminationResult)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _examinationResultService
                    .UpdateExaminationResultAsync(updateExaminationResultDto, id));

            Assert.Equal(
                $"Examination result with ID {id} not found",
                exception.Message);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.UpdateExaminationResultAsync(It.IsAny<ExaminationResult>()),
                    Times.Never);

            _IMapperMock
                .Verify(
                    m => m.Map<ExaminationResultDto>(
                        It.IsAny<ExaminationResult>()),
                    Times.Never);
        }


        // Delete Examination Result

        [Fact]
        public async Task DeleteExaminationResult_ExistingId_DeletesExaminationResult()
        {
            // Arrange
            var id = 1;

            var examinationResult = new ExaminationResult(
                "Blood Test",
                "Positive",
                "mg/dL",
                "0-100",
                "Sample note",
                DateTime.Now,
                1,
                1);

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync(examinationResult);

            _examinationResultRepositoryMock
                .Setup(r => r.DeleteExaminationResultAsync(examinationResult))
                .Returns(Task.CompletedTask);

            // Act
            await _examinationResultService.DeleteExaminationResultAsync(id);

            // Assert
            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.DeleteExaminationResultAsync(examinationResult),
                    Times.Once);
        }


        [Fact]
        public async Task DeleteExaminationResult_NonExistingId_ThrowsException()
        {
            // Arrange
            var id = 999;

            _examinationResultRepositoryMock
                .Setup(r => r.GetExaminationResultAsync(id))
                .ReturnsAsync((ExaminationResult)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _examinationResultService
                    .DeleteExaminationResultAsync(id));

            Assert.Equal(
                $"Examination result with ID {id} not found",
                exception.Message);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.GetExaminationResultAsync(id),
                    Times.Once);

            _examinationResultRepositoryMock
                .Verify(
                    r => r.DeleteExaminationResultAsync(
                        It.IsAny<ExaminationResult>()),
                    Times.Never);
        }
    }
}
