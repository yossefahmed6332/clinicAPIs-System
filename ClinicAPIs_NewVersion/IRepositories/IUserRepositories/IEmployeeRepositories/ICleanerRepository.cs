using clinicAPIsSystem.Models.User.Employee;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository
{
    public interface ICleanerRepository
    {
        Task<(
            Cleaner cleaner,
            bool addUserRes,
            bool addPasswordRes,
            bool addRoleRes)> CreateCleanerAsync(
                Cleaner cleaner,
                string password);

        Task<List<Cleaner>> GetAllCleanersAsync();

        Task<Cleaner?> GetCleanerAsync(int id);

        Task<Cleaner> UpdateCleanerAsync(Cleaner cleaner);

        Task DeleteCleanerAsync(Cleaner cleaner);
    }
}