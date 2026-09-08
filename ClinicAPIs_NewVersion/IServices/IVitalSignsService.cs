using clinicAPIsSystem.DTOs.VitalSignsDTOs;

namespace clinicAPIsSystem.IService
{
    public interface IVitalSignsService
    {
        public Task<VitalSignsDto> CreateVitalSignsAsync(CreateVitalSignsDto createVitalSignsDto);
        public Task<List<VitalSignsDto>> GetAllVitalSignsAsync();
        public Task<VitalSignsDto> GetVitalSignsAsync(int id);
        public Task<List<VitalSignsDto>> GetVitalSignsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<List<VitalSignsDto>> GetVitalSignsByNurseIdAsync(int nurseId);
        public Task<VitalSignsDto> UpdateVitalSignsAsync(UpdateVitalSignsDto updateVitalSignsDto,int id);
        public Task DeleteVitalSignsAsync(int id);

    }
}
