using AutoMapper;
using clinicAPIsSystem.DTOs.PaymentOperationDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class PaymentOperationServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPaymentOperationRepository> _paymentOperationRepositoryMock;
        private readonly Mock<ILogger<PaymentOperationService>> _loggerMock;

        private readonly PaymentOperationService _paymentOperationService;

        public PaymentOperationServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _paymentOperationRepositoryMock = new Mock<IPaymentOperationRepository>();
            _loggerMock = new Mock<ILogger<PaymentOperationService>>();

            _paymentOperationService = new PaymentOperationService(
                _paymentOperationRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        // Create Payment Operation

        [Fact]
        public async Task CreatePaymentOperation_HappyPath_ReturnsCreatedPaymentOperation()
        {
            // Arrange

            var createPaymentOperationDto = new CreatePaymentOperationDto
            {
                Amount = 1000,
                Date = DateTime.Now,
                OperationType = OperationType.Payment,
                OperationStatus = OperationStatus.Completed,
                PatientId = 1,
                AccountantId = 2,
                PaymentMethod = PaymentMethod.Cash
            };

            var paymentOperation = new PaymentOperation(
                createPaymentOperationDto.Amount,
                createPaymentOperationDto.Date,
                createPaymentOperationDto.OperationType,
                createPaymentOperationDto.OperationStatus,
                createPaymentOperationDto.PatientId,
                createPaymentOperationDto.AccountantId,
                createPaymentOperationDto.PaymentMethod);

            var expectedDto = new PaymentOperationDto
            {
                Id = 1,
                Amount = createPaymentOperationDto.Amount,
                Date = createPaymentOperationDto.Date,
                OperationType = createPaymentOperationDto.OperationType,
                OperationStatus = createPaymentOperationDto.OperationStatus,
                PatientId = createPaymentOperationDto.PatientId,
                AccountantId = createPaymentOperationDto.AccountantId,
                PaymentMethod = createPaymentOperationDto.PaymentMethod
            };

            _paymentOperationRepositoryMock
                .Setup(r => r.CreatePaymentOperationAsync(
                    It.IsAny<PaymentOperation>()))
                .ReturnsAsync(paymentOperation);

            _mapperMock
                .Setup(m => m.Map<PaymentOperationDto>(
                    It.IsAny<PaymentOperation>()))
                .Returns(expectedDto);

            // Act

            var result =
                await _paymentOperationService.CreatePaymentOperationAsync(
                    createPaymentOperationDto);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.CreatePaymentOperationAsync(
                        It.IsAny<PaymentOperation>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PaymentOperationDto>(
                        It.IsAny<PaymentOperation>()),
                    Times.Once);
        }

        // Get All Payment Operations

        [Fact]
        public async Task GetAllPaymentOperations_HappyPath_ReturnsListOfPaymentOperationDto()
        {
            // Arrange

            var paymentOperations = new List<PaymentOperation>
            {
                new PaymentOperation(
                    1000,
                    DateTime.Now,
                    OperationType.Payment,
                    OperationStatus.Completed,
                    1,
                    2,
                    PaymentMethod.Cash),

                new PaymentOperation(
                    2000,
                    DateTime.Now.AddDays(1),
                    OperationType.Refund,
                    OperationStatus.Completed,
                    2,
                    3,
                    PaymentMethod.Card)
            };

            var expectedDtos = new List<PaymentOperationDto>
            {
                new PaymentOperationDto
                {
                    Id = 1,
                    Amount = paymentOperations[0].Amount,
                    Date = paymentOperations[0].Date,
                    OperationType = paymentOperations[0].OperationType,
                    OperationStatus = paymentOperations[0].Status,
                    PatientId = paymentOperations[0].PatientId,
                    AccountantId = paymentOperations[0].AccountantId,
                    PaymentMethod = paymentOperations[0].PaymentMethod
                },

                new PaymentOperationDto
                {
                    Id = 2,
                    Amount = paymentOperations[1].Amount,
                    Date = paymentOperations[1].Date,
                    OperationType = paymentOperations[1].OperationType,
                    OperationStatus = paymentOperations[1].Status,
                    PatientId = paymentOperations[1].PatientId,
                    AccountantId = paymentOperations[1].AccountantId,
                    PaymentMethod = paymentOperations[1].PaymentMethod
                }
            };

            _paymentOperationRepositoryMock
                .Setup(r => r.GetAllPaymentOperationsAsync())
                .ReturnsAsync(paymentOperations);

            _mapperMock
                .Setup(m => m.Map<List<PaymentOperationDto>>(
                    paymentOperations))
                .Returns(expectedDtos);

            // Act

            var result =
                await _paymentOperationService.GetAllPaymentOperationsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetAllPaymentOperationsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PaymentOperationDto>>(
                        paymentOperations),
                    Times.Once);
        }

        [Fact]
        public async Task GetAllPaymentOperations_NoOperations_ReturnsEmptyList()
        {
            // Arrange

            var paymentOperations = new List<PaymentOperation>();
            var expectedDtos = new List<PaymentOperationDto>();

            _paymentOperationRepositoryMock
                .Setup(r => r.GetAllPaymentOperationsAsync())
                .ReturnsAsync(paymentOperations);

            _mapperMock
                .Setup(m => m.Map<List<PaymentOperationDto>>(
                    paymentOperations))
                .Returns(expectedDtos);

            // Act

            var result =
                await _paymentOperationService.GetAllPaymentOperationsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetAllPaymentOperationsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PaymentOperationDto>>(
                        paymentOperations),
                    Times.Once);
        }

        // Get Payment Operation By ID

        [Fact]
        public async Task GetPaymentOperation_ExistingId_ReturnsPaymentOperationDto()
        {
            // Arrange

            var id = 1;

            var paymentOperation = new PaymentOperation(
                1000,
                DateTime.Now,
                OperationType.Payment,
                OperationStatus.Completed,
                1,
                2,
                PaymentMethod.Cash);

            var expectedDto = new PaymentOperationDto
            {
                Id = id,
                Amount = paymentOperation.Amount,
                Date = paymentOperation.Date,
                OperationType = paymentOperation.OperationType,
                OperationStatus = paymentOperation.Status,
                PatientId = paymentOperation.PatientId,
                AccountantId = paymentOperation.AccountantId,
                PaymentMethod = paymentOperation.PaymentMethod
            };

            _paymentOperationRepositoryMock
                .Setup(r => r.GetPaymentOperationAsync(id))
                .ReturnsAsync(paymentOperation);

            _mapperMock
                .Setup(m => m.Map<PaymentOperationDto>(
                    paymentOperation))
                .Returns(expectedDto);

            // Act

            var result =
                await _paymentOperationService.GetPaymentOperationAsync(id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetPaymentOperationAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PaymentOperationDto>(
                        paymentOperation),
                    Times.Once);
        }

        [Fact]
        public async Task GetPaymentOperation_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _paymentOperationRepositoryMock
                .Setup(r => r.GetPaymentOperationAsync(id))
                .ReturnsAsync((PaymentOperation?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _paymentOperationService.GetPaymentOperationAsync(id));

            Assert.Equal(
                $"Payment operation with ID {id} not found.",
                exception.Message);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetPaymentOperationAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<PaymentOperationDto>(
                        It.IsAny<PaymentOperation>()),
                    Times.Never);
        }

        // Get Payment Operations By Patient ID

        [Fact]
        public async Task GetPaymentOperationsByPatientId_HappyPath_ReturnsListOfPaymentOperationDto()
        {
            // Arrange

            var patientId = 1;

            var paymentOperations = new List<PaymentOperation>
            {
                new PaymentOperation(
                    1000,
                    DateTime.Now,
                    OperationType.Payment,
                    OperationStatus.Completed,
                    patientId,
                    2,
                    PaymentMethod.Cash),

                new PaymentOperation(
                    1500,
                    DateTime.Now.AddDays(1),
                    OperationType.Payment,
                    OperationStatus.Completed,
                    patientId,
                    3,
                    PaymentMethod.Card)
            };

            var expectedDtos = new List<PaymentOperationDto>
            {
                new PaymentOperationDto
                {
                    Id = 1,
                    Amount = paymentOperations[0].Amount,
                    Date = paymentOperations[0].Date,
                    OperationType = paymentOperations[0].OperationType,
                    OperationStatus = paymentOperations[0].Status,
                    PatientId = paymentOperations[0].PatientId,
                    AccountantId = paymentOperations[0].AccountantId,
                    PaymentMethod = paymentOperations[0].PaymentMethod
                },

                new PaymentOperationDto
                {
                    Id = 2,
                    Amount = paymentOperations[1].Amount,
                    Date = paymentOperations[1].Date,
                    OperationType = paymentOperations[1].OperationType,
                    OperationStatus = paymentOperations[1].Status,
                    PatientId = paymentOperations[1].PatientId,
                    AccountantId = paymentOperations[1].AccountantId,
                    PaymentMethod = paymentOperations[1].PaymentMethod
                }
            };

            _paymentOperationRepositoryMock
                .Setup(r => r.GetPaymentOperationsByPatientIdAsync(patientId))
                .ReturnsAsync(paymentOperations);

            _mapperMock
                .Setup(m => m.Map<List<PaymentOperationDto>>(
                    paymentOperations))
                .Returns(expectedDtos);

            // Act

            var result =
                await _paymentOperationService
                    .GetPaymentOperationsByPatientIdAsync(patientId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetPaymentOperationsByPatientIdAsync(patientId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PaymentOperationDto>>(
                        paymentOperations),
                    Times.Once);
        }

        // Get Payment Operations By Accountant ID

        [Fact]
        public async Task GetPaymentOperationsByAccountantId_HappyPath_ReturnsListOfPaymentOperationDto()
        {
            // Arrange

            var accountantId = 2;

            var paymentOperations = new List<PaymentOperation>
            {
                new PaymentOperation(
                    1000,
                    DateTime.Now,
                    OperationType.Payment,
                    OperationStatus.Completed,
                    1,
                    accountantId,
                    PaymentMethod.Cash),

                new PaymentOperation(
                    2000,
                    DateTime.Now.AddDays(1),
                    OperationType.Refund,
                    OperationStatus.Completed,
                    2,
                    accountantId,
                    PaymentMethod.Card)
            };

            var expectedDtos = new List<PaymentOperationDto>
            {
                new PaymentOperationDto
                {
                    Id = 1,
                    Amount = paymentOperations[0].Amount,
                    Date = paymentOperations[0].Date,
                    OperationType = paymentOperations[0].OperationType,
                    OperationStatus = paymentOperations[0].Status,
                    PatientId = paymentOperations[0].PatientId,
                    AccountantId = paymentOperations[0].AccountantId,
                    PaymentMethod = paymentOperations[0].PaymentMethod
                },

                new PaymentOperationDto
                {
                    Id = 2,
                    Amount = paymentOperations[1].Amount,
                    Date = paymentOperations[1].Date,
                    OperationType = paymentOperations[1].OperationType,
                    OperationStatus = paymentOperations[1].Status,
                    PatientId = paymentOperations[1].PatientId,
                    AccountantId = paymentOperations[1].AccountantId,
                    PaymentMethod = paymentOperations[1].PaymentMethod
                }
            };

            _paymentOperationRepositoryMock
                .Setup(r => r.GetPaymentOperationsByAccountantIdAsync(accountantId))
                .ReturnsAsync(paymentOperations);

            _mapperMock
                .Setup(m => m.Map<List<PaymentOperationDto>>(
                    paymentOperations))
                .Returns(expectedDtos);

            // Act

            var result =
                await _paymentOperationService
                    .GetPaymentOperationsByAccountantIdAsync(accountantId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _paymentOperationRepositoryMock
                .Verify(
                    r => r.GetPaymentOperationsByAccountantIdAsync(accountantId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<PaymentOperationDto>>(
                        paymentOperations),
                    Times.Once);
        }
    }
}
