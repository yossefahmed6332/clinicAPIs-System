namespace clinicAPIsSystem.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Scheduled,
        Confirmed,
        Completed,
        Cancelled,
        NoShow
    }
    public enum UserRole
    {
        Manager,
        Doctor,
        Nurse,
        Receptionist,
        Cleaner,
        Accountant,
        Patient,
        Admin
    }
    public enum OperationType
    {
        Payment, 
        Refund, 
        Salary,
        Expense
    }
    public enum OperationStatus
    {
        Pending,
        Completed,
        Failed
    }
    public enum PaymentMethod
    {
        Cash,
        Card
    }
}
