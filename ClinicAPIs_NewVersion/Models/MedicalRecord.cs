using clinicAPIsSystem.Models.User;

namespace clinicAPIsSystem.Models
{
    public class MedicalRecord
    {
        public int Id { get; private set; }

        public decimal Height { get; private set; }

        public decimal Weight { get; private set; }

        public string? BloodType { get; private set; }

        public Patient? Patient { get; private set; }

        public int PatientId { get; private set; }

        public ICollection<Prescription> Prescriptions { get; private set; }
            = new HashSet<Prescription>();

        public ICollection<ExaminationResult> ExaminationResults { get; private set; }
            = new HashSet<ExaminationResult>();

        public ICollection<VitalSigns> VitalSigns { get; private set; }
            = new HashSet<VitalSigns>();


        // Constructor
        public MedicalRecord(
            decimal height,
            decimal weight,
            string bloodType,
            int patientId)
        {
            Height = height;
            Weight = weight;
            BloodType = bloodType;
            PatientId = patientId;
        }


        // Required by EF Core
        public MedicalRecord()
        {
        }


        // Update
        public void Update(
            decimal height,
            decimal weight,
            string bloodType,
            int patientId)
        {
            Height = height;
            Weight = weight;
            BloodType = bloodType;
            PatientId = patientId;
        }
    }
}