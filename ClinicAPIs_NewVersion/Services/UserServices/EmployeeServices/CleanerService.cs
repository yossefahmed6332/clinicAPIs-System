using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices;
using AutoMapper;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.CleanerDTO;
using clinicAPIsSystem.Models.User.Employee;

namespace clinicAPIsSystem.Services.UserServices.EmployeeServices
{
    public class CleanerService:ICleanerService
    {
        private readonly ICleanerRepository _cleanerRepository; 
        private readonly IUserService _userService; 
        private readonly IMapper _mapper;
        private readonly ILogger<CleanerService> _logger;

        public CleanerService(ICleanerRepository cleanerRepository, IUserService userService, IMapper mapper, ILogger<CleanerService> logger)
        {
            _cleanerRepository = cleanerRepository;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CleanerDto> CreateCleanerAsync(CreateCleanerDto createCleanerDto)
        { 
            _logger
                .LogInformation("Creating a new cleaner with email: {Email} , username: {UserName} ,salary: {SalaryPerHour} and hours of work {hrsOfWork}", createCleanerDto.Email, createCleanerDto.UserName, createCleanerDto.SalaryPerHour, createCleanerDto.HoursWorked);
            await _userService.ValidateUserCreation(createCleanerDto.Email, createCleanerDto.UserName, createCleanerDto.PhoneNumber);


            var cleaner = new Cleaner(createCleanerDto.FirstName,
                createCleanerDto.LastName,
                createCleanerDto.UserName,
                createCleanerDto.Email,
                createCleanerDto.PhoneNumber,
                createCleanerDto.SalaryPerHour,
                createCleanerDto.HoursWorked,
                createCleanerDto.ShiftStart,
                createCleanerDto.ShiftEnd
                );

            var createdCleaner = await _cleanerRepository.CreateCleanerAsync(cleaner,createCleanerDto.Password);
            if (!(createdCleaner.addUserRes &&
                  createdCleaner.addPasswordRes &&
                  createdCleaner.addRoleRes))
            {
                if (createdCleaner.addUserRes)
                {
                    await _userService.DeleteUserAsync(
                        createdCleaner.cleaner.Id);
                }
                _logger
                    .LogError("Failed to create cleaner with salary: {SalaryPerHour} and hours of work {hrsOfWork}",  createCleanerDto.SalaryPerHour, createCleanerDto.HoursWorked);

                throw new Exception("Cannot create user, try again");
            }
            _logger
                .LogInformation("Successfully created a new cleaner with ID {id} , email: {Email} , username: {UserName} ,salary: {SalaryPerHour} and hours of work {hrsOfWork}",createdCleaner.cleaner.Id, createCleanerDto.Email, createCleanerDto.UserName, createCleanerDto.SalaryPerHour, createCleanerDto.HoursWorked);

            return _mapper.Map<CleanerDto>(createdCleaner.cleaner);
        }

        public async Task<List<CleanerDto>> GetAllCleanersAsync()
        {
            _logger
                .LogDebug("Retrieving all cleaners from the database");
            var cleaners = await _cleanerRepository.GetAllCleanersAsync();
            return _mapper.Map<List<CleanerDto>>(cleaners);
        }

        public async Task<CleanerDto> GetCleanerAsync(int cleanerId)
        {
            _logger
                .LogDebug("Retrieving cleaner with ID {id} from the database", cleanerId);
            var cleaner = await _cleanerRepository.GetCleanerAsync(cleanerId);
            if (cleaner == null)
            {
                _logger
                .LogWarning("Cleaner with ID {id} not found in the database", cleanerId);
                throw new Exception("Cleaner not found");

            }
            return _mapper.Map<CleanerDto>(cleaner);
        }

        public async Task<CleanerDto> UpdateCleanerAsync(
            UpdateCleanerDto updateCleanerDto,
            int id)
        {
            _logger
                .LogInformation("Updating cleaner with ID {id} ", id);
            await _userService.ValidateUserCreation(
                updateCleanerDto.Email,
                updateCleanerDto.UserName,
                updateCleanerDto.PhoneNumber);

            var cleaner = await _cleanerRepository.GetCleanerAsync(id);
            _logger
                .LogDebug("Retrieving cleaner with ID {id} from the database", id);

            if (cleaner == null)
            {
                _logger
                    .LogWarning("Cleaner with ID {id} not found in the database", id);
                throw new KeyNotFoundException(
                    $"Cannot find cleaner with id {id}");
            }

            cleaner.UpdateCleanerInfo(
                updateCleanerDto.FirstName,
                updateCleanerDto.LastName,
                updateCleanerDto.UserName,
                updateCleanerDto.Email,
                updateCleanerDto.PhoneNumber,
                updateCleanerDto.SalaryPerHour,
                updateCleanerDto.HoursWorked,
                updateCleanerDto.ShiftStart,
                updateCleanerDto.ShiftEnd
            );

            var updatedCleaner =
                await _cleanerRepository.UpdateCleanerAsync(cleaner);
            _logger
                .LogInformation("Successfully updated cleaner with ID {id} ", id);

            return _mapper.Map<CleanerDto>(updatedCleaner);
        }

    }
}
