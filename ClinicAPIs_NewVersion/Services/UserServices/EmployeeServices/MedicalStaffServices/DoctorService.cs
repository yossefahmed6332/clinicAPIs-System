using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using AutoMapper;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;

namespace clinicAPIsSystem.Services.UserServices.EmployeeServices.MedicalStaffServices
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IMapper mapper,
            IUserService userService,
            ILogger<DoctorService> logger)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        public async Task<DoctorDto> CreateDoctorAsync(
            CreateDoctorDto createDoctorDto
           )
        {
            _logger
                .LogInformation(
                    "Creating new doctor"); 

            await _userService.ValidateUserCreation(
                createDoctorDto.Email,
                createDoctorDto.UserName,
                createDoctorDto.PhoneNumber);

            var doctor = new Doctor(
                createDoctorDto.FirstName,
                createDoctorDto.LastName,
                createDoctorDto.UserName,
                createDoctorDto.Email,
                createDoctorDto.PhoneNumber,
                createDoctorDto.SalaryPerHour,
                createDoctorDto.HoursWorked,
                createDoctorDto.ShiftStart,
                createDoctorDto.ShiftEnd,
                createDoctorDto.Degree,
                createDoctorDto.University,
                createDoctorDto.YearsOfExperience,
                createDoctorDto.GraduationYear,
                createDoctorDto.License
            );

            var doctorCreated =
                await _doctorRepository.CreateDoctorAsync(
                    doctor,
                    createDoctorDto.Password);

            if (!(doctorCreated.addUserRes &&
                  doctorCreated.addPasswordRes &&
                  doctorCreated.addRoleRes))
            {
                if (doctorCreated.addUserRes)
                {
                    await _userService.DeleteUserAsync(
                        doctorCreated.doctor.Id);
                }
                _logger
                    .LogError(
                        "Failed to create doctor"
                        );

                throw new Exception(
                    "Cannot create user, try again");
            }
            _logger
                .LogInformation(
                    "Doctor created successfully with ID {doctorId}",
                    doctorCreated.doctor.Id);

            return _mapper.Map<DoctorDto>(
                doctorCreated.doctor);
        }

        public async Task<DoctorDto> GetDoctorAsync(int id)
        {
            _logger
                .LogDebug(
                    "Retrieving doctor with ID {id}",
                    id);
            var doctor =
                await _doctorRepository.GetDoctorAsync(id);

            if (doctor == null)
            {
                _logger.LogWarning(
                    "Doctor with ID {id} not found",
                    id);
                throw new KeyNotFoundException(
                    $"Cannot find doctor with id {id}");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            _logger.LogDebug("Retrieving all doctors from the database");
            var doctors =
                await _doctorRepository.GetAllDoctorsAsync();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(
            UpdateDoctorDto updateDoctorDto,
            int id)
        {
            _logger
                .LogInformation(
                    "Updating doctor with ID {id}",
                    id);
            await _userService.ValidateUserCreation(
                updateDoctorDto.Email,
                updateDoctorDto.UserName,
                updateDoctorDto.PhoneNumber);

            var doctor =
                await _doctorRepository.GetDoctorAsync(id);

            if (doctor == null)
            {
                _logger.LogWarning(
                    "Doctor with ID {id} not found for update",
                    id);
                throw new KeyNotFoundException(
                    $"Cannot find doctor with id {id}");
            }

            doctor.UpdateDoctorInfo(
                updateDoctorDto.FirstName,
                updateDoctorDto.LastName,
                updateDoctorDto.UserName,
                updateDoctorDto.Email,
                updateDoctorDto.PhoneNumber,
                updateDoctorDto.SalaryPerHour,
                updateDoctorDto.HoursWorked,
                updateDoctorDto.ShiftStart,
                updateDoctorDto.ShiftEnd,
                updateDoctorDto.Degree,
                updateDoctorDto.University,
                updateDoctorDto.YearsOfExperience,
                updateDoctorDto.GraduationYear,
                updateDoctorDto.License
            );

            var updatedDoctor =
                await _doctorRepository.UpdateDoctorAsync(doctor);
            _logger
                .LogInformation("Doctor with ID {id} updated successfully", id);

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }
    }
}