using clinicAPIsSystem.Data;
using clinicAPIsSystem.Data.Seeder;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.IRepositoryService.IUserRepository;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.IMedicalStaffRepository;
using clinicAPIsSystem.IRepositoryService.IUserRepository.IEmployeeRepository.INonMedicalStaffRepository;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.IMedicalStaffServices;
using clinicAPIsSystem.IServices.IUserServices.IEmployeeServices.NonMedicalStaffServices;
using clinicAPIsSystem.IUserRepositories;
using clinicAPIsSystem.Models.User;
using clinicAPIsSystem.Repositories.UserRepository;
using clinicAPIsSystem.RepositoryService;
using clinicAPIsSystem.RepositoryService.UserRepository;
using clinicAPIsSystem.RepositoryService.UserRepository.EmployeeRepository;
using clinicAPIsSystem.RepositoryService.UserRepository.MedicalStaffRepository;
using clinicAPIsSystem.RepositoryService.UserRepository.NonMedicalStaffRepository;
using clinicAPIsSystem.Service;
using clinicAPIsSystem.Services.AuthServices;
using clinicAPIsSystem.Services.UserServices;
using clinicAPIsSystem.Services.UserServices.EmployeeServices;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.MedicalStaffServices;
using clinicAPIsSystem.Services.UserServices.EmployeeServices.NonMedicalStaffServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Controllers

builder.Services.AddControllers();

#endregion

#region Database

builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    // Password Settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // User Settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ClinicDbContext>()
.AddDefaultTokenProviders();

#endregion


#region Dependency Injection
//depency injection for repository services
//depenct injection for users repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICleanerRepository, CleanerRepository>();
builder.Services.AddScoped<IAccountantRepository, AccountantRepository>();
builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<INurseRepository, NurseRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
//depenct injection for other repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IExaminationResultRepository, ExaminationResultRepository>();
builder.Services.AddScoped<IFinancialReportRepository, FinancialReportRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IPaymentOperationRepository, PaymentOperationRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IVitalSignsRepository, VitalSignsRepository>();
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<AdminSeeder>();

//dependency injection for services
//dependency injection for non-user services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IExaminationResultService, ExaminationResultService>();
builder.Services.AddScoped<IFinancialReportService, FinancialReportService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IPaymentOperationService, PaymentOperationService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IVitalSignsService, VitalSignsService>();
//dependency injection for user services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICleanerService, CleanerService>();
builder.Services.AddScoped<IAccountantService, AccountantService>();
builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
//depency injection for auth service 
builder.Services.AddScoped<ILoginService, LoginService>();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMappingProfile>();
});
#endregion



#region JWT Key Validation

var jwtKey = builder.Configuration["JWT:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT:Key is missing from configuration. Please set it in appsettings.json or environment variables.");
}

#endregion

#region Authentication (JWT)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Validate JWT
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // JWT Configuration
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],

            // Secret Key
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            // Remove default 5-minute tolerance
            ClockSkew = TimeSpan.Zero
        };
    });

#endregion

#region Authorization

builder.Services.AddAuthorization();

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? new[] { "http://localhost:3000" }
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

#region Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion


#region OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

#endregion

var app = builder.Build();
#region swagger 

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion 

#region Exception handler 
app.UseExceptionHandler();
#endregion

#region Seeder

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleSeeder = services.GetRequiredService<RoleSeeder>();
    var adminSeeder = services.GetRequiredService<AdminSeeder>();

    await roleSeeder.SeedAsync();
    await adminSeeder.SeedAsync();
}

#endregion

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion