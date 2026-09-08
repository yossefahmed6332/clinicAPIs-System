
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;

namespace clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices
{
    public interface IDoctorService
    {
        public Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto);
        public Task<List<DoctorDto>> GetAllDoctorsAsync();
        public Task<DoctorDto> GetDoctorAsync(int id);
        public Task<DoctorDto> UpdateDoctorAsync(UpdateDoctorDto updateDoctorDto, int id);
    }
}
