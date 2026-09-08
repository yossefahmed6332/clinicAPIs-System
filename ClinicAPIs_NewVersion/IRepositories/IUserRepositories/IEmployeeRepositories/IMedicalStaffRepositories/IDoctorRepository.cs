using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository
{
    public interface IDoctorRepository
    {
        public Task<(Doctor doctor, bool addUserRes, bool addPasswordRes, bool addRoleRes)> CreateDoctorAsync(Doctor doctor, string password);
        public Task<List<Doctor>> GetAllDoctorsAsync();
        public Task<Doctor?> GetDoctorAsync(int id);
        public Task<Doctor> UpdateDoctorAsync(Doctor doctor);
    }
}
