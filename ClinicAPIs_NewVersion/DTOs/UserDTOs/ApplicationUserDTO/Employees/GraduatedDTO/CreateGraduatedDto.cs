using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO
{
    public abstract class CreateGraduatedDto:CreateEmployeeDto
    {
        [Required, MaxLength(100)]
        public string Degree { get; protected set; } = null!; 
            [Required, MaxLength(100)]
        public string University { get; protected set; } = null!; 
        [Required]
        public int YearsOfExperience { get; protected set; }
        [Required,Range(1900,2600)]
        public int GraduationYear { get; protected set; }
        [Required, MaxLength(100)]
        public string License { get; protected set; }=null!;



    }
}
