using clinicAPIsSystem.IService;
using AutoMapper;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.Service
{
    public class MedicalRecordService:IMedicalRecordService
    {
        private readonly IMapper _mapper;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly ILogger<MedicalRecordService> _logger; 
        public MedicalRecordService(IMapper mapper, IMedicalRecordRepository medicalRecordRepository, ILogger<MedicalRecordService> logger)
        {
            _mapper = mapper;
            _medicalRecordRepository = medicalRecordRepository;
            _logger = logger;
        }
        public async Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createMedicalRecordDto)
        {
            _logger.LogInformation
                ("Creating medical record for Patient ID {PatientId}",
                createMedicalRecordDto.PatientId);

            var medicalRecord = new MedicalRecord(
                createMedicalRecordDto.Height,
                createMedicalRecordDto.Weight,
                createMedicalRecordDto.BloodType,
                createMedicalRecordDto.PatientId);
            var createdMedicalRecord = await _medicalRecordRepository.CreateMedicalRecordAsync(medicalRecord);
             _logger.LogInformation
                ("Medical record created with ID {MedicalRecordId} for Patient ID {PatientId}", createdMedicalRecord.Id, createdMedicalRecord.PatientId);
            return _mapper.Map<MedicalRecordDto>(createdMedicalRecord);
        }
 

        public async Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync()
        {
            _logger.
                LogDebug("Retrieving all medical records");
            var medicalRecords = await _medicalRecordRepository.GetAllMedicalRecordsAsync();
            return _mapper.Map<List<MedicalRecordDto>>(medicalRecords);
        }

        public async Task<MedicalRecordDto> GetMedicalRecord(int id)
        {
            _logger
                .LogDebug("Retrieving medical record with ID {MedicalRecordId}", id);
            var medicalRecord = await _medicalRecordRepository.GetMedicalRecordAsync(id);
            if (medicalRecord == null)
            {
                _logger
                    .LogWarning("Medical record with ID {MedicalRecordId} not found", id);
                throw new KeyNotFoundException($"Medical record with ID {id} not found.");
            }
            return _mapper.Map<MedicalRecordDto>(medicalRecord);
        }

        public async Task<MedicalRecordDto> GetMedicalByPatientIdRecord(int PatientId)
        {
            _logger
                .LogDebug("Retrieving medical record with Patient ID {PatientId}", PatientId);
            var medicalRecord = await _medicalRecordRepository.GetMedicalRecordByPatientIdAsync(PatientId);
            if (medicalRecord == null)
            {
                _logger
                    .LogWarning("Medical record with Patient ID {PatientId} not found", PatientId);
                throw new KeyNotFoundException($"Medical record with Patient ID {PatientId} not found.");
            }
            return _mapper.Map<MedicalRecordDto>(medicalRecord);
        }

        public async Task<MedicalRecordDto> UpdateMedicalRecordAsync(UpdateMedicalRecordDto updateMedicalRecord, int Id)
        {
            _logger.LogDebug
                ("Updating medical record with ID {MedicalRecordId}", Id);
            var medicalRecord = await _medicalRecordRepository.GetMedicalRecordAsync(Id); 
            if (medicalRecord == null)
            {
                _logger.LogWarning
                    ("Medical record with ID {MedicalRecordId} not found", Id);
                throw new KeyNotFoundException($"Medical record with ID {Id} not found.");
            }
            _logger.LogInformation
                ("Updating medical record with ID {MedicalRecordId} for Patient ID {PatientId}",
                Id, updateMedicalRecord.PatientId);
            medicalRecord.Update(updateMedicalRecord.Height, updateMedicalRecord.Weight,updateMedicalRecord.BloodType, updateMedicalRecord.PatientId);
            _logger.LogInformation
                ("Medical record with ID {MedicalRecordId} updated for Patient ID {PatientId}",
                Id, updateMedicalRecord.PatientId);
            return _mapper.Map<MedicalRecordDto>(medicalRecord);
        }



    }
}
