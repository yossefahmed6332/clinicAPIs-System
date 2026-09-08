using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Data;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.RepositoryService
{
    public class ExaminationResultRepository: IExaminationResultRepository
    {
        private readonly ClinicDbContext _context;
        public ExaminationResultRepository(ClinicDbContext context)
        {
            _context = context;
        }
        public async Task<ExaminationResult> CreateExaminationResultAsync(ExaminationResult examinationResult)
        {
            await _context.TExaminationResults.AddAsync(examinationResult);
            await _context.SaveChangesAsync();
            return examinationResult;
        }
        public async Task<List<ExaminationResult>> GetAllExaminationResultsAsync()
        {
            return await _context.TExaminationResults.ToListAsync();
        }
        public async Task<ExaminationResult?> GetExaminationResultAsync(int id)
        {
            return await _context.TExaminationResults.FindAsync(id);
        }
        public async Task<List<ExaminationResult>> GetExaminationResultsByNurseIdAsync(int nurseId)
        {
            return await _context.TExaminationResults.Where(er => er.NurseId == nurseId).ToListAsync();
        }
        public async Task<List<ExaminationResult>> GetExaminationResultsByMedicalRecordIdAsync  (int medicalRecordId)
        {
            return await _context.TExaminationResults.Where(er => er.MedicalRecordId == medicalRecordId).ToListAsync();
        }
        public async Task<ExaminationResult> UpdateExaminationResultAsync(ExaminationResult examinationResult)
        {
            _context.TExaminationResults.Update(examinationResult);
            await _context.SaveChangesAsync();
            return examinationResult;
        }
        public async Task DeleteExaminationResultAsync(ExaminationResult examinationResult)
        {
            _context.TExaminationResults.Remove(examinationResult);
            await _context.SaveChangesAsync();
        }
    }
}
