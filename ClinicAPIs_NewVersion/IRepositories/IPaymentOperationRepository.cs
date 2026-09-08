using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IPaymentOperationRepository
    {
        public Task<PaymentOperation> CreatePaymentOperationAsync(PaymentOperation paymentOperation);
        public Task<List<PaymentOperation>> GetAllPaymentOperationsAsync();
        public Task<PaymentOperation?> GetPaymentOperationAsync(int id);
        public Task<List<PaymentOperation>> GetPaymentOperationsByPatientIdAsync(int patientId);
        public Task<List<PaymentOperation>> GetPaymentOperationsByAccountantIdAsync(int accountantId);
    }
}
