using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using clinicAPIsSystem.Models.User.Employee;

namespace clinicAPIsSystem.Data
{
    public class ClinicDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public virtual DbSet<Doctor> TDoctors { get; set; }
        public virtual DbSet<Nurse> TNurses { get; set; }
        public virtual DbSet<Accountant> TAccountants { get; set; }
        public virtual DbSet<Receptionist> TReceptionists { get; set; }
        public virtual DbSet<Manager> TManagers { get; set; }
        public virtual DbSet<Cleaner> TCleaners { get; set; }
         public virtual DbSet<Patient> TPatients { get; set; }
        public virtual DbSet<Prescription> TPrescriptions { get; set; }
        public virtual DbSet<Appointment> TAppointments { get; set; }
        public virtual DbSet<PaymentOperation> TPaymentOperations { get; set; }
        public virtual DbSet<MedicalRecord> TMedicalRecords { get; set; }
        public virtual DbSet<Admin> TAdmins { get; set; }
        public virtual DbSet<FinancialReport> TFinancialReports { get; set; }
        public virtual DbSet<ExaminationResult> TExaminationResults { get; set; }
        public virtual DbSet<VitalSigns> TVitalSigns { get; set; }


    }
}