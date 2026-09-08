using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Data;
using clinicAPIsSystem.Models;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.RepositoryService
{
    public class PaymentOperationRepository:IPaymentOperationRepository
    {
        private readonly ClinicDbContext _context; 
        public PaymentOperationRepository(ClinicDbContext context)
        {
            _context = context;
        } 

        public async Task<PaymentOperation> CreatePaymentOperationAsync(PaymentOperation paymentOperation)
        {
           await _context.TPaymentOperations.AddAsync(paymentOperation);
            await _context.SaveChangesAsync();
            return paymentOperation;
        }

        public async Task<List<PaymentOperation>> GetAllPaymentOperationsAsync()
        {
            return await _context.TPaymentOperations.ToListAsync();
        }

        public async Task<PaymentOperation?> GetPaymentOperationAsync(int id)
        {
            return await _context.TPaymentOperations.FindAsync(id);
        }
        public async Task<List<PaymentOperation>> GetPaymentOperationsByPatientIdAsync(int patientId)
        {
            return await _context.TPaymentOperations.Where(po => po.PatientId == patientId).ToListAsync();
        }

        public async Task<List<PaymentOperation>> GetPaymentOperationsByAccountantIdAsync(int accountantId)
        {
            return await _context.TPaymentOperations.Where(po => po.AccountantId == accountantId).ToListAsync();
        }



    }
}
