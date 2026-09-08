using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.DTOs.PaymentOperationDTOs
{
    public class PaymentOperationDto
    {
        public int Id { get;  set; }
        public decimal Amount { get;  set; }
        public DateTime Date { get;  set; }
        public OperationType OperationType { get;  set; }
        public OperationStatus OperationStatus { get;  set; }
        public int? PatientId { get;  set; }
        public int? AccountantId { get;  set; }
        public PaymentMethod PaymentMethod { get;  set; }


    }
}
