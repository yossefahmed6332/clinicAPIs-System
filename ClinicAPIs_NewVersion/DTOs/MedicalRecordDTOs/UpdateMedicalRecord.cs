using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.MedicalRecordDTOs
{
    public class UpdateMedicalRecordDto
    {
        [Required, Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Height { get;  set; }
        [Required, Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Weight { get;  set; }
        [Required, MaxLength(10)]
        public string BloodType { get; set; } = null!; 
        [Required, Range(1, int.MaxValue)]
        public int PatientId { get;  set; }

 
    }
}
