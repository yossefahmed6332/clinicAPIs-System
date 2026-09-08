using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository
{
    public interface IAccountantRepository
    {
        public Task<(Accountant accountant,bool addUserRes , bool addPasswordRes , bool addRoleRes)> CreateAccountantAsync(Accountant accountant,string password);
        public Task<List<Accountant>> GetAllAccountantsAsync();
        public Task<Accountant?> GetAccountantAsync(int id); 
        public Task<Accountant> UpdateAccountantAsync(Accountant accountant);
        public Task DeleteAccountantAsync(Accountant accountant);
    }
}
