using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IMedicalRecordRepository
    {
        public Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord);
        public Task<List<MedicalRecord>> GetAllMedicalRecordsAsync();
        public Task<MedicalRecord?> GetMedicalRecordAsync(int id);
        public Task<MedicalRecord?> GetMedicalRecordByPatientIdAsync(int patientId);
        public Task<MedicalRecord> UpdateMedicalRecordAsync(MedicalRecord medicalRecord);

    }
}
