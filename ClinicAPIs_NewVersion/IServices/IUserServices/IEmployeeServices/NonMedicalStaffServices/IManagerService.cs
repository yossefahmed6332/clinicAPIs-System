using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ManagerDTO;

namespace clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices
{
    public interface IManagerService
    {
        public Task<ManagerDto> CreateManagerAsync(CreateManagerDto createManagerDto);
        public Task<List<ManagerDto>> GetAllManagersAsync();
        public Task<ManagerDto> GetManagerAsync(int id);
        public Task<ManagerDto> UpdateManagerAsync(UpdateManagerDto updateManagerDto, int id); 

    }
}
