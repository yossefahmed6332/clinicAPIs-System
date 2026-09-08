using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.PrescriptionDTOs
{
    public class CreatePrescriptionDto
    {
        [Required,MaxLength(100)]
        public string MedicalName { get;  set; } = null!;
        [Required,MaxLength(100)]
        public string Dosage { get;  set; } = null!;
        [Required,MaxLength(100)]
        public string  Frequency { get;  set; }= null!;
        [Required,MaxLength(100)]
        public string Duration { get;  set; }=null!;
        [Required, MaxLength(500)]
        public string Instructions { get; set; } = null!;
        [Required,MaxLength(100)]
        public string Diagnosis { get;  set; } = null!;
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required,Range(1, int.MaxValue)]
        public int DoctorId { get;  set; }
        [Required, Range(1, int.MaxValue)]
        public int medicalRecordId { get; set; }



    }
}
