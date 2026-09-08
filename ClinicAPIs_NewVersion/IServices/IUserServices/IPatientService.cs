using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;

namespace clinicAPIsSystem.IServices.IUserServices
{
    public interface IPatientService
    {
        public Task<PatientDto> CreatePatientAsync(
            CreatePatientDto createPatientDto
            );
        public Task<List<PatientDto>> GetAllPatientsAsync();
        public Task<PatientDto?> GetPatientAsync(int id);
        public Task<PatientDto> UpdatePatientAsync(UpdatePatientDto updatePatientDto,int id);
    }
}
