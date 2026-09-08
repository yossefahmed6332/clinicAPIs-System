using clinicAPIsSystem.DTOs.PrescriptionDTOs;

namespace clinicAPIsSystem.IService
{
    public interface IPrescriptionService
    {
        public Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createPrescriptionDto);
        public Task<List<PrescriptionDto>> GetAllPrescriptionsAsync();
        public Task<PrescriptionDto> GetPrescriptionAsync(int id);
        public Task<List<PrescriptionDto>> GetPrescriptionsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<List<PrescriptionDto>> GetPrescriptionsByDoctorIdAsync(int doctorId);
        public Task<PrescriptionDto> UpdatePrescriptionAsync(UpdatePrescriptionDto updatePrescriptionDto,int id);
        public Task DeletePrescriptionAsync(int id);
    }
}
