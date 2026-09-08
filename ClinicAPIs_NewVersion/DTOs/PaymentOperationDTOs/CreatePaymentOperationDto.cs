using System.ComponentModel.DataAnnotations;
using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.DTOs.PaymentOperationDTOs
{
    public class CreatePaymentOperationDto
    {
        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get;  set; }
        [Required]
        public DateTime Date { get;  set; } = DateTime.UtcNow;
        [Required, EnumDataType(typeof(OperationType))]
        public OperationType OperationType { get;  set; }
        [Required, EnumDataType(typeof(OperationStatus))]
        public OperationStatus OperationStatus { get;  set; }
        [Range(1, int.MaxValue)]
        public int? PatientId { get;  set; }
        [Range(1, int.MaxValue)]
        public int? AccountantId { get;  set; }
        [Required, EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get;  set; }


    }
}
