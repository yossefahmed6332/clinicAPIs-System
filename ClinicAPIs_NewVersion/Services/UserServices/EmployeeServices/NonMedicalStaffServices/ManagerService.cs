using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ManagerDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

namespace clinicAPIsSystem.Services.UserServices.EmployeeServices.NonMedicalStaffServices
{
    public class ManagerService:IManagerService
    {
        private readonly IMapper _mapper; 
        private readonly IUserService _userService; 
        private readonly IManagerRepository _managerRepository;
        private readonly ILogger<ManagerService> _logger;

        public ManagerService(IMapper mapper, IUserService userService, IManagerRepository managerRepository, ILogger<ManagerService> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _managerRepository = managerRepository;
            _logger = logger;
        }
        public async Task<ManagerDto> CreateManagerAsync(CreateManagerDto createManagerDto)
        {
            _logger
                .LogInformation("Creating a new manager with email: {Email}", createManagerDto.Email);
            await _userService.ValidateUserCreation(createManagerDto.Email, createManagerDto.UserName, createManagerDto.PhoneNumber);
            var manager = new Manager(
                createManagerDto.FirstName,
                createManagerDto.LastName,
                createManagerDto.UserName,
                createManagerDto.Email,
                createManagerDto.PhoneNumber,
                createManagerDto.SalaryPerHour,
                createManagerDto.HoursWorked,
                createManagerDto.ShiftStart,
                createManagerDto.ShiftEnd,
                createManagerDto.Degree,
                createManagerDto.University,
                createManagerDto.YearsOfExperience,
                createManagerDto.GraduationYear,
                createManagerDto.License
            );

            var managerCreated = await _managerRepository.CreateManagerAsync(manager, createManagerDto.Password);
            if (!(managerCreated.addUserRes &&
                  managerCreated.addPasswordRes &&
                  managerCreated.addRoleRes))
            {
                if (managerCreated.addUserRes)
                {
                    await _userService.DeleteUserAsync(managerCreated.manager.Id);
                }
                _logger
                    .LogError("Failed to create manager with email: {Email},try again", createManagerDto.Email);
                throw new Exception("Cannot create user, try again");
            }

            return _mapper.Map<ManagerDto>(managerCreated.manager);
        }

        public async Task<List<ManagerDto>> GetAllManagersAsync()
        {
            _logger
                .LogDebug("Retrieving all managers from the repository");
            var managers = await _managerRepository.GetAllManagersAsync();
            return _mapper.Map<List<ManagerDto>>(managers);
        }
        public async Task<ManagerDto> GetManagerAsync(int id)
        {
            _logger
                .LogDebug("Retrieving manager with ID {ID}", id);
            var manager = await _managerRepository.GetManagerAsync(id);
            if (manager == null)
            {
                _logger
                    .LogError("Can not get manager with ID {ID}, manager not found", id);
                throw new KeyNotFoundException($"Cannot find user with ID{id}");
            }
            return _mapper.Map<ManagerDto>(manager);
        }

        public async Task<ManagerDto> UpdateManagerAsync (UpdateManagerDto updateManager,int id)
        {
            _logger
                .LogInformation("Updating manager with ID {ID}", id);
            await _userService.ValidateUserCreation(updateManager.Email,
                updateManager.UserName,
                updateManager.PhoneNumber);
            var manager = await _managerRepository.GetManagerAsync(id);  
            if (manager== null)
            {
                _logger
                    .LogWarning("Manager with ID {ID} not found for update", id);
                throw new KeyNotFoundException($"User with ID{id} not found"); 
            }

            manager.UpdateManagerInfo(
                updateManager.FirstName,
                updateManager.LastName,
                updateManager.UserName,
                updateManager.Email,
                updateManager.PhoneNumber,
                updateManager.SalaryPerHour,
                updateManager.HoursWorked,
                updateManager.ShiftStart,
                updateManager.ShiftEnd,
                updateManager.Degree,
                updateManager.University,
                updateManager.YearsOfExperience,
                updateManager.GraduationYear,
                updateManager.License
            );

            var updatedManager = await _managerRepository.UpdateManagerAsync(manager);
            _logger.
                LogInformation("Manager with ID {ID} updated successfully", id);

            return _mapper.Map<ManagerDto> (updatedManager);

           

        }

    }
}
