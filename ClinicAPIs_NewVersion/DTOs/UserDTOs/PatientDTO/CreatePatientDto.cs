using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO;
using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.PatientDTO
{
    public class CreatePatientDto:CreateApplicationUserDto
    {
        [Required]
        public CreateMedicalRecordDto createMedicalRecordDto { set; get; } = null!;

    
    }
}
