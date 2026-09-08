using AutoMapper;
using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class AppointmentSeviceTesting
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;

        private readonly AppointmentService _appointmentService;

        public AppointmentSeviceTesting()
        {
            _mapperMock = new Mock<IMapper>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);
        }


        // Create Appointment

        [Fact]
        public async Task CreateAppointment_HappyPath_ReturnsCreatedAppointment()
        {
            // Arrange

            var appointmentDto = new CreateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1).AddHours(1),
                PatientId = 1,
                NurseId = 2,
                DoctorId = 3
            };

            var appointment = new Appointment(
                appointmentDto.StartDate,
                appointmentDto.EndDate,
                appointmentDto.PatientId,
                appointmentDto.NurseId,
                appointmentDto.DoctorId);

            var expectedDto = new AppointmentDto
            {
                StartDate = appointmentDto.StartDate,
                EndDate = appointmentDto.EndDate,
                PatientId = appointmentDto.PatientId,
                NurseId = appointmentDto.NurseId,
                DoctorId = appointmentDto.DoctorId
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentInTimeRange(
                    appointmentDto.StartDate,
                    appointmentDto.EndDate,
                    appointmentDto.DoctorId,
                    appointmentDto.NurseId))
                .ReturnsAsync((Appointment?)null);

            _appointmentRepositoryMock
                .Setup(a => a.CreateAppointmentAsync(
                    It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(m => m.Map<AppointmentDto>(
                    It.IsAny<Appointment>()))
                .Returns(expectedDto);


            // Act

            var result =
                await _appointmentService.CreateAppointmentAsync(
                    appointmentDto);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        appointmentDto.StartDate,
                        appointmentDto.EndDate,
                        appointmentDto.DoctorId,
                        appointmentDto.NurseId),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.CreateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Once);
        }


        [Fact]
        public async Task CreateAppointment_StartDateInPast_ThrowsException()
        {
            // Arrange

            var appointmentDto = new CreateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now,
                PatientId = 1,
                NurseId = 2,
                DoctorId = 3
            };


            // Act & Assert

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _appointmentService.CreateAppointmentAsync(
                    appointmentDto));

            Assert.Equal(
                "Appointment cannot be created in the past",
                exception.Message);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()),
                    Times.Never);

            _appointmentRepositoryMock
                .Verify(
                    a => a.CreateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        [Fact]
        public async Task CreateAppointment_UnavailableTimeRange_ThrowsException()
        {
            // Arrange

            var appointmentDto = new CreateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1).AddHours(1),
                PatientId = 1,
                NurseId = 2,
                DoctorId = 3
            };

            var existingAppointment = new Appointment(
                appointmentDto.StartDate,
                appointmentDto.EndDate,
                appointmentDto.PatientId,
                appointmentDto.NurseId,
                appointmentDto.DoctorId);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentInTimeRange(
                    appointmentDto.StartDate,
                    appointmentDto.EndDate,
                    appointmentDto.DoctorId,
                    appointmentDto.NurseId))
                .ReturnsAsync(existingAppointment);


            // Act & Assert

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _appointmentService.CreateAppointmentAsync(
                    appointmentDto));

            Assert.Equal(
                "Doctor or Nurse is not available in the given time range",
                exception.Message);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        appointmentDto.StartDate,
                        appointmentDto.EndDate,
                        appointmentDto.DoctorId,
                        appointmentDto.NurseId),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.CreateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        // Get All Appointments

        [Fact]
        public async Task GetAllAppointments_HappyPath_ReturnsListOfAppointmentDto()
        {
            // Arrange

            var appointments = new List<Appointment>
            {
                new Appointment(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(1),
                    1, 2, 3),

                new Appointment(
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(2).AddHours(1),
                    4, 5, 6)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    StartDate = appointments[0].StartDate,
                    EndDate = appointments[0].EndDate,
                    PatientId = appointments[0].PatientId,
                    NurseId = appointments[0].NurseId,
                    DoctorId = appointments[0].DoctorId
                },

                new AppointmentDto
                {
                    StartDate = appointments[1].StartDate,
                    EndDate = appointments[1].EndDate,
                    PatientId = appointments[1].PatientId,
                    NurseId = appointments[1].NurseId,
                    DoctorId = appointments[1].DoctorId
                }
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAllAppointmentsAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAllAppointmentsAsync();


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAllAppointmentsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        // Get Appointment By ID

        [Fact]
        public async Task GetAppointmentById_ExistingId_ReturnsAppointmentDto()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            var expectedDto = new AppointmentDto
            {
                Id = id,
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                PatientId = appointment.PatientId,
                NurseId = appointment.NurseId,
                DoctorId = appointment.DoctorId
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(expectedDto);


            // Act

            var result =
                await _appointmentService.GetAppointmentAsync(id);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(appointment),
                    Times.Once);
        }


        [Fact]
        public async Task GetAppointmentById_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 1;

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync((Appointment?)null);


            // Act & Assert

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _appointmentService.GetAppointmentAsync(id));

            Assert.Equal(
                $"Appointment with ID {id} not found.",
                exception.Message);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        // Get Appointments By Doctor ID

        [Fact]
        public async Task GetAppointmentByDoctorId_ExistingId_ReturnsAppointments()
        {
            // Arrange

            var doctorId = 3;

            var appointments = new List<Appointment>
            {
                new Appointment(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(1),
                    1, 2, doctorId),

                new Appointment(
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(2).AddHours(1),
                    4, 5, doctorId)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    StartDate = appointments[0].StartDate,
                    EndDate = appointments[0].EndDate,
                    PatientId = appointments[0].PatientId,
                    NurseId = appointments[0].NurseId,
                    DoctorId = appointments[0].DoctorId
                },

                new AppointmentDto
                {
                    StartDate = appointments[1].StartDate,
                    EndDate = appointments[1].EndDate,
                    PatientId = appointments[1].PatientId,
                    NurseId = appointments[1].NurseId,
                    DoctorId = appointments[1].DoctorId
                }
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsByDoctorIdAsync(doctorId))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsByDoctorIdAsync(
                    doctorId);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsByDoctorIdAsync(doctorId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        // Get Appointments By Nurse ID

        [Fact]
        public async Task GetAppointmentByNurseId_ExistingId_ReturnsAppointments()
        {
            // Arrange

            var nurseId = 2;

            var appointments = new List<Appointment>
            {
                new Appointment(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(1),
                    1, nurseId, 3),

                new Appointment(
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(2).AddHours(1),
                    4, nurseId, 3)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    StartDate = appointments[0].StartDate,
                    EndDate = appointments[0].EndDate,
                    PatientId = appointments[0].PatientId,
                    NurseId = appointments[0].NurseId,
                    DoctorId = appointments[0].DoctorId
                },

                new AppointmentDto
                {
                    StartDate = appointments[1].StartDate,
                    EndDate = appointments[1].EndDate,
                    PatientId = appointments[1].PatientId,
                    NurseId = appointments[1].NurseId,
                    DoctorId = appointments[1].DoctorId
                }
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsByNurseIdAsync(nurseId))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsByNurseIdAsync(
                    nurseId);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsByNurseIdAsync(nurseId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        [Fact]
        public async Task GetAppointmentByNurseId_NoAppointments_ReturnsEmptyList()
        {
            // Arrange

            var nurseId = 999;

            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentDto>();

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsByNurseIdAsync(nurseId))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsByNurseIdAsync(
                    nurseId);


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsByNurseIdAsync(nurseId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        // Get Appointments By Status

        [Fact]
        public async Task GetAppointmentsByStatus_ExistingStatus_ReturnsAppointments()
        {
            // Arrange

            var status = AppointmentStatus.Pending;

            var appointments = new List<Appointment>
            {
                new Appointment(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(1),
                    1, 2, 3),

                new Appointment(
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(2).AddHours(1),
                    4, 5, 3)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    StartDate = appointments[0].StartDate,
                    EndDate = appointments[0].EndDate,
                    PatientId = appointments[0].PatientId,
                    NurseId = appointments[0].NurseId,
                    DoctorId = appointments[0].DoctorId
                },

                new AppointmentDto
                {
                    StartDate = appointments[1].StartDate,
                    EndDate = appointments[1].EndDate,
                    PatientId = appointments[1].PatientId,
                    NurseId = appointments[1].NurseId,
                    DoctorId = appointments[1].DoctorId
                }
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsByStatusAsync(status))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsByStatusAsync(
                    status);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsByStatusAsync(status),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        [Fact]
        public async Task GetAppointmentsByStatus_NoAppointments_ReturnsEmptyList()
        {
            // Arrange

            var status = AppointmentStatus.Pending;

            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentDto>();

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsByStatusAsync(status))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsByStatusAsync(
                    status);


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsByStatusAsync(status),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        // Get Appointments For Current User

        [Fact]
        public async Task GetAppointmentsForUserByTokens_ValidToken_ReturnsAppointments()
        {
            // Arrange

            var token = "valid-token";
            var userId = 5;

            var appointments = new List<Appointment>
            {
                new Appointment(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(1),
                    1, 2, 3),

                new Appointment(
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(2).AddHours(1),
                    4, 5, 3)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    StartDate = appointments[0].StartDate,
                    EndDate = appointments[0].EndDate,
                    PatientId = appointments[0].PatientId,
                    NurseId = appointments[0].NurseId,
                    DoctorId = appointments[0].DoctorId
                },

                new AppointmentDto
                {
                    StartDate = appointments[1].StartDate,
                    EndDate = appointments[1].EndDate,
                    PatientId = appointments[1].PatientId,
                    NurseId = appointments[1].NurseId,
                    DoctorId = appointments[1].DoctorId
                }
            };

            _userServiceMock
                .Setup(u => u.GetIdFromTokensAsync(token))
                .ReturnsAsync(userId);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsForUser(userId))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsForUserByTokens(
                    token);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _userServiceMock
                .Verify(
                    u => u.GetIdFromTokensAsync(token),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsForUser(userId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        [Fact]
        public async Task GetAppointmentsForUserByTokens_NoAppointments_ReturnsEmptyList()
        {
            // Arrange

            var token = "valid-token";
            var userId = 5;

            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentDto>();

            _userServiceMock
                .Setup(u => u.GetIdFromTokensAsync(token))
                .ReturnsAsync(userId);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentsForUser(userId))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);


            // Act

            var result =
                await _appointmentService.GetAppointmentsForUserByTokens(
                    token);


            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _userServiceMock
                .Verify(
                    u => u.GetIdFromTokensAsync(token),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentsForUser(userId),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<List<AppointmentDto>>(appointments),
                    Times.Once);
        }


        // Update Appointment

        [Fact]
        public async Task UpdateAppointment_HappyPath_ReturnsAppointmentDto()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            var updateDto = new UpdateAppointmentDto
            {
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                PatientId = appointment.PatientId,
                NurseId = appointment.NurseId,
                DoctorId = appointment.DoctorId
            };

            var expectedDto = new AppointmentDto
            {
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                PatientId = appointment.PatientId,
                NurseId = appointment.NurseId,
                DoctorId = appointment.DoctorId
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(a => a.UpdateAppointmentAsync(appointment))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(expectedDto);


            // Act

            var result =
                await _appointmentService.UpdateAppointmentAsync(
                    updateDto,
                    id);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.UpdateAppointmentAsync(appointment),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(appointment),
                    Times.Once);
        }


        [Fact]
        public async Task UpdateAppointment_AppointmentNotFound_ThrowsException()
        {
            // Arrange

            var id = 1;

            var updateDto = new UpdateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1).AddHours(1),
                PatientId = 1,
                NurseId = 2,
                DoctorId = 3
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync((Appointment?)null);


            // Act & Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _appointmentService.UpdateAppointmentAsync(
                    updateDto,
                    id));



            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.UpdateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        // Update Appointment - Past Date

        [Fact]
        public async Task UpdateAppointment_ChangedDataWithPastStartDate_ThrowsException()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            var updateDto = new UpdateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(-1).AddHours(1),
                PatientId = 1,
                NurseId = 2,
                DoctorId = 3
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);


            // Act & Assert

            await Assert.ThrowsAsync<ArgumentException>(
                () => _appointmentService.UpdateAppointmentAsync(
                    updateDto,
                    id));

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()),
                    Times.Never);

            _appointmentRepositoryMock
                .Verify(
                    a => a.UpdateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        // Update Appointment - Doctor Or Nurse Unavailable

        [Fact]
        public async Task UpdateAppointment_DoctorOrNurseUnavailable_ThrowsException()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            var updateDto = new UpdateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(2).AddHours(1),
                PatientId = 1,
                NurseId = 5,
                DoctorId = 6
            };

            var existingAppointment = new Appointment(
                updateDto.StartDate,
                updateDto.EndDate,
                10, 5, 6);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentInTimeRange(
                    updateDto.StartDate,
                    updateDto.EndDate,
                    updateDto.DoctorId,
                    updateDto.NurseId))
                .ReturnsAsync(existingAppointment);


            // Act & Assert

            await Assert.ThrowsAsync<ArgumentException>(
                () => _appointmentService.UpdateAppointmentAsync(
                    updateDto,
                    id));

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        updateDto.StartDate,
                        updateDto.EndDate,
                        updateDto.DoctorId,
                        updateDto.NurseId),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.UpdateAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }


        // Update Appointment - Data Changed And Available

        [Fact]
        public async Task UpdateAppointment_ChangedDataAndAvailable_ReturnsAppointmentDto()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            var updateDto = new UpdateAppointmentDto
            {
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(2).AddHours(1),
                PatientId = 1,
                NurseId = 5,
                DoctorId = 6
            };

            var expectedDto = new AppointmentDto
            {
                StartDate = updateDto.StartDate,
                EndDate = updateDto.EndDate,
                PatientId = updateDto.PatientId,
                NurseId = updateDto.NurseId,
                DoctorId = updateDto.DoctorId
            };

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentInTimeRange(
                    updateDto.StartDate,
                    updateDto.EndDate,
                    updateDto.DoctorId,
                    updateDto.NurseId))
                .ReturnsAsync((Appointment?)null);

            _appointmentRepositoryMock
                .Setup(a => a.UpdateAppointmentAsync(appointment))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(expectedDto);


            // Act

            var result =
                await _appointmentService.UpdateAppointmentAsync(
                    updateDto,
                    id);


            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentInTimeRange(
                        updateDto.StartDate,
                        updateDto.EndDate,
                        updateDto.DoctorId,
                        updateDto.NurseId),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.UpdateAppointmentAsync(appointment),
                    Times.Once);

            _mapperMock
                .Verify(
                    m => m.Map<AppointmentDto>(appointment),
                    Times.Once);
        }


        // Delete Appointment

        [Fact]
        public async Task DeleteAppointment_ExistingId_DeletesAppointment()
        {
            // Arrange

            var id = 1;

            var appointment = new Appointment(
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1).AddHours(1),
                1, 2, 3);

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(a => a.DeleteAppointmentAsync(appointment))
                .Returns(Task.CompletedTask);


            // Act

            await _appointmentService.DeleteAppointmentAsync(id);


            // Assert

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.DeleteAppointmentAsync(appointment),
                    Times.Once);
        }


        [Fact]
        public async Task DeleteAppointment_AppointmentNotFound_ThrowsException()
        {
            // Arrange

            var id = 1;

            _appointmentRepositoryMock
                .Setup(a => a.GetAppointmentAsync(id))
                .ReturnsAsync((Appointment?)null);


            // Act & Assert

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _appointmentService.DeleteAppointmentAsync(id));

            _appointmentRepositoryMock
                .Verify(
                    a => a.GetAppointmentAsync(id),
                    Times.Once);

            _appointmentRepositoryMock
                .Verify(
                    a => a.DeleteAppointmentAsync(
                        It.IsAny<Appointment>()),
                    Times.Never);
        }
    }
}
