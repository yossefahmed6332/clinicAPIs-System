namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO
{
    public abstract class ApplicationUserDto
    {
        public int Id { get;  set; }
        public string UserName { get; set; } = null!; 
        public string Email { get;  set; } = null!;
        public string FirstName { get;  set; } = null!;
        public string LastName { get;  set; } = null!;
        public string PhoneNumber { get;  set; } = null!;


    }
}
