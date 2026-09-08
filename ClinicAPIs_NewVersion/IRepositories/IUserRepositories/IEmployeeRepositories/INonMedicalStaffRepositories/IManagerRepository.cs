using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository
{
    public interface IManagerRepository
    {
        public Task<(Manager manager , bool addUserRes , bool addPasswordRes , bool addRoleRes)> CreateManagerAsync(Manager manager, string password );
        public Task<List<Manager>> GetAllManagersAsync();
        public Task<Manager?> GetManagerAsync(int id);
        public Task<Manager> UpdateManagerAsync(Manager manager);
        public Task DeleteManagerAsync(Manager manager);
    }
}
