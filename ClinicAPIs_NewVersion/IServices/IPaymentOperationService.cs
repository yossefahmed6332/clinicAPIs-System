using clinicAPIsSystem.DTOs.PaymentOperationDTOs;

namespace clinicAPIsSystem.IService
{
    public interface IPaymentOperationService
    {
        public Task<PaymentOperationDto> CreatePaymentOperationAsync(CreatePaymentOperationDto createPaymentOperationDto);
        public Task<List<PaymentOperationDto>> GetAllPaymentOperationsAsync();
        public Task<PaymentOperationDto> GetPaymentOperationAsync(int id);
        public Task<List<PaymentOperationDto>> GetPaymentOperationsByPatientIdAsync(int patientId);
        public Task<List<PaymentOperationDto>> GetPaymentOperationsByAccountantIdAsync(int accountantId);

    }
}
