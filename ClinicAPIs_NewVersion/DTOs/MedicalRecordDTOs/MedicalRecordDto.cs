using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.DTOs.VitalSignsDTOs;
using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
namespace clinicAPIsSystem.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordDto
    {
        public int Id { get;  set; }
        public double Height { get;  set; }
        public double Weight { get;  set; }
        public string? BloodType { get;  set; }
        public int PatientId { get;  set; }
        public ICollection<PrescriptionDto>? Prescriptions { get;  set; }
        public ICollection<VitalSignsDto>? VitalSigns { get;  set; }
        public ICollection<ExaminationResultDto>? ExaminationResults { get;  set; }


    }
}
