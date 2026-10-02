using clinicAPIsSystem.DTOs.PaymentOperationDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOperationController : ControllerBase
    {
        private readonly IPaymentOperationService _paymentOperationService;
        private readonly IMemoryCache _cache;

        public PaymentOperationController(
            IPaymentOperationService paymentOperationService,
            IMemoryCache cache)
        {
            _paymentOperationService = paymentOperationService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Accountant)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentOperation(
            [FromBody] CreatePaymentOperationDto createPaymentOperationDto)
        {
            var createdPaymentOperation =
                await _paymentOperationService.CreatePaymentOperationAsync(
                    createPaymentOperationDto);

            _cache.Remove("allPaymentOperations");

            return CreatedAtAction(
                nameof(GetPaymentOperation),
                new { id = createdPaymentOperation.Id },
                createdPaymentOperation);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Accountant)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllPaymentOperations()
        {
            const string cacheKey = "allPaymentOperations";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PaymentOperationDto>? paymentOperations))
            {
                paymentOperations =
                    await _paymentOperationService
                        .GetAllPaymentOperationsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, paymentOperations, cacheOptions);
            }

            return Ok(paymentOperations);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Accountant)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentOperation(int id)
        {
            string cacheKey = $"paymentOperation:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out PaymentOperationDto? paymentOperation))
            {
                paymentOperation =
                    await _paymentOperationService
                        .GetPaymentOperationAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, paymentOperation, cacheOptions);
            }

            return Ok(paymentOperation);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Accountant)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPaymentOperationsByPatientId(
            int patientId)
        {
            string cacheKey = $"paymentOperations:patient:{patientId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PaymentOperationDto>? paymentOperations))
            {
                paymentOperations =
                    await _paymentOperationService
                        .GetPaymentOperationsByPatientIdAsync(patientId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, paymentOperations, cacheOptions);
            }

            return Ok(paymentOperations);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Accountant)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [HttpGet("accountant/{accountantId}")]
        public async Task<IActionResult> GetPaymentOperationsByAccountantId(
            int accountantId)
        {
            string cacheKey = $"paymentOperations:accountant:{accountantId}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<PaymentOperationDto>? paymentOperations))
            {
                paymentOperations =
                    await _paymentOperationService
                        .GetPaymentOperationsByAccountantIdAsync(accountantId);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, paymentOperations, cacheOptions);
            }

            return Ok(paymentOperations);
        }
    }
}