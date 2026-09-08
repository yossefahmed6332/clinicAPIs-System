using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace clinicAPIsSystem.DTOs.VitalSignsDTOs
{
    public class CreateVitalSignsDto
    {

        [Required, Range(0, 300)]
        public int BloodPressureSystolic { get;  set; }
        [Required, Range(0, 300)]
        public int BloodPressureDiastolic { get;  set; }
        [Required, Range(0, 300)]   
        public int HeartRate { get;  set; }
        [Required, Range(0, 45)]
        public decimal Temperature { get;  set; }
        [Required, Range(0, 100)]
        public decimal OxygenSaturation { get;  set; }
        [Required]
        public DateTime RecordedAt { get;  set; }= DateTime.UtcNow;
        [Required, Range(1, int.MaxValue)]
        public int NurseId { get;   set; }

        [Required,Range(1,int.MaxValue)]
        public int MedicalRecordId { get;  set; }




    }
}
