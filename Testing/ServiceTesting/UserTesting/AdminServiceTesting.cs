using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.AdminDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Services.UserServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.ServiceTesting.UnitTesting
{
    public class AdminServiceTesting
    {
        private readonly Mock<IAdminRepository> _adminRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AdminService>> _loggerMock;

        private readonly AdminService _adminService;

        public AdminServiceTesting()
        {
            _adminRepositoryMock = new Mock<IAdminRepository>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AdminService>>();

            _adminService = new AdminService(
                _adminRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object);
        }

        // Create Admin

        [Fact]
        public async Task CreateAdmin_HappyPath_ReturnsAdminDto()
        {
            // Arrange

            var createAdminDto = new CreateAdminDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "01012345678",
                Password = "Password123!"
            };

            var admin = new Admin(
                createAdminDto.FirstName,
                createAdminDto.LastName,
                createAdminDto.Email,
                createAdminDto.UserName,
                createAdminDto.PhoneNumber);

            var createdAdminResult = (
                admin: admin,
                addUserRes: true,
                addPasswordRes: true,
                addRoleRes: true
            );

            var expectedDto = new AdminDto
            {
                Id = 1,
                FirstName = createAdminDto.FirstName,
                LastName = createAdminDto.LastName,
                Email = createAdminDto.Email,
                UserName = createAdminDto.UserName,
                PhoneNumber = createAdminDto.PhoneNumber
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAdminDto.Email,
                    createAdminDto.UserName,
                    createAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.CreateAdminAsync(
                    It.IsAny<Admin>(),
                    createAdminDto.Password))
                .ReturnsAsync(createdAdminResult);

            _mapperMock
                .Setup(x => x.Map<AdminDto>(
                    admin))
                .Returns(expectedDto);

            // Act

            var result =
                await _adminService.CreateAdminAsync(createAdminDto);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _userServiceMock
                .Verify(
                    x => x.ValidateUserCreation(
                        createAdminDto.Email,
                        createAdminDto.UserName,
                        createAdminDto.PhoneNumber),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.CreateAdminAsync(
                        It.IsAny<Admin>(),
                        createAdminDto.Password),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(admin),
                    Times.Once);

            _userServiceMock
                .Verify(
                    x => x.DeleteUserAsync(
                        It.IsAny<int>()),
                    Times.Never);
        }

        [Fact]
        public async Task CreateAdmin_UserCreationFails_ThrowsException()
        {
            // Arrange

            var createAdminDto = new CreateAdminDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "01012345678",
                Password = "Password123!"
            };

            var admin = new Admin(
                createAdminDto.FirstName,
                createAdminDto.LastName,
                createAdminDto.Email,
                createAdminDto.UserName,
                createAdminDto.PhoneNumber);

            var createdAdminResult = (
                admin: admin,
                addUserRes: false,
                addPasswordRes: false,
                addRoleRes: false
            );

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAdminDto.Email,
                    createAdminDto.UserName,
                    createAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.CreateAdminAsync(
                    It.IsAny<Admin>(),
                    createAdminDto.Password))
                .ReturnsAsync(createdAdminResult);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _adminService.CreateAdminAsync(createAdminDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock
                .Verify(
                    x => x.ValidateUserCreation(
                        createAdminDto.Email,
                        createAdminDto.UserName,
                        createAdminDto.PhoneNumber),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.CreateAdminAsync(
                        It.IsAny<Admin>(),
                        createAdminDto.Password),
                    Times.Once);

            _userServiceMock
                .Verify(
                    x => x.DeleteUserAsync(
                        It.IsAny<int>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        It.IsAny<Admin>()),
                    Times.Never);
        }

        [Fact]
        public async Task CreateAdmin_PasswordCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange

            var createAdminDto = new CreateAdminDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "01012345678",
                Password = "Password123!"
            };

            var admin = new Admin(
                createAdminDto.FirstName,
                createAdminDto.LastName,
                createAdminDto.Email,
                createAdminDto.UserName,
                createAdminDto.PhoneNumber);

            var createdAdminResult = (
                admin: admin,
                addUserRes: true,
                addPasswordRes: false,
                addRoleRes: false
            );

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAdminDto.Email,
                    createAdminDto.UserName,
                    createAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.CreateAdminAsync(
                    It.IsAny<Admin>(),
                    createAdminDto.Password))
                .ReturnsAsync(createdAdminResult);

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(admin.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _adminService.CreateAdminAsync(createAdminDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _userServiceMock
                .Verify(
                    x => x.ValidateUserCreation(
                        createAdminDto.Email,
                        createAdminDto.UserName,
                        createAdminDto.PhoneNumber),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.CreateAdminAsync(
                        It.IsAny<Admin>(),
                        createAdminDto.Password),
                    Times.Once);

            _userServiceMock
                .Verify(
                    x => x.DeleteUserAsync(admin.Id),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        It.IsAny<Admin>()),
                    Times.Never);
        }

        [Fact]
        public async Task CreateAdmin_RoleCreationFails_DeletesUserAndThrowsException()
        {
            // Arrange

            var createAdminDto = new CreateAdminDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "01012345678",
                Password = "Password123!"
            };

            var admin = new Admin(
                createAdminDto.FirstName,
                createAdminDto.LastName,
                createAdminDto.Email,
                createAdminDto.UserName,
                createAdminDto.PhoneNumber);

            var createdAdminResult = (
                admin: admin,
                addUserRes: true,
                addPasswordRes: true,
                addRoleRes: false
            );

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    createAdminDto.Email,
                    createAdminDto.UserName,
                    createAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.CreateAdminAsync(
                    It.IsAny<Admin>(),
                    createAdminDto.Password))
                .ReturnsAsync(createdAdminResult);

            _userServiceMock
                .Setup(x => x.DeleteUserAsync(admin.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<Exception>(
                    () => _adminService.CreateAdminAsync(createAdminDto));

            Assert.Equal(
                "Cannot create user, try again",
                exception.Message);

            _adminRepositoryMock
                .Verify(
                    x => x.CreateAdminAsync(
                        It.IsAny<Admin>(),
                        createAdminDto.Password),
                    Times.Once);

            _userServiceMock
                .Verify(
                    x => x.DeleteUserAsync(admin.Id),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        It.IsAny<Admin>()),
                    Times.Never);
        }

        // Get All Admins

        [Fact]
        public async Task GetAllAdmins_HappyPath_ReturnsListOfAdminDto()
        {
            // Arrange

            var admins = new List<Admin>
            {
                new Admin(
                    "John",
                    "Doe",
                    "john@example.com",
                    "johndoe",
                    "01012345678"),

                new Admin(
                    "Jane",
                    "Smith",
                    "jane@example.com",
                    "janesmith",
                    "01112345678")
            };

            var expectedDtos = new List<AdminDto>
            {
                new AdminDto
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    UserName = "johndoe",
                    PhoneNumber = "01012345678"
                },

                new AdminDto
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    UserName = "janesmith",
                    PhoneNumber = "01112345678"
                }
            };

            _adminRepositoryMock
                .Setup(x => x.GetAllAdminsAsync())
                .ReturnsAsync(admins);

            _mapperMock
                .Setup(x => x.Map<List<AdminDto>>(
                    admins))
                .Returns(expectedDtos);

            // Act

            var result =
                await _adminService.GetAllAdminsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count);
            Assert.Equal(expectedDtos[0], result[0]);
            Assert.Equal(expectedDtos[1], result[1]);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAllAdminsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<List<AdminDto>>(
                        admins),
                    Times.Once);
        }

        [Fact]
        public async Task GetAllAdmins_NoAdmins_ReturnsEmptyList()
        {
            // Arrange

            var admins = new List<Admin>();
            var expectedDtos = new List<AdminDto>();

            _adminRepositoryMock
                .Setup(x => x.GetAllAdminsAsync())
                .ReturnsAsync(admins);

            _mapperMock
                .Setup(x => x.Map<List<AdminDto>>(
                    admins))
                .Returns(expectedDtos);

            // Act

            var result =
                await _adminService.GetAllAdminsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAllAdminsAsync(),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<List<AdminDto>>(
                        admins),
                    Times.Once);
        }

        // Get Admin By ID

        [Fact]
        public async Task GetAdmin_ExistingId_ReturnsAdminDto()
        {
            // Arrange

            var id = 1;

            var admin = new Admin(
                "John",
                "Doe",
                "john@example.com",
                "johndoe",
                "01012345678");

            var expectedDto = new AdminDto
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                PhoneNumber = "01012345678"
            };

            _adminRepositoryMock
                .Setup(x => x.GetAdminAsync(id))
                .ReturnsAsync(admin);

            _mapperMock
                .Setup(x => x.Map<AdminDto>(admin))
                .Returns(expectedDto);

            // Act

            var result =
                await _adminService.GetAdminAsync(id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAdminAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(admin),
                    Times.Once);
        }

        [Fact]
        public async Task GetAdmin_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            _adminRepositoryMock
                .Setup(x => x.GetAdminAsync(id))
                .ReturnsAsync((Admin?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _adminService.GetAdminAsync(id));

            Assert.Equal(
                $"Cannot find user with ID{id}",
                exception.Message);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAdminAsync(id),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        It.IsAny<Admin>()),
                    Times.Never);
        }

        // Update Admin

        [Fact]
        public async Task UpdateAdmin_ExistingId_ReturnsUpdatedAdminDto()
        {
            // Arrange

            var id = 1;

            var updateAdminDto = new UpdateAdminDto
            {
                FirstName = "Updated",
                LastName = "Admin",
                Email = "updated@example.com",
                UserName = "updatedadmin",
                PhoneNumber = "01234567890"
            };

            var admin = new Admin(
                "John",
                "Doe",
                "john@example.com",
                "johndoe",
                "01012345678");

            var updatedAdmin = new Admin(
                updateAdminDto.FirstName,
                updateAdminDto.LastName,
                updateAdminDto.Email,
                updateAdminDto.UserName,
                updateAdminDto.PhoneNumber);

            var expectedDto = new AdminDto
            {
                Id = id,
                FirstName = updateAdminDto.FirstName,
                LastName = updateAdminDto.LastName,
                Email = updateAdminDto.Email,
                UserName = updateAdminDto.UserName,
                PhoneNumber = updateAdminDto.PhoneNumber
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateAdminDto.Email,
                    updateAdminDto.UserName,
                    updateAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.GetAdminAsync(id))
                .ReturnsAsync(admin);

            _adminRepositoryMock
                .Setup(x => x.UpdateAdminAsync(
                    It.IsAny<Admin>()))
                .ReturnsAsync(updatedAdmin);

            _mapperMock
                .Setup(x => x.Map<AdminDto>(
                    updatedAdmin))
                .Returns(expectedDto);

            // Act

            var result =
                await _adminService.UpdateAdminAsync(
                    updateAdminDto,
                    id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(expectedDto, result);

            _userServiceMock
                .Verify(
                    x => x.ValidateUserCreation(
                        updateAdminDto.Email,
                        updateAdminDto.UserName,
                        updateAdminDto.PhoneNumber),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAdminAsync(id),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.UpdateAdminAsync(
                        It.IsAny<Admin>()),
                    Times.Once);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        updatedAdmin),
                    Times.Once);
        }

        [Fact]
        public async Task UpdateAdmin_NonExistingId_ThrowsException()
        {
            // Arrange

            var id = 999;

            var updateAdminDto = new UpdateAdminDto
            {
                FirstName = "Updated",
                LastName = "Admin",
                Email = "updated@example.com",
                UserName = "updatedadmin",
                PhoneNumber = "01234567890"
            };

            _userServiceMock
                .Setup(x => x.ValidateUserCreation(
                    updateAdminDto.Email,
                    updateAdminDto.UserName,
                    updateAdminDto.PhoneNumber))
                .Returns(Task.CompletedTask);

            _adminRepositoryMock
                .Setup(x => x.GetAdminAsync(id))
                .ReturnsAsync((Admin?)null);

            // Act & Assert

            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _adminService.UpdateAdminAsync(
                        updateAdminDto,
                        id));

            Assert.Equal(
                $"Cannot found user with id {id}",
                exception.Message);

            _userServiceMock
                .Verify(
                    x => x.ValidateUserCreation(
                        updateAdminDto.Email,
                        updateAdminDto.UserName,
                        updateAdminDto.PhoneNumber),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.GetAdminAsync(id),
                    Times.Once);

            _adminRepositoryMock
                .Verify(
                    x => x.UpdateAdminAsync(
                        It.IsAny<Admin>()),
                    Times.Never);

            _mapperMock
                .Verify(
                    x => x.Map<AdminDto>(
                        It.IsAny<Admin>()),
                    Times.Never);
        }
    }
}
