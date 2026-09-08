using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO
{
    public abstract class UpdateApplicationUserDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MaxLength(50)]
        public string UserName { get; set; } = null!;
        [Required, MaxLength(30)]
        public string PhoneNumber { get; set; } = null!;
        [Required,MinLength(6)]
        public string Password { get; set; } = null!;



    }
}
