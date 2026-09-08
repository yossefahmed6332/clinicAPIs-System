using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.DTOs.AppointmentDTOs
{
    public class AppointmentDto
    {
        public int Id { get;  set; }
        public DateTime StartDate { get;  set; }
        public DateTime EndDate {get;  set; }
        public int PatientId { get;  set; }
        public int NurseId { get;  set; }
        public int DoctorId { get;  set; }
        public AppointmentStatus Status { get;  set; } 

    }
}
