using clinicAPIsSystem.Models;
using Microsoft.Identity.Client;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IPrescriptionRepository
    {
        public Task<Prescription> CreatePrescriptionAsync(Prescription prescription);
        public Task<List<Prescription>> GetAllPrescriptionsAsync();
        public Task<Prescription?> GetPrescriptionAsync(int id);
        public Task<List<Prescription>> GetPrescriptionsByMedicalRecordIdAsync(int medicalRecordId);
        public Task<List<Prescription>> GetPrescriptionsByDoctorIdAsync(int doctorId);
        public Task<Prescription> UpdatePrescriptionAsync(Prescription prescription);
        public Task DeletePrescriptionAsync(Prescription prescription);   
    }
}
