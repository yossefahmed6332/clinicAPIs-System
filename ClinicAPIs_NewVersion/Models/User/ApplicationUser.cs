using Microsoft.AspNetCore.Identity;
namespace clinicAPIsSystem.Models.User
{
    public abstract class ApplicationUser:IdentityUser<int>
    {
        public string FirstName { get; protected set; } = null!;
        public string LastName { get; protected set; } = null!;

        public ApplicationUser (string firstName, string secondName,string userName,string email,string phoneNumber)
        {
            FirstName = firstName;
            LastName = secondName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public ApplicationUser()
        {
        }

        public void UpdateUserInfo(string firstName, string lastName, string userName, string email ,string phoneNumber)
        {
            FirstName=firstName;
            LastName=lastName;
            UserName=userName;
            Email = email;
            PhoneNumber = phoneNumber;

        }


    }
}
