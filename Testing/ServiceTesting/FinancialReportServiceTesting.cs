using AutoMapper;
using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class FinancialReportServiceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFinancialReportRepository> _financialReportRepositoryMock;
        private readonly Mock<ILogger<FinancialReportService>> _loggerMock;

        private readonly FinancialReportService _financialReportService;

        public FinancialReportServiceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _financialReportRepositoryMock = new Mock<IFinancialReportRepository>();
            _loggerMock = new Mock<ILogger<FinancialReportService>>();

            _financialReportService = new FinancialReportService(
                _financialReportRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }


        // Create Financial Report

        [Fact]
        public async Task CreateFinancialReport_HappyPath_ReturnsCreatedFinancialReport()
        {
            // Arrange

            var createFinancialReportDto = new CreateFinancialReportDto
            {
                MonthlyExpenses = 10000,
                NetProfit = 5000,
                MonthlyRevenue = 15000,
                Date = DateTime.Now
            };

            var financialReport = new FinancialReport(
                createFinancialReportDto.MonthlyExpenses,
                createFinancialReportDto.NetProfit,
                createFinancialReportDto.MonthlyRevenue,
                createFinancialReportDto.Date);

            var expectedDto = new FinancialReportDto
            {
                Id = 1,
                MonthlyExpenses = createFinancialReportDto.MonthlyExpenses,
                NetProfit = createFinancialReportDto.NetProfit,
                MonthlyRevenue = createFinancialReportDto.MonthlyRevenue,
                Date = createFinancialReportDto.Date
            };

            _financialReportRepositoryMock
                .Setup(r => r.CreateFinancialReportAsync(
                    It.IsAny<FinancialReport>()))
                .ReturnsAsync(financialReport);

            _mapperMock
                .Setup(m => m.Map<FinancialReportDto>(
                    It.IsAny<FinancialReport>()))
                .Returns(expectedDto);


            // Act

            var result =
                await _financialReportService.CreateFinancialReportAsync(
                    createFinancialReportDto);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _financialReportRepositoryMock
                .Verify(
                    r => r.CreateFinancialReportAsync(
                        It.IsAny<FinancialReport>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<FinancialReportDto>(
                        It.IsAny<FinancialReport>()),
                    Times.Once);
        }


        // Get All Financial Reports

        [Fact]
        public async Task GetAllFinancialReports_HappyPath_ReturnsListOfFinancialReportDto()
        {
            // Arrange

            var financialReports = new List<FinancialReport>
            {
                new FinancialReport(
                    10000,
                    5000,
                    15000,
                    DateTime.Now),

                new FinancialReport(
                    12000,
                    6000,
                    18000,
                    DateTime.Now.AddMonths(1))
            };

            var expectedDtos = new List<FinancialReportDto>
            {
                new FinancialReportDto
                {
                    Id = 1,
                    MonthlyExpenses = financialReports[0].MonthlyExpenses,
                    NetProfit = financialReports[0].NetProfit,
                    MonthlyRevenue = financialReports[0].MonthlyRevenue,
                    Date = financialReports[0].Date
                },

                new FinancialReportDto
                {
                    Id = 2,
                    MonthlyExpenses = financialReports[1].MonthlyExpenses,
                    NetProfit = financialReports[1].NetProfit,
                    MonthlyRevenue = financialReports[1].MonthlyRevenue,
                    Date = financialReports[1].Date
                }
            };

            _financialReportRepositoryMock
                .Setup(r => r.GetAllFinancialReportsAsync())
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetAllFinancialReportsAsync();


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetAllFinancialReportsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                        financialReports),
                    Times.Once);
        }


        [Fact]
        public async Task GetAllFinancialReports_NoReports_ReturnsEmptyList()
        {
            // Arrange

            var financialReports = new List<FinancialReport>();
            var expectedDtos = new List<FinancialReportDto>();

            _financialReportRepositoryMock
                .Setup(r => r.GetAllFinancialReportsAsync())
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetAllFinancialReportsAsync();


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetAllFinancialReportsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                        financialReports),
                    Times.Once);
        }


        // Get Financial Report By ID

        [Fact]
        public async Task GetFinancialReport_ExistingId_ReturnsFinancialReportDto()
        {
            // Arrange

            var id = 1;

            var financialReport = new FinancialReport(
                10000,
                5000,
                15000,
                DateTime.Now);

            var expectedDto = new FinancialReportDto
            {
                Id = id,
                MonthlyExpenses = financialReport.MonthlyExpenses,
                NetProfit = financialReport.NetProfit,
                MonthlyRevenue = financialReport.MonthlyRevenue,
                Date = financialReport.Date
            };

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportAsync(id))
                .ReturnsAsync(financialReport);

            _mapperMock
                .Setup(m => m.Map<FinancialReportDto>(
                    financialReport))
                .Returns(expectedDto);


            // Act

            var result =
                await _financialReportService.GetFinancialReportAsync(id);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<FinancialReportDto>(
                        financialReport),
                    Times.Once);
        }


        [Fact]
        public async Task GetFinancialReport_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportAsync(id))
                .ReturnsAsync((FinancialReport?)null);


            // Act & Assert

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _financialReportService.GetFinancialReportAsync(id));

            Assert.Equal(
                $"Financial report with ID {id} not found.",
                exception.Message);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<FinancialReportDto>(
                        It.IsAny<FinancialReport>()),
                    Times.Never);
        }


        // Get Financial Reports By Range

        [Fact]
        public async Task GetFinancialReportsByRange_HappyPath_ReturnsListOfFinancialReportDto()
        {
            // Arrange

            decimal min = 5000;
            decimal max = 20000;
            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = DateTime.Now;

            var financialReports = new List<FinancialReport>
            {
                new FinancialReport(
                    10000,
                    5000,
                    15000,
                    startDate),

                new FinancialReport(
                    12000,
                    6000,
                    18000,
                    endDate)
            };

            var expectedDtos = new List<FinancialReportDto>
            {
                new FinancialReportDto
                {
                    Id = 1,
                    MonthlyExpenses = financialReports[0].MonthlyExpenses,
                    NetProfit = financialReports[0].NetProfit,
                    MonthlyRevenue = financialReports[0].MonthlyRevenue,
                    Date = financialReports[0].Date
                },

                new FinancialReportDto
                {
                    Id = 2,
                    MonthlyExpenses = financialReports[1].MonthlyExpenses,
                    NetProfit = financialReports[1].NetProfit,
                    MonthlyRevenue = financialReports[1].MonthlyRevenue,
                    Date = financialReports[1].Date
                }
            };

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportsByRangeAsync(
                    min,
                    max,
                    startDate,
                    endDate))
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetFinancialReportsByRangeAsync(
                    min,
                    max,
                    startDate,
                    endDate);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportsByRangeAsync(
                        min,
                        max,
                        startDate,
                        endDate),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                        financialReports),
                    Times.Once);
        }


        [Fact]
        public async Task GetFinancialReportsByRange_NoReports_ReturnsEmptyList()
        {
            // Arrange

            decimal min = 5000;
            decimal max = 20000;
            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = DateTime.Now;

            var financialReports = new List<FinancialReport>();
            var expectedDtos = new List<FinancialReportDto>();

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportsByRangeAsync(
                    min,
                    max,
                    startDate,
                    endDate))
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetFinancialReportsByRangeAsync(
                    min,
                    max,
                    startDate,
                    endDate);


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportsByRangeAsync(
                        min,
                        max,
                        startDate,
                        endDate),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                        financialReports),
                    Times.Once);
        }


        // Get Financial Reports By Date Range

        [Fact]
        public async Task GetFinancialReportsByDateRange_HappyPath_ReturnsListOfFinancialReportDto()
        {
            // Arrange

            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = DateTime.Now;

            var financialReports = new List<FinancialReport>
            {
                new FinancialReport(
                    10000,
                    5000,
                    15000,
                    startDate),

                new FinancialReport(
                    12000,
                    6000,
                    18000,
                    endDate)
            };

            var expectedDtos = new List<FinancialReportDto>
            {
                new FinancialReportDto
                {
                    Id = 1,
                    MonthlyExpenses = financialReports[0].MonthlyExpenses,
                    NetProfit = financialReports[0].NetProfit,
                    MonthlyRevenue = financialReports[0].MonthlyRevenue,
                    Date = financialReports[0].Date
                },

                new FinancialReportDto
                {
                    Id = 2,
                    MonthlyExpenses = financialReports[1].MonthlyExpenses,
                    NetProfit = financialReports[1].NetProfit,
                    MonthlyRevenue = financialReports[1].MonthlyRevenue,
                    Date = financialReports[1].Date
                }
            };

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportsByDateRangeAsync(
                    startDate,
                    endDate))
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetFinancialReportsByDateRangeAsync(
                    startDate,
                    endDate);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportsByDateRangeAsync(
                        startDate,
                        endDate),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                        financialReports),
                    Times.Once);
        }


        [Fact]
        public async Task GetFinancialReportsByDateRange_NoReports_ReturnsEmptyList()
        {
            // Arrange

            var startDate = DateTime.Now.AddMonths(-2);
            var endDate = DateTime.Now;

            var financialReports = new List<FinancialReport>();
            var expectedDtos = new List<FinancialReportDto>();

            _financialReportRepositoryMock
                .Setup(r => r.GetFinancialReportsByDateRangeAsync(
                    startDate,
                    endDate))
                .ReturnsAsync(financialReports);

            _mapperMock
                .Setup(m => m.Map<List<FinancialReportDto>>(
                    financialReports))
                .Returns(expectedDtos);


            // Act

            var result =
                await _financialReportService.GetFinancialReportsByDateRangeAsync(
                    startDate,
                    endDate);


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _financialReportRepositoryMock
                .Verify(
                    r => r.GetFinancialReportsByDateRangeAsync(
                        startDate,
                        endDate),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<FinancialReportDto>>(
                    financialReports),
                    Times.Once);
        }
    }
}
