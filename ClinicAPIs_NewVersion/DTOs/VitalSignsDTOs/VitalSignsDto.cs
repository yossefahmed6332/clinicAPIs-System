namespace clinicAPIsSystem.DTOs.VitalSignsDTOs
{
    public class VitalSignsDto
    {
        public int Id { get;  set; }
        public int BloodPressureSystolic { get;  set; }
        public int BloodPressureDiastolic { get;  set; }
        public int HeartRate { get;  set; }
        public decimal Temperature { get;  set; }
        public decimal OxygenSaturation { get;  set; }
        public DateTime RecordedAt { get;  set; }
        public int NurseId { get;  set; }
        public int PatientId { get;  set; }


    }
}
