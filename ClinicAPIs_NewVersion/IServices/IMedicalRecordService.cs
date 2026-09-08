using clinicAPIsSystem.DTOs.MedicalRecordDTOs;

namespace clinicAPIsSystem.IService
{
    public interface IMedicalRecordService
    {
        public Task<MedicalRecordDto> CreateMedicalRecordAsync (CreateMedicalRecordDto createMedicalRecordDto);
        public Task<List<MedicalRecordDto>> GetAllMedicalRecordsAsync();
        public Task<MedicalRecordDto> GetMedicalRecord(int id); 
        public Task<MedicalRecordDto> GetMedicalByPatientIdRecord(int PatientId);
        public Task<MedicalRecordDto> UpdateMedicalRecordAsync(UpdateMedicalRecordDto medicalRecord, int Id);

    }
}
 