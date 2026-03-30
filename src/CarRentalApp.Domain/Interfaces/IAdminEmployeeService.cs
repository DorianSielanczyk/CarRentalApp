namespace CarRentalApp.Domain.Interfaces
{
    public interface IAdminEmployeeService
    {
        Task<List<AdminEmployeeListItem>> LoadEmployeesAsync();
        Task<AdminEmployeeOperationResult> UpdateEmployeeStatusAsync(string userId, string status);
        Task<AdminEmployeeOperationResult> DeleteEmployeeAsync(string userId, string currentUserId);
    }

    public sealed class AdminEmployeeListItem
    {
        public string UserId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "Employee";
    }

    public sealed class AdminEmployeeOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
