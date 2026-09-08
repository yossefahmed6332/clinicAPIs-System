using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IExaminationResultRepository
    {
        public Task<ExaminationResult> CreateExaminationResultAsync(ExaminationResult examinationResult);
        public Task<List<ExaminationResult>> GetAllExaminationResultsAsync();
        public Task<ExaminationResult?> GetExaminationResultAsync(int id);
        public Task<List<ExaminationResult>> GetExaminationResultsByNurseIdAsync(int nurseId);
        public Task<List<ExaminationResult>> GetExaminationResultsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<ExaminationResult> UpdateExaminationResultAsync(ExaminationResult examinationResult);
        public Task DeleteExaminationResultAsync(ExaminationResult examinationResult);
        
    }
}
