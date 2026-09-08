namespace clinicAPIsSystem.Models.User
{
    public class Admin:ApplicationUser
    {
       
        public Admin(string firstName, string lastName, string userName, string email, string phoneNumber): base(firstName, lastName, userName, email, phoneNumber)
        {

        }

        //for EF Core
        public Admin()
        {
        }

        public void UpdateAdminInfo(string firstName, string lastName, string userName, string email, string phoneNumber)
        {
            UpdateUserInfo(firstName, lastName, userName, email, phoneNumber);
        }

    }
}
