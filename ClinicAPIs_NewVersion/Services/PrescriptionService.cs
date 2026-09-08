using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IService;
using AutoMapper;
using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PrescriptionService> _logger;
        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMapper mapper, ILogger<PrescriptionService> logger)
        {
            _mapper = mapper;
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
        }


        public async Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createPrescriptionDto)
        {
            _logger.
                LogInformation(
                "Creating prescription for Doctor ID {DoctorId} and Medical Record ID {MedicalRecordId}",
                createPrescriptionDto.DoctorId,
                createPrescriptionDto.medicalRecordId);
            var prescription = new Prescription(
                createPrescriptionDto.MedicalName,
                createPrescriptionDto.Dosage,
                createPrescriptionDto.Frequency,
                createPrescriptionDto.Duration,
                createPrescriptionDto.Instructions,
                createPrescriptionDto.Diagnosis,
                createPrescriptionDto.Date,
                createPrescriptionDto.DoctorId,
                createPrescriptionDto.medicalRecordId
            );
            
            prescription = await _prescriptionRepository.CreatePrescriptionAsync(prescription);
            _logger.
                LogInformation("Prescription created with ID {PrescriptionId}", prescription.Id);
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<List<PrescriptionDto>> GetAllPrescriptionsAsync()
        {
            _logger
                .LogDebug("Retrieving all prescriptions from the repository.");
            return _mapper.Map<List<PrescriptionDto>>(await _prescriptionRepository.GetAllPrescriptionsAsync());
        }

        public async Task<PrescriptionDto> GetPrescriptionAsync(int id)
        {
            _logger
                .LogDebug("Retrieving prescription with ID {PrescriptionId} from the repository.", id);
            var prescription = await _prescriptionRepository.GetPrescriptionAsync(id);
            if (prescription == null)
            {
                _logger
                    .LogWarning("Prescription with ID {PrescriptionId} not found.", id);
                throw new KeyNotFoundException($"Prescription with ID{id} does not exist");

            }

            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<List<PrescriptionDto>> GetPrescriptionsByMedicalRecordIdAsync(int medicalRecordId)
        {
            _logger
                .LogDebug("Retrieving prescriptions for Medical Record ID {MedicalRecordId} from the repository.", medicalRecordId);
            return _mapper.Map<List<PrescriptionDto>>(await _prescriptionRepository.GetPrescriptionsByMedicalRecordIdAsync(medicalRecordId));
        }

        public async Task<List<PrescriptionDto>> GetPrescriptionsByDoctorIdAsync(int doctorId)
        {
            _logger
                .LogDebug("Retrieving prescriptions for Doctor ID {DoctorId} from the repository.", doctorId);
            return _mapper.Map<List<PrescriptionDto>>(await _prescriptionRepository.GetPrescriptionsByDoctorIdAsync(doctorId));

        }

        public async Task<PrescriptionDto> UpdatePrescriptionAsync(UpdatePrescriptionDto updatePrescriptionDto,int id)
        {
            _logger
                .LogDebug("Updating prescription with ID {PrescriptionId} in the repository.", id);
            var prescription = await _prescriptionRepository.GetPrescriptionAsync(id);
            if (prescription == null)
            {
                _logger
                    .LogWarning("Prescription with ID {PrescriptionId} not found for update.", id);
                throw new KeyNotFoundException($"Prescription with ID {id} does not exist");
            }

            prescription.Update(
                updatePrescriptionDto.MedicalName,
                updatePrescriptionDto.Dosage,
                updatePrescriptionDto.Frequency,
                updatePrescriptionDto.Duration,
                updatePrescriptionDto.Instructions,
                updatePrescriptionDto.Diagnosis
                );
            prescription = await _prescriptionRepository.UpdatePrescriptionAsync(prescription);
            _logger
                .LogInformation("Prescription with ID {PrescriptionId} updated successfully.", id);
            return _mapper.Map<PrescriptionDto>(prescription) ;
        }

        public async Task DeletePrescriptionAsync(int id)
        {
            _logger
                .LogInformation($"DeletePrescription: {id}");
            var prescription = await _prescriptionRepository.GetPrescriptionAsync(id); 
            if (prescription == null)
            {
                _logger
                    .LogWarning("Can not delete prescription with ID {PrescriptionId} ", id);
                throw new KeyNotFoundException($"User with ID {id} not found");
            }
            await _prescriptionRepository.DeletePrescriptionAsync(prescription);
            _logger
                .LogInformation("Prescription with ID {PrescriptionId} deleted successfully.", id);


        }
    }
}
