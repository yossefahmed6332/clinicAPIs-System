using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;

namespace clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices
{
    public interface INurseService
    {
        public Task<NurseDto> CreateNurseAsync(CreateNurseDto createNurseDto);
        public Task<List<NurseDto>> GetAllNursesAsync();
        public Task<NurseDto> GetNurseAsync(int id); 
        public Task <NurseDto> UpdateNurseAsync (UpdateNurseDto updateNurseDto, int id);

    }
}
