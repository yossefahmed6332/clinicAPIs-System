using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository
{
    public interface IReceptionistRepository
    {
        public Task<(Receptionist receptionist, bool addUserRes, bool addPasswordRes, bool addRoleRes)> CreateReceptionistAsync(Receptionist receptionist, string password);
        public Task<List<Receptionist>> GetAllReceptionistsAsync();
        public Task<Receptionist?> GetReceptionistAsync(int id);
        public Task<Receptionist> UpdateReceptionistAsync(Receptionist receptionist);
        public Task DeleteReceptionistAsync(Receptionist receptionist);
    }
}
