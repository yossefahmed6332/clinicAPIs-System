namespace clinicAPIsSystem.Models.User
{
    public class Patient: ApplicationUser
    {
        public ICollection<Appointment> Appointments { get; protected set; } = new HashSet<Appointment>();
        public MedicalRecord? MedicalRecord { get; protected set; }
        public int MedicalRecordId { get; protected set; } 

        public ICollection<PaymentOperation> PaymentOperations { get; protected set; } = new HashSet<PaymentOperation>();
        public Patient(int medicalRecordId, string firstName, string lastName, string userName, string email,string phoneNumber): base(firstName, lastName, userName, email, phoneNumber)
        {
            
            MedicalRecordId = medicalRecordId;
        }

        public Patient()
        {
        }
        public void UpdatePatientInfo(string firstName, string lastName, string userName, string email ,string phoneNumber)
        {
            UpdateUserInfo(firstName, lastName, userName, email, phoneNumber);
        }


    }
}
