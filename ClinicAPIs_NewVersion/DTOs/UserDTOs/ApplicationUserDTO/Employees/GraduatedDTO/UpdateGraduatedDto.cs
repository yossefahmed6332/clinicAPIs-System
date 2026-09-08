using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO
{
    public abstract class UpdateGraduatedDto : UpdateEmployeeDto
    {
        [Required, MaxLength(100)]
        public string Degree { get; set; } = null!; 
        [Required, MaxLength(100)]
        public string University { get; set; } = null!; 
        [Required]
        public int YearsOfExperience { get;  set; }
        [Required, Range(1900, 2600)]
        public int GraduationYear { get;  set; }
        [Required, MaxLength(100)]
        public string License { get; set; } = null!; 



    }
}
