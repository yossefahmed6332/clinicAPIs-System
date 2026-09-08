using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

namespace clinicAPIsSystem.Models
{
    public class PaymentOperation
    {
        public int Id { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime Date { get; private set; }

        public OperationType OperationType { get; private set; }

        public OperationStatus Status { get; private set; }

        public Patient? Patient { get; private set; }

        public int? PatientId { get; private set; }

        public Accountant? Accountant { get; private set; }

        public int? AccountantId { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }


        // Constructor
        public PaymentOperation(
            decimal amount,
            DateTime date,
            OperationType operationType,
            OperationStatus status,
            int? patientId,
            int? accountantId,
            PaymentMethod paymentMethod)
        {
            Amount = amount;
            Date = date;
            OperationType = operationType;
            Status = status;
            PatientId = patientId;
            AccountantId = accountantId;
            PaymentMethod = paymentMethod;
        }


        // Required by EF Core
        public PaymentOperation()
        {
        }


        // Update
        public void Update(
            decimal amount,
            DateTime date,
            OperationType operationType,
            OperationStatus status,
            int? patientId,
            int? accountantId,
            PaymentMethod paymentMethod)
        {
            Amount = amount;
            Date = date;
            OperationType = operationType;
            Status = status;
            PatientId = patientId;
            AccountantId = accountantId;
            PaymentMethod = paymentMethod;
        }
    }
}