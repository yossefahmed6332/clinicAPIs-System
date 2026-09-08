using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.PrescriptionDTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string MedicalName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Instructions { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public DateTime Date { get; set; }
        public int DoctorId { get; set; }
        public int medicalRecordId { get; set; }


    }
}
