using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using AutoMapper;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
namespace clinicAPIsSystem.Services.UserServices.EmployeeServices.MedicalStaffServices

{
    public class NurseService:INurseService
    {
        private readonly INurseRepository _nurseRepository; 
        private readonly IMapper _mapper; 
        private readonly IUserService _userService;
        private readonly ILogger<NurseService> _logger;
        public NurseService(INurseRepository nurseRepository, IMapper mapper, IUserService userService, ILogger<NurseService> logger)
        {
            _nurseRepository = nurseRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        public async Task<NurseDto> CreateNurseAsync(CreateNurseDto createNurseDto)
        {
            _logger
                .LogInformation("Creating new nurse"); 
            await _userService.ValidateUserCreation(createNurseDto.Email, createNurseDto.UserName, createNurseDto.PhoneNumber);

            var nurse = new Nurse(
                createNurseDto.FirstName,
                createNurseDto.LastName,
                createNurseDto.UserName,
                createNurseDto.Email,
                createNurseDto.PhoneNumber,
                createNurseDto.SalaryPerHour,
                createNurseDto.HoursWorked,
                createNurseDto.ShiftStart,
                createNurseDto.ShiftEnd,
                createNurseDto.Degree,
                createNurseDto.University,
                createNurseDto.YearsOfExperience,
                createNurseDto.GraduationYear,
                createNurseDto.License
                );

            var nurseCreated = await _nurseRepository.CreateNurseAsync(nurse, createNurseDto.Password);

            if (!(nurseCreated.addUserRes &&
                  nurseCreated.addPasswordRes &&
                  nurseCreated.addRoleRes))
            {
                if (nurseCreated.addUserRes)
                {
                    await _userService.DeleteUserAsync(
                        nurseCreated.nurse.Id);
                }
                _logger
                    .LogError("Failed to create nurse");
                throw new Exception("Cannot create user, try again");
            }
            _logger
                .LogInformation("Nurse created successfully with ID {nurseId}", nurseCreated.nurse.Id);

            return _mapper.Map<NurseDto>(nurseCreated.nurse);
        }

        public async Task<NurseDto> GetNurseAsync(int nurseId)
        {
            _logger
                .LogDebug("Fetching nurse with ID {nurseId}", nurseId);
            var nurse = await _nurseRepository.GetNurseAsync(nurseId);
            if (nurse == null)
            {
                _logger
                    .LogWarning("Nurse with ID {nurseId} not found", nurseId);
                throw new Exception("Nurse not found");
            }
            return _mapper.Map<NurseDto>(nurse);
        }
        public async Task<List<NurseDto>> GetAllNursesAsync()
        {
            _logger
                .LogDebug("Retrieving all nurses from the repository");
            var nurses = await _nurseRepository.GetAllNursesAsync();
            return _mapper.Map<List<NurseDto>>(nurses);
        }



        public async Task<NurseDto> UpdateNurseAsync(       
            
            UpdateNurseDto updateNurseDto, int id)
        {
            _logger
                .LogInformation("Updating nurse with ID {nurseId}", id);
            await _userService.ValidateUserCreation(
                updateNurseDto.Email,
                updateNurseDto.UserName,
                updateNurseDto.PhoneNumber);

            var nurse = await _nurseRepository.GetNurseAsync(id);

            if (nurse == null)
            {
                _logger
                    .LogWarning("Nurse with ID {nurseId} not found for update", id);
                throw new KeyNotFoundException(
                    $"Cannot find nurse with id {id}");
            }

            nurse.UpdateNurseInfo(
                updateNurseDto.FirstName,
                updateNurseDto.LastName,
                updateNurseDto.UserName,
                updateNurseDto.Email,
                updateNurseDto.PhoneNumber,
                updateNurseDto.SalaryPerHour,
                updateNurseDto.HoursWorked,
                updateNurseDto.ShiftStart,
                updateNurseDto.ShiftEnd,
                updateNurseDto.Degree,
                updateNurseDto.University,
                updateNurseDto.YearsOfExperience,
                updateNurseDto.GraduationYear,
                updateNurseDto.License
            );

            var updatedNurse =
                await _nurseRepository.UpdateNurseAsync(nurse);
            _logger
                .LogInformation("Nurse with ID {nurseId} updated successfully", id);

            return _mapper.Map<NurseDto>(updatedNurse);
        }

    }
}
