using AutoMapper;
using clinicAPIsSystem.DTOs.VitalSignsDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;

namespace clinicAPIsSystem.Service
{
    public class VitalSignsService : IVitalSignsService
    {
        private readonly IVitalSignsRepository _vitalSignsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VitalSignsService> _logger;

        public VitalSignsService(
            IVitalSignsRepository vitalSignsRepository,
            IMapper mapper,
            ILogger<VitalSignsService> logger)
        {
            _vitalSignsRepository = vitalSignsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<VitalSignsDto> CreateVitalSignsAsync(
            CreateVitalSignsDto createVitalSignsDto)
        {
            _logger.LogInformation(
                "Creating new vital signs for patient with MedicalRecordId: {MedicalRecordId} by NurseId: {NurseId}",
                createVitalSignsDto.MedicalRecordId,
                createVitalSignsDto.NurseId);

            var vitalSigns = new VitalSigns(
                createVitalSignsDto.BloodPressureSystolic,
                createVitalSignsDto.BloodPressureDiastolic,
                createVitalSignsDto.HeartRate,
                createVitalSignsDto.Temperature,
                createVitalSignsDto.OxygenSaturation,
                createVitalSignsDto.RecordedAt,
                createVitalSignsDto.NurseId,
                createVitalSignsDto.MedicalRecordId
            );

            vitalSigns =
                await _vitalSignsRepository.CreateVitalSignsAsync(vitalSigns);

            _logger.LogInformation(
                "Vital signs created successfully with ID {VitalSignsId}",
                vitalSigns.Id);

            return _mapper.Map<VitalSignsDto>(vitalSigns);
        }

        public async Task<List<VitalSignsDto>> GetAllVitalSignsAsync()
        {
            _logger.LogDebug(
                "Retrieving all vital signs from the repository.");

            var vitalSigns =
                await _vitalSignsRepository.GetAllVitalSignsAsync();

            return _mapper.Map<List<VitalSignsDto>>(vitalSigns);
        }

        public async Task<VitalSignsDto> GetVitalSignsAsync(int id)
        {
            _logger.LogDebug(
                "Retrieving vital signs with ID {VitalSignsId} from the repository.",
                id);

            var vitalSigns =
                await _vitalSignsRepository.GetVitalSignsAsync(id);

            if (vitalSigns == null)
            {
                _logger.LogWarning(
                    "Vital signs with ID {VitalSignsId} not found.",
                    id);

                throw new KeyNotFoundException(
                    $"Vital signs with ID {id} not found.");
            }

            return _mapper.Map<VitalSignsDto>(vitalSigns);
        }

        public async Task<List<VitalSignsDto>> GetVitalSignsByMedicalRecordIdAsync(
            int medicalRecordId)
        {
            _logger.LogDebug(
                "Retrieving vital signs for MedicalRecordId {MedicalRecordId} from the repository.",
                medicalRecordId);

            var vitalSigns =
                await _vitalSignsRepository
                    .GetVitalSignsByMedicalRecordIdAsync(medicalRecordId);

            return _mapper.Map<List<VitalSignsDto>>(vitalSigns);
        }

        public async Task<List<VitalSignsDto>> GetVitalSignsByNurseIdAsync(
            int nurseId)
        {
            _logger.LogDebug(
                "Retrieving vital signs recorded by NurseId {NurseId} from the repository.",
                nurseId);

            var vitalSigns =
                await _vitalSignsRepository
                    .GetVitalSignsByNurseIdAsync(nurseId);

            return _mapper.Map<List<VitalSignsDto>>(vitalSigns);
        }

        public async Task<VitalSignsDto> UpdateVitalSignsAsync(
            UpdateVitalSignsDto updateVitalSignsDto,
            int id)
        {
            _logger.LogInformation(
                "Updating vital signs with ID {VitalSignsId}.",
                id);

            var vitalSigns =
                await _vitalSignsRepository.GetVitalSignsAsync(id);

            if (vitalSigns == null)
            {
                _logger.LogWarning(
                    "Vital signs with ID {VitalSignsId} not found for update.",
                    id);

                throw new KeyNotFoundException(
                    $"Vital signs with ID {id} not found.");
            }

            vitalSigns.Update(
                updateVitalSignsDto.BloodPressureSystolic,
                updateVitalSignsDto.BloodPressureDiastolic,
                updateVitalSignsDto.HeartRate,
                updateVitalSignsDto.Temperature,
                updateVitalSignsDto.OxygenSaturation
            );

            vitalSigns =
                await _vitalSignsRepository.UpdateVitalSignsAsync(vitalSigns);

            _logger.LogInformation(
                "Vital signs with ID {VitalSignsId} updated successfully.",
                id);

            return _mapper.Map<VitalSignsDto>(vitalSigns);
        }

        public async Task DeleteVitalSignsAsync(int id)
        {
            _logger.LogInformation(
                "Deleting vital signs with ID {VitalSignsId}.",
                id);

            _logger.LogDebug(
                "Retrieving vital signs with ID {VitalSignsId} before deletion.",
                id);

            var vitalSigns =
                await _vitalSignsRepository.GetVitalSignsAsync(id);

            if (vitalSigns == null)
            {
                _logger.LogWarning(
                    "Vital signs with ID {VitalSignsId} not found for deletion.",
                    id);

                throw new KeyNotFoundException(
                    $"Vital signs with ID {id} not found.");
            }

            await _vitalSignsRepository.DeleteVitalSignsAsync(vitalSigns);

            _logger.LogInformation(
                "Vital signs with ID {VitalSignsId} deleted successfully.",
                id);
        }
    }
}