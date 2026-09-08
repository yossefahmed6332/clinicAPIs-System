using System.ComponentModel.DataAnnotations;
using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.DTOs.AppointmentDTOs
{
    public class UpdateAppointmentDto
    {
        [Required]
        public DateTime StartDate { get;  set; }
        [Required]
        public DateTime EndDate { get;  set; }
        [Required, Range(1, int.MaxValue)]
        public int PatientId { get;  set; }
        [Required, Range(1, int.MaxValue)]
        public int NurseId { get;  set; }
        [Required, Range(1, int.MaxValue)]
        public int DoctorId { get;  set; }
        [Required, EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus Status { get;  set; }


    }
}
