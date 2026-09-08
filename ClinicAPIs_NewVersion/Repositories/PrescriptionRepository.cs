using clinicAPIsSystem.Data;
using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.RepositoryService
{
    public class PrescriptionRepository:IPrescriptionRepository
    {
        private readonly ClinicDbContext _context; 
        public PrescriptionRepository(ClinicDbContext context)
        {
            _context = context;
        }
     
        public async Task<Prescription> CreatePrescriptionAsync(Prescription prescription)
        {
           await _context.TPrescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync(); 
            return prescription;
        }
        public async Task<List<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.TPrescriptions.ToListAsync();
        }
        public async Task<Prescription?> GetPrescriptionAsync(int id)
        {
            return await _context.TPrescriptions.FindAsync(id);
        }
        public async Task<List<Prescription>> GetPrescriptionsByMedicalRecordIdAsync(int medicalRecordId)
        {
            return await _context.TPrescriptions
                .Where(p => p.MedicalRecordId == medicalRecordId)
                .ToListAsync();
        }

        public async Task<List<Prescription>> GetPrescriptionsByDoctorIdAsync(int doctorId)
        {
            return await _context.TPrescriptions.Where(prs=>prs.DoctorId==doctorId).ToListAsync();
        }

        public async Task<Prescription> UpdatePrescriptionAsync(Prescription prescription)
        {
             _context.TPrescriptions.Update(prescription);
            await _context.SaveChangesAsync();

            return prescription;
        }

        public async Task DeletePrescriptionAsync(Prescription prescription)
        {
             _context.TPrescriptions.Remove(prescription);
            await _context.SaveChangesAsync();


        }
    }
}
