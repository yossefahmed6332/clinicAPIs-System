using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.PrescriptionDTOs
{
    public class UpdatePrescriptionDto
    {
        [Required, MaxLength(100)]
        public string MedicalName { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Dosage { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Frequency { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Duration { get; set; } = null!;
        [Required, MaxLength(500)]
        public string Instructions { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Diagnosis { get; set; } = null!;
 


    }
}
