using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;

namespace clinicAPIsSystem.IRepositoryService.IUserRepository
{
    public interface IPatientRepository
    {
        public Task<( Patient patient,bool addUserRes,bool addPasswordRes,bool addRoleRes)> CreatePatientAsync(Patient patient,string password);
        public Task<List<Patient>> GetAllPatientsAsync ();
        public Task<Patient?> GetPatientAsync(int id);
        public Task<Patient> UpdatePatientAsync( Patient patient);
        public Task DeletePatientAsync(Patient patient);

    }
}
