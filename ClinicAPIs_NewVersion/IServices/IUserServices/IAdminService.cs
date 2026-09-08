using clinicAPIsSystem.DTOs.UserDTOs.AdminDTO;
using clinicAPIsSystem.Models.User;

namespace clinicAPIsSystem.IServices.IUserServices
{
    public interface IAdminService
    {
        public Task<AdminDto> CreateAdminAsync(
            CreateAdminDto admin
            );
        public Task<List<AdminDto>> GetAllAdminsAsync(); 
        public Task<AdminDto> GetAdminAsync(int id);
        public Task<AdminDto> UpdateAdminAsync(UpdateAdminDto admin,int id);
    }
}
