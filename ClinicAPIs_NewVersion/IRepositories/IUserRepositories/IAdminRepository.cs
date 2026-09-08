using clinicAPIsSystem.Models.User;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository
{
    public interface IAdminRepository
    {
        public  Task<(Admin admin,bool addUserRes,bool addPasswordRes, bool addRoleRes)> CreateAdminAsync(
        Admin admin,
        string password);
        public Task<List<Admin>> GetAllAdminsAsync();
        public Task<Admin?> GetAdminAsync(int id);
        public Task<Admin> UpdateAdminAsync(Admin admin);
        public Task DeleteAdminAsync(Admin admin);

    }
}
