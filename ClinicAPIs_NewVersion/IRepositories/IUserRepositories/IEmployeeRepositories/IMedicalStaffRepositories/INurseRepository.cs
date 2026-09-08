using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository
{
    public interface INurseRepository
    {
        public Task<(Nurse nurse, bool addUserRes, bool addPasswordRes, bool addRoleRes)> CreateNurseAsync(Nurse nurse, string password);
        public Task<List<Nurse>> GetAllNursesAsync();
        public Task<Nurse?> GetNurseAsync(int id);
        public Task<Nurse> UpdateNurseAsync(Nurse nurse);
        public Task DeleteNurseAsync(Nurse nurse);
    }
}
