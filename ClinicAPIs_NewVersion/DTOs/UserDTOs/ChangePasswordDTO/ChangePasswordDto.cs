using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ChangePasswordDTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required, MinLength(6)]
        public string Password { get; set; } = null!; 
    }
}
