using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.RepositoryService
{
    public class MedicalRecordRepository:IMedicalRecordRepository
    {
        private readonly ClinicDbContext _context; 
        public MedicalRecordRepository(ClinicDbContext context)
        {
            _context = context;
        }
        public async Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            await _context.TMedicalRecords.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        public async Task<List<MedicalRecord>> GetAllMedicalRecordsAsync()
        {
            return await _context.TMedicalRecords.ToListAsync();
        }

        public async Task<MedicalRecord?> GetMedicalRecordAsync(int id)
        {
            return await _context.TMedicalRecords.FindAsync(id);
        }
 
        public async Task<MedicalRecord?> GetMedicalRecordByPatientIdAsync(int patientId)
        {
            return await _context.TMedicalRecords.FirstOrDefaultAsync(m => m.PatientId == patientId);
        }

        public async Task<MedicalRecord> UpdateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            _context.TMedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }



    }
}
