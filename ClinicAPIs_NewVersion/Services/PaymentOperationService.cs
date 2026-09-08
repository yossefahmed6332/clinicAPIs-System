using clinicAPIsSystem.IService;
using AutoMapper;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.DTOs.PaymentOperationDTOs;
using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.Service
{
    public class PaymentOperationService:IPaymentOperationService
    {
        private readonly IPaymentOperationRepository _paymentOperationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentOperationService> _logger;
        public PaymentOperationService(IPaymentOperationRepository paymentOperationRepository, IMapper mapper, ILogger<PaymentOperationService> logger)
        {
            _paymentOperationRepository = paymentOperationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentOperationDto> CreatePaymentOperationAsync(CreatePaymentOperationDto createPaymentOperationDto)
        {
            _logger.LogInformation(
                "Creating payment operation for Patient ID {PatientId} and Accountant ID {AccountantId}",
                createPaymentOperationDto.PatientId,
                createPaymentOperationDto.AccountantId);
            var paymentOperation = 
                new PaymentOperation(
                createPaymentOperationDto.Amount, 
                createPaymentOperationDto.Date,
                createPaymentOperationDto.OperationType,
                createPaymentOperationDto.OperationStatus,
                createPaymentOperationDto.PatientId,
                createPaymentOperationDto.AccountantId,
                createPaymentOperationDto.PaymentMethod);

            var createdPaymentOperation = await _paymentOperationRepository.CreatePaymentOperationAsync(paymentOperation);
            _logger.LogInformation(
                "Payment operation created with ID {PaymentOperationId} for Patient ID {PatientId} and Accountant ID {AccountantId}",
                createdPaymentOperation.Id,
                createdPaymentOperation.PatientId,
                createdPaymentOperation.AccountantId);
            return _mapper.Map<PaymentOperationDto>(createdPaymentOperation);
        }

        public async Task<List<PaymentOperationDto>> GetAllPaymentOperationsAsync()
        {
            _logger
                .LogDebug("Retrieving all payment operations from the repository.");
            var paymentOperations = await _paymentOperationRepository.GetAllPaymentOperationsAsync();
            return _mapper.Map<List<PaymentOperationDto>>(paymentOperations);
        }

        public async Task<PaymentOperationDto> GetPaymentOperationAsync(int id)
        {
            _logger
                .LogDebug("Retrieving payment operation with ID {PaymentOperationId} from the repository.", id);
            var paymentOperation = await _paymentOperationRepository.GetPaymentOperationAsync(id);
            if (paymentOperation == null)
            {
                _logger
                    .LogWarning("Payment operation with ID {PaymentOperationId} not found.", id);
                throw new KeyNotFoundException($"Payment operation with ID {id} not found.");
            }
            return _mapper.Map<PaymentOperationDto>(paymentOperation);
        }

        public async Task<List<PaymentOperationDto>> GetPaymentOperationsByPatientIdAsync(int patientId)
        {
            _logger
                .LogDebug("Retrieving payment operations for Patient ID {PatientId} from the repository.", patientId);
            var paymentOperations = await _paymentOperationRepository.GetPaymentOperationsByPatientIdAsync(patientId);
            return _mapper.Map<List<PaymentOperationDto>>(paymentOperations);
        }

        public async Task<List<PaymentOperationDto>> GetPaymentOperationsByAccountantIdAsync(int accountantId)
        {
            _logger
                .LogDebug("Retrieving payment operations for Accountant ID {AccountantId} from the repository.", accountantId);
            var paymentOperations = await _paymentOperationRepository.GetPaymentOperationsByAccountantIdAsync(accountantId);
            return _mapper.Map<List<PaymentOperationDto>>(paymentOperations);
        }

    }
}
