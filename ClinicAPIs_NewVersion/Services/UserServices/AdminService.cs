using clinicAPIsSystem.IRepositoryService.IUserRepository;
using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.AdminDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User;
namespace clinicAPIsSystem.Services.UserServices
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService; 
        private readonly ILogger<AdminService> _logger;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, IUserService userService,ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        public async Task<AdminDto> CreateAdminAsync(CreateAdminDto admin)
        {
            _logger
                .LogInformation("Creating new admin with email {adminEmail} " , admin.Email);
            await _userService.ValidateUserCreation(admin.Email, admin.UserName, admin.PhoneNumber);

            var adminEntity = new Admin(admin.FirstName, admin.LastName, admin.Email, admin.UserName, admin.PhoneNumber);
            var createdAdmin = await _adminRepository.CreateAdminAsync(adminEntity, admin.Password);

            if (!(createdAdmin.addUserRes &&
                createdAdmin.addPasswordRes &&
                createdAdmin.addRoleRes))
            {
                if (createdAdmin.addUserRes)
                {
                    await _userService.DeleteUserAsync(createdAdmin.admin.Id);
                }
                _logger.LogError("Can not create the new admin with email {adminEmail} , try again",admin.Email);
                throw new Exception("Cannot create user, try again");
            }
            _logger
                .LogInformation("New admin created successfully with ID {adminId}", createdAdmin.admin.Id);
            return _mapper.Map<AdminDto>(createdAdmin.admin);
        }

        public async Task<List<AdminDto>> GetAllAdminsAsync()
        {
            _logger.
                LogDebug("Retrieving all admins ");
            return _mapper.Map<List<AdminDto>>(await _adminRepository.GetAllAdminsAsync());
        }

        public async Task<AdminDto> GetAdminAsync(int id)
        {
            _logger
                .LogDebug("Retrieving new admin with ID {adminId}", id);
            var admin = await _adminRepository.GetAdminAsync(id);
            if (admin == null)
            {
                _logger
                    .LogWarning("Admin with ID {adminId} not found", id);
                throw new KeyNotFoundException($"Cannot find user with ID{id}");
            }

            return _mapper.Map<AdminDto>(admin);
        }


        public async Task<AdminDto> UpdateAdminAsync (UpdateAdminDto admin, int id)
        {

            _logger
                .LogInformation("Updating admin with ID {adminId}", id);

            _logger
                .LogDebug("Retrieving admin with ID {adminID} ", id); 
            await _userService.ValidateUserCreation(admin.Email,admin.UserName, admin.PhoneNumber);
            var adminEntity = await _adminRepository.GetAdminAsync(id);


            if (adminEntity == null)
            {
                _logger
                    .LogWarning("Can not find admin with ID {adminId} ", id);
                throw new KeyNotFoundException($"Cannot found user with id {id}");

            }
            adminEntity.UpdateAdminInfo(admin.FirstName, admin.LastName, admin.UserName, admin.Email, admin.PhoneNumber);
            var updateAdmin = await _adminRepository.UpdateAdminAsync(adminEntity); 
            _logger
                .LogInformation("Admin with ID {adminId} updated successfully", id);
            return _mapper.Map<AdminDto>(updateAdmin);
        }




    }
}
