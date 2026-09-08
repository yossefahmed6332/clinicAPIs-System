using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;

namespace clinicAPIsSystem.Models
{
    public class Appointment
    {
        public int Id { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public Patient? Patient { get; private set; }

        public int PatientId { get; private set; }

        public Nurse? Nurse { get; private set; }

        public int NurseId { get; private set; }

        public Doctor? Doctor { get; private set; }

        public int DoctorId { get; private set; }

        public AppointmentStatus Status { get; private set; }


        // Constructor
        public Appointment(
            DateTime startDate,
            DateTime endDate,
            int patientId,
            int nurseId,
            int doctorId)
        {
            StartDate = startDate;
            EndDate = endDate;
            PatientId = patientId;
            NurseId = nurseId;
            DoctorId = doctorId;
            Status = AppointmentStatus.Pending;
        }


        // Required by EF Core
        public Appointment()
        {
        }


        public void Update(
            DateTime startDate,
            DateTime endDate,
            AppointmentStatus status,
            int nurseId,
            int patientId,
            int doctorId)
        {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            NurseId = nurseId;
            PatientId = patientId;
            DoctorId = doctorId;
        }
    }
}