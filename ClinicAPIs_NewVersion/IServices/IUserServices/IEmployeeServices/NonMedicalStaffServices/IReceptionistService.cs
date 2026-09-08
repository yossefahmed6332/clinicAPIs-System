using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ReceptionistDTO;

namespace clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices
{
    public interface IReceptionistService
    {
        public Task<ReceptionistDto> CreateReceptionistAsync(
            CreateReceptionistDto createReceptionistDto
            );

        public Task<ReceptionistDto> GetReceptionistAsync(int id);

        public Task<List<ReceptionistDto>> GetAllReceptionistsAsync();

        public Task<ReceptionistDto> UpdateReceptionistAsync(
            UpdateReceptionistDto updateReceptionistDto,
            int id);
    }
}