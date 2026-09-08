using clinicAPIsSystem.Data;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace clinicAPIsSystem.RepositoryService
{
    public class VitalSignsRepository:IVitalSignsRepository
    {
        private readonly ClinicDbContext _context; 
        public VitalSignsRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<VitalSigns> CreateVitalSignsAsync(VitalSigns vitalSigns)
        {
            await _context.TVitalSigns.AddAsync(vitalSigns);
            await _context.SaveChangesAsync();
            return vitalSigns;
        }

        public async Task<List<VitalSigns>> GetAllVitalSignsAsync()
        {
            return await _context.TVitalSigns.ToListAsync();
        }
        
        public async Task<VitalSigns?> GetVitalSignsAsync(int id)
        {
            return await _context.TVitalSigns.FirstOrDefaultAsync(vt=>vt.Id==id); 

        }

        public async Task<List<VitalSigns>> GetVitalSignsByMedicalRecordIdAsync(int medicalRecordId)
        {
            return await _context.TVitalSigns.Where(vt=>vt.MedicalRecordId==medicalRecordId).ToListAsync();

        }

        public async Task<List<VitalSigns>> GetVitalSignsByNurseIdAsync(int nurseId)
        {
            return await _context.TVitalSigns.Where(vt=>vt.NurseId==nurseId).ToListAsync();
        }
        public async Task<VitalSigns> UpdateVitalSignsAsync (VitalSigns vitalSigns)
        {
              _context.TVitalSigns.Update(vitalSigns);
            await _context.SaveChangesAsync();
            return vitalSigns; 
        }
        public async Task DeleteVitalSignsAsync(VitalSigns vitalSigns)
        {
             _context.TVitalSigns.Remove(vitalSigns);
            await _context.SaveChangesAsync();

        }

    }
}
