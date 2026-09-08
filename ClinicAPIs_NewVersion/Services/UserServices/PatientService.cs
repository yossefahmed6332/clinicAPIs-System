using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models.User;
using AutoMapper; 
namespace clinicAPIsSystem.Services.UserServices

{
    public class PatientService:IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserService _userService;
        private readonly IMedicalRecordService _medicalRecordService; 
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;
        public PatientService(IPatientRepository patientRepository, IUserService userService, IMedicalRecordService medicalRecordService, IMapper mapper,ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _userService = userService;
            _medicalRecordService = medicalRecordService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto )
        {
            _logger
                .LogInformation("Creating new Patient ");
            await _userService.ValidateUserCreation(createPatientDto.Email,createPatientDto.UserName,createPatientDto.PhoneNumber);
            _logger
                .LogInformation("Creating new Medical Record for Patient ");
            var medicalRecord= await _medicalRecordService.CreateMedicalRecordAsync(createPatientDto.createMedicalRecordDto);

            var patient = new Patient(medicalRecord.Id,createPatientDto.FirstName,createPatientDto.LastName,createPatientDto.UserName,createPatientDto.Email,createPatientDto.PhoneNumber);
            var patientCreated = await  _patientRepository.CreatePatientAsync(patient, createPatientDto.Password);

            if (!(patientCreated.addUserRes &&
                patientCreated.addPasswordRes &&
                patientCreated.addRoleRes))
            {
                
                if (patientCreated.addUserRes)
                {
                    await _userService.DeleteUserAsync(patientCreated.patient.Id);
                }
                _logger
                    .LogError("Cannot create user, try again");
                throw new Exception("Cannot create user, try again");
            }

            _logger.
                LogInformation($"Patient with ID {patientCreated.patient.Id} created successfully");
            return _mapper.Map<PatientDto>(patientCreated.patient);
        }


        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            _logger
                .LogDebug("Retrieving all patients from the database");
            return _mapper.Map<List<PatientDto>>(await _patientRepository.GetAllPatientsAsync());
        }
        public async Task<PatientDto?> GetPatientAsync(int id)
        {
            _logger.LogDebug("Retrieving patient with ID {patientId} ", id);
            var patient = await _patientRepository.GetPatientAsync(id);
            if (patient == null)
            {
                _logger
                    .LogWarning("Can not find Patient with ID {patientId}", id);
                throw new KeyNotFoundException($"Cannot find user with ID{id}");
            }
            return _mapper.Map<PatientDto>(patient);
        }
        public async Task<PatientDto> UpdatePatientAsync(UpdatePatientDto patient, int id)
        {
            _logger
                .LogInformation("Updating patient with ID {patientID}", id);
            await _userService.ValidateUserCreation(patient.Email, patient.UserName, patient.PhoneNumber);
            _logger.
                LogDebug("Retrieving patient with ID {patientId}", id); 
            var patientEntity = await _patientRepository.GetPatientAsync(id);
            if (patientEntity == null)
            {
                _logger
                    .LogWarning("Patient with ID {patientId} not found", id);
                throw new KeyNotFoundException($"Cannot find user with ID{id}");
            }
            patientEntity.UpdatePatientInfo(patient.FirstName, patient.LastName, patient.UserName, patient.Email, patient.PhoneNumber);
            var updatedPatient = await _patientRepository.UpdatePatientAsync(patientEntity);    
            _logger.
                LogInformation("Patient with ID {patientId} updated successfully",id);
            return _mapper.Map<PatientDto>(updatedPatient);
        }



    }
}
