using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public interface IDepartmentService
    {
        List<Department> GetAll();
        Department? GetById(int id);
        Department Add(string name);
        (Department? Result, string? Error) Update(int id, string name);
        bool Delete(int id);
        List<DepartmentStatisticsDto> GetStatistics();
        (DepartmentStatisticsDto? Highest, DepartmentStatisticsDto? Lowest) GetHighestLowest();
    }
}