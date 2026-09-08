using AutoMapper;

using clinicAPIsSystem.DTOs.AppointmentDTOs;
using clinicAPIsSystem.DTOs.ExaminationResultDTOs;
using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.DTOs.MedicalRecordDTOs;
using clinicAPIsSystem.DTOs.PaymentOperationDTOs;
using clinicAPIsSystem.DTOs.PrescriptionDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.AdminDTO;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.CleanerDTO;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.DoctorDTO;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.AccountantDTO;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ManagerDTO;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.ReceptionistDTO;
using clinicAPIsSystem.DTOs.UserDTOs.PatientDTO;
using clinicAPIsSystem.DTOs.VitalSignsDTOs;

using clinicAPIsSystem.Models;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Models.User.Employee;
using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Doctor
        CreateMap<Doctor, DoctorDto>();

        // Nurse
        CreateMap<Nurse, NurseDto>();

        // Accountant
        CreateMap<Accountant, AccountantDto>();

        // Receptionist
        CreateMap<Receptionist, ReceptionistDto>();

        // Manager
        CreateMap<Manager, ManagerDto>();

        // Cleaner
        CreateMap<Cleaner, CleanerDto>();

        // Patient
        CreateMap<Patient, PatientDto>();

        // Prescription
        CreateMap<Prescription, PrescriptionDto>();

        // Appointment
        CreateMap<Appointment, AppointmentDto>();

        // Payment Operation
        CreateMap<PaymentOperation, PaymentOperationDto>();

        // Medical Record
        CreateMap<MedicalRecord, MedicalRecordDto>();

        // Admin
        CreateMap<Admin, AdminDto>();

        // Financial Report
        CreateMap<FinancialReport, FinancialReportDto>();

        // Examination Result
        CreateMap<ExaminationResult, ExaminationResultDto>();

        // Vital Signs
        CreateMap<VitalSigns, VitalSignsDto>();
    }
}