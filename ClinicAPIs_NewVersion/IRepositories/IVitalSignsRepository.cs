using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IVitalSignsRepository
    {
        public Task<VitalSigns> CreateVitalSignsAsync(VitalSigns vitalSigns);
        public Task<List<VitalSigns>> GetAllVitalSignsAsync();
        public Task<VitalSigns?> GetVitalSignsAsync(int id);
        public Task<List<VitalSigns>> GetVitalSignsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<List<VitalSigns>> GetVitalSignsByNurseIdAsync(int nurseId);

        public Task<VitalSigns> UpdateVitalSignsAsync(VitalSigns vitalSigns);
        public Task DeleteVitalSignsAsync(VitalSigns vitalSigns);
    }
}
