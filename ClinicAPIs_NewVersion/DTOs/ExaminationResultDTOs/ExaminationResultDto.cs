namespace clinicAPIsSystem.DTOs.ExaminationResultDTOs
{
    public class ExaminationResultDto
    {
        public int Id { get;  set; }
        public string TestType { get;  set; } = null!;
        public string ResultValue { get;  set; }= null!;
        public string Unit { get;  set; } = null!;
        public string NormalRange { get;  set; }=null!;
        public string Note { get;  set; } = null!;
        public DateTime RecordedAt { get;  set; } 
        public int NurseId { get;  set; }
        public int MedicalRecordId { get;  set; }



    }
    }

