using AutoMapper;
using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.Service
{
    public class ExaminationResultService : IExaminationResultService
    {
        private readonly IMapper _mapper;
        private readonly IExaminationResultRepository _examinationResultRepository;
        private readonly ILogger<ExaminationResultService>  _logger;
        public ExaminationResultService(IMapper mapper, IExaminationResultRepository examinationResultRepository, ILogger<ExaminationResultService> logger)
        {
            _mapper = mapper;

            _examinationResultRepository = examinationResultRepository;
            _logger = logger;
        
        }
        public async Task<ExaminationResultDto> CreateExaminationResultAsync(CreateExaminationResultDto createExaminationResultDto)
        {
            _logger.LogInformation(
                "Creating examination result for Medical Record {MedicalRecordId}",
                createExaminationResultDto.MedicalRecordId);


            var examinationResult = new ExaminationResult(
                createExaminationResultDto.TestType!
                , createExaminationResultDto.ResultValue!
                , createExaminationResultDto.Unit!
                , createExaminationResultDto.NormalRange!
                , createExaminationResultDto.Note!
                , createExaminationResultDto.RecordedAt
                , createExaminationResultDto.NurseId
                , createExaminationResultDto.MedicalRecordId
            );

            examinationResult = await _examinationResultRepository.CreateExaminationResultAsync(examinationResult);
            _logger
                .LogInformation("Creating examination result  with ID {ExaminationResultId} ",
                examinationResult.Id);
            return _mapper.Map<ExaminationResultDto>(examinationResult);
        }

        public async Task<List<ExaminationResultDto>> GetAllExaminationResultsAsync()
        {
            _logger
                .LogDebug("Retrieving all examination results from the database.");
            var examinationResults = await _examinationResultRepository.GetAllExaminationResultsAsync();
            return _mapper.Map<List<ExaminationResultDto>>(examinationResults);
        }

        public async Task<ExaminationResultDto> GetExaminationResultAsync(int id)
        {
            _logger
                .LogDebug("Retrieving examination result with ID {ExaminationResultId} from the database.", id);

            var examinationResult = await _examinationResultRepository.GetExaminationResultAsync(id);
            if (examinationResult == null)
            {
                _logger
                    .LogWarning("Examination result with ID {ExaminationResultId} not found.", id);
                throw new KeyNotFoundException($"Examination Result with ID {id} not found.");
            }
            return _mapper.Map<ExaminationResultDto>(examinationResult);
        }
        public async Task<List<ExaminationResultDto>> GetExaminationResultsByNurseIdAsync(int nurseId)
        {
            _logger
                .LogDebug("Retrieving examination results for Nurse with ID {NurseId} from the database.", nurseId);
            var examinationResults = await _examinationResultRepository.GetExaminationResultsByNurseIdAsync(nurseId);
            return _mapper.Map<List<ExaminationResultDto>>(examinationResults);
        }
        public async Task<List<ExaminationResultDto>> GetExaminationResultsByMedicalRecordIdAsync(int medicalRecordId)
        {
            _logger
                .LogDebug("Retrieving examination results for Medical Record with ID {MedicalRecordId} from the database.", medicalRecordId);
            var examinationResults = await _examinationResultRepository.GetExaminationResultsByMedicalRecordIdAsync(medicalRecordId);
            return _mapper.Map<List<ExaminationResultDto>>(examinationResults);
        }
        public async Task<ExaminationResultDto> UpdateExaminationResultAsync(UpdateExaminationResultDto updateExaminationResultDto,int id)
        {
            _logger.LogInformation(
                "Updating examination result with ID {ExaminationResultId}",
                id);
            var examinationResult = await _examinationResultRepository.GetExaminationResultAsync(id);
            if (examinationResult == null)
            {
                _logger
                    .LogWarning("Examination result with ID {ExaminationResultId} not found.", id);
                throw new KeyNotFoundException($"Examination result with ID {id} not found");
            }
            examinationResult.Update
                (
                updateExaminationResultDto.TestType,
                updateExaminationResultDto.ResultValue,
                updateExaminationResultDto.Unit,
                updateExaminationResultDto.NormalRange,
                updateExaminationResultDto.Note);
            _logger
                .LogInformation("Examination result with ID {ExaminationResultId} updated successfully.", id);
            examinationResult = await _examinationResultRepository.UpdateExaminationResultAsync(examinationResult);
            return _mapper.Map<ExaminationResultDto>(examinationResult);
        }

        public async Task DeleteExaminationResultAsync(int id)
        {
            _logger
                .LogInformation("Deleting examination result with ID {ExaminationResultId} from the database.", id);
            var examinationResult = await _examinationResultRepository.GetExaminationResultAsync(id);
            if (examinationResult == null)
            {
                _logger
                    .LogWarning("Examination result with ID {ExaminationResultId} not found.", id);
                throw new KeyNotFoundException($"Examination result with ID {id} not found");
            }
            await _examinationResultRepository.DeleteExaminationResultAsync(examinationResult);
            _logger.LogInformation(
    "Examination result with ID {ExaminationResultId} deleted successfully",
    id);
        }
    }
}