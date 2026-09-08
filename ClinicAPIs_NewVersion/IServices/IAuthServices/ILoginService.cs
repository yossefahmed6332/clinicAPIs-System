using clinicAPIsSystem.DTOs.AuthDTO;

namespace clinicAPIsSystem.IServices.IUserServices
{
    public interface ILoginService
    {
        public Task<string> LoginAsync(LoginDto loginDto) ;
    }
}
