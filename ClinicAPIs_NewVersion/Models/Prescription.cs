using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;

namespace clinicAPIsSystem.Models
{
    public class Prescription
    {
        public int Id { get; private set; }

        public string? MedicalName { get; private set; }

        public string? Dosage { get; private set; }

        public string? Frequency { get; private set; }

        public string? Duration { get; private set; }

        public string? Instructions { get; private set; }

        public string? Diagnosis { get; private set; }

        public DateTime Date { get; private set; }

        public Doctor? Doctor { get; private set; }

        public int DoctorId { get; private set; }

        public MedicalRecord? MedicalRecord { get; private set; }

        public int MedicalRecordId { get; private set; }


        // Constructor for EF Core
        public Prescription()
        {
        }


        // Create
        public Prescription(
            string medicalName,
            string dosage,
            string frequency,
            string duration,
            string instructions,
            string diagnosis,
            DateTime date,
            int doctorId,
            int medicalRecordId)
        {
            MedicalName = medicalName;
            Dosage = dosage;
            Frequency = frequency;
            Duration = duration;
            Instructions = instructions;
            Diagnosis = diagnosis;
            Date = date;
            DoctorId = doctorId;
            MedicalRecordId = medicalRecordId;
        }


        // Update
        public void Update(
            string medicalName,
            string dosage,
            string frequency,
            string duration,
            string instructions,
            string diagnosis
            )
        {
            MedicalName = medicalName;
            Dosage = dosage;
            Frequency = frequency;
            Duration = duration;
            Instructions = instructions;
            Diagnosis = diagnosis;
        }
    }
}