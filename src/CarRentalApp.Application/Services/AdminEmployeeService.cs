using CarRentalApp.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Application.Services
{
    public class AdminEmployeeService : IAdminEmployeeService
    {
        private static readonly string[] ManagedRoles = ["User", "Worker", "Admin"];
        private const string ProtectedAdminEmail = "admin@carrental.com";

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminEmployeeService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<AdminEmployeeListItem>> LoadEmployeesAsync()
        {
            var users = await _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.Email)
                .ToListAsync();

            var clients = (await _unitOfWork.Clients.GetAllAsync())
                .Where(c => !string.IsNullOrWhiteSpace(c.UserId))
                .Select(c => new { UserId = c.UserId!, c.FirstName, c.LastName })
                .ToDictionary(c => c.UserId, c => $"{c.FirstName} {c.LastName}".Trim());

            var result = new List<AdminEmployeeListItem>(users.Count);

            foreach (var user in users)
            {
                var displayName = clients.TryGetValue(user.Id, out var fullName)
                    ? fullName
                    : (user.UserName?.Split('@').FirstOrDefault() ?? "Unknown");

                var userRoles = await _userManager.GetRolesAsync(user);

                var status = userRoles.Contains("Admin", StringComparer.OrdinalIgnoreCase)
                    ? "Admin"
                    : userRoles.Contains("Worker", StringComparer.OrdinalIgnoreCase)
                        ? "Worker"
                        : "Employee";

                result.Add(new AdminEmployeeListItem
                {
                    UserId = user.Id,
                    DisplayName = displayName,
                    Email = user.Email ?? user.UserName ?? "N/A",
                    Status = status
                });
            }

            return result;
        }

        public async Task<AdminEmployeeOperationResult> UpdateEmployeeStatusAsync(string userId, string status)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "User not found." };
            }

            if (IsProtectedAdmin(user))
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "Protected admin account status cannot be changed." };
            }

            if (!TryMapStatusToRole(status, out var targetRole))
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "Invalid employee status." };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Where(r => ManagedRoles.Contains(r, StringComparer.OrdinalIgnoreCase)).ToList();

            if (rolesToRemove.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    return new AdminEmployeeOperationResult
                    {
                        Success = false,
                        Message = removeResult.Errors.FirstOrDefault()?.Description ?? "Failed to update employee status."
                    };
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, targetRole);
            if (!addResult.Succeeded)
            {
                return new AdminEmployeeOperationResult
                {
                    Success = false,
                    Message = addResult.Errors.FirstOrDefault()?.Description ?? "Failed to update employee status."
                };
            }

            return new AdminEmployeeOperationResult { Success = true, Message = "Employee status updated successfully." };
        }

        public async Task<AdminEmployeeOperationResult> DeleteEmployeeAsync(string userId, string currentUserId)
        {
            if (userId == currentUserId)
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "You cannot delete your own account." };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "User not found." };
            }

            if (IsProtectedAdmin(user))
            {
                return new AdminEmployeeOperationResult { Success = false, Message = "Protected admin account cannot be deleted." };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new AdminEmployeeOperationResult
                {
                    Success = false,
                    Message = result.Errors.FirstOrDefault()?.Description ?? "Failed to delete user."
                };
            }

            return new AdminEmployeeOperationResult { Success = true, Message = "User deleted successfully." };
        }

        private static bool TryMapStatusToRole(string status, out string role)
        {
            role = status.Trim() switch
            {
                "Employee" => "User",
                "Worker" => "Worker",
                "Admin" => "Admin",
                _ => string.Empty
            };

            return !string.IsNullOrWhiteSpace(role);
        }

        private static bool IsProtectedAdmin(IdentityUser user)
        {
            var identity = user.Email ?? user.UserName ?? string.Empty;
            return identity.Equals(ProtectedAdminEmail, StringComparison.OrdinalIgnoreCase);
        }
    }
}
