using clinicAPIsSystem.DTOs.ExaminationResultDTOs;

namespace clinicAPIsSystem.IService
{
    public interface IExaminationResultService
    {
        public Task<ExaminationResultDto> CreateExaminationResultAsync(CreateExaminationResultDto createExaminationResultDto);
        public Task<List<ExaminationResultDto>> GetAllExaminationResultsAsync();
        public Task<ExaminationResultDto> GetExaminationResultAsync(int id);
        public Task<List<ExaminationResultDto>> GetExaminationResultsByNurseIdAsync(int nurseId);
        public Task<List<ExaminationResultDto>> GetExaminationResultsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<ExaminationResultDto> UpdateExaminationResultAsync(UpdateExaminationResultDto updateExaminationResultDto,int id);
        public Task DeleteExaminationResultAsync (int id);
    }
}
