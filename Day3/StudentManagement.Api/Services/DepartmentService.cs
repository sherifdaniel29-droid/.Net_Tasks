using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<(Department? Result, string? Error)> AddAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (null, "Department name is required.");

            var duplicate = await _context.Departments
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());
            if (duplicate)
                return (null, $"A department named '{name}' already exists.");

            var department = new Department { Name = name };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return (department, null);
        }

        public async Task<(Department? Result, string? Error)> UpdateAsync(int id, string name)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null) return (null, "NotFound");

            if (string.IsNullOrWhiteSpace(name))
                return (null, "Department name is required.");

            var duplicate = await _context.Departments
                .AnyAsync(d => d.Id != id && d.Name.ToLower() == name.ToLower());
            if (duplicate)
                return (null, $"A department named '{name}' already exists.");

            department.Name = name;
            await _context.SaveChangesAsync();

            return (department, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null) return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DepartmentStatisticsDto>> GetStatisticsAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Students)
                .ToListAsync();

            return departments.Select(d => new DepartmentStatisticsDto
            {
                DepartmentName = d.Name,
                StudentCount = d.Students.Count,
                AverageAge = d.Students.Any() ? d.Students.Average(s => s.Age) : 0,
                OldestAge = d.Students.Any() ? d.Students.Max(s => s.Age) : 0,
                YoungestAge = d.Students.Any() ? d.Students.Min(s => s.Age) : 0
            }).ToList();
        }

        public async Task<(List<DepartmentStatisticsDto> Highest, List<DepartmentStatisticsDto> Lowest)> GetHighestLowestAsync()
        {
            var stats = await GetStatisticsAsync();
            if (!stats.Any()) return (new List<DepartmentStatisticsDto>(), new List<DepartmentStatisticsDto>());

            int maxCount = stats.Max(s => s.StudentCount);
            int minCount = stats.Min(s => s.StudentCount);

            var highest = stats.Where(s => s.StudentCount == maxCount).ToList();
            var lowest = stats.Where(s => s.StudentCount == minCount).ToList();

            return (highest, lowest);
        }
    }
}