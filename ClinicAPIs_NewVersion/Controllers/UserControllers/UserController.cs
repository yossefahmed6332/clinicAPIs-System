using clinicAPIsSystem.DTOs.UserDTOs.ChangePasswordDTO;
using clinicAPIsSystem.IServices.IUserServices;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public UserController(
            IUserService userService,
            IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            string cacheKey = $"user:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out var user))
            {
                user = await _userService.GetUserAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, user, cacheOptions);
            }

            return Ok(user);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            string cacheKey = $"user:email:{email}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out var user))
            {
                user = await _userService.GetUserByEmailAsync(email);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, user, cacheOptions);
            }

            return Ok(user);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            string cacheKey = $"user:username:{username}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out var user))
            {
                user = await _userService.GetUserByUsernameAsync(username);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, user, cacheOptions);
            }

            return Ok(user);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("exists/phone/{phoneNumber}")]
        public async Task<IActionResult> PhoneNumberExists(string phoneNumber)
        {
            string cacheKey = $"user:exists:phone:{phoneNumber}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out bool result))
            {
                result = await _userService.PhoneNumberExistsAsync(phoneNumber);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, result, cacheOptions);
            }

            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("exists/email/{email}")]
        public async Task<IActionResult> EmailExists(string email)
        {
            string cacheKey = $"user:exists:email:{email}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out bool result))
            {
                result = await _userService.EmailExistsAsync(email);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, result, cacheOptions);
            }

            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("exists/username/{username}")]
        public async Task<IActionResult> UsernameExists(string username)
        {
            string cacheKey = $"user:exists:username:{username}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out bool result))
            {
                result = await _userService.UsernameExistsAsync(username);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, result, cacheOptions);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto request)
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
                return Unauthorized();

            if (!int.TryParse(idClaim.Value, out int id))
                return Unauthorized();

            await _userService.ChangePasswordASync(
                id,
                request.CurrentPassword,
                request.Password);

            // User data may have changed
            _cache.Remove($"user:{id}");

            return NoContent();
        }

        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);

            // Remove user by ID
            _cache.Remove($"user:{id}");

            return Ok();
        }
    }
}
