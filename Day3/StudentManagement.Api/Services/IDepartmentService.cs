using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<(Department? Result, string? Error)> AddAsync(string name);
        Task<(Department? Result, string? Error)> UpdateAsync(int id, string name);
        Task<bool> DeleteAsync(int id);
        Task<List<DepartmentStatisticsDto>> GetStatisticsAsync();
        Task<(List<DepartmentStatisticsDto> Highest, List<DepartmentStatisticsDto> Lowest)> GetHighestLowestAsync();
    }
}