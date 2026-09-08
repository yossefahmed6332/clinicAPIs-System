using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO;
using System.ComponentModel.DataAnnotations;
namespace clinicAPIsSystem.DTOs.UserDTOs.PatientDTO
{
    public class PatientDto: ApplicationUserDto
    {
        [Required,Range(1,int.MaxValue)]
        public int MedicalRecordId { get; private set; }


    }
    
    }

