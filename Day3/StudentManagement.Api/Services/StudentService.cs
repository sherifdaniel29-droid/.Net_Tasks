using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static readonly Func<IQueryable<Student>, IQueryable<StudentDetailsDto>> ProjectToDto =
            query => query.Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                DepartmentName = s.Department != null ? s.Department.Name : "Unknown"
            });

        public async Task<List<StudentDetailsDto>> GetAllAsync()
        {
            return await ProjectToDto(_context.Students.Include(s => s.Department)).ToListAsync();
        }

        public async Task<StudentDetailsDto?> GetByIdAsync(int id)
        {
            return await ProjectToDto(_context.Students.Include(s => s.Department).Where(s => s.Id == id))
                .FirstOrDefaultAsync();
        }

        public async Task<(StudentDetailsDto? Result, string? Error)> AddAsync(StudentCreateDto dto)
        {
            var error = Validate(dto.Name, dto.Age);
            if (error != null) return (null, error);

            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
                return (null, $"Department with id {dto.DepartmentId} does not exist.");

            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                DepartmentId = dto.DepartmentId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var result = await GetByIdAsync(student.Id);
            return (result, null);
        }

        public async Task<(StudentDetailsDto? Result, string? Error)> UpdateAsync(int id, StudentUpdateDto dto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return (null, "NotFound");

            var error = Validate(dto.Name, dto.Age);
            if (error != null) return (null, error);

            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
                return (null, $"Department with id {dto.DepartmentId} does not exist.");

            student.Name = dto.Name;
            student.Age = dto.Age;
            student.DepartmentId = dto.DepartmentId;

            await _context.SaveChangesAsync();

            var result = await GetByIdAsync(id);
            return (result, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentDetailsDto>> SearchAsync(string text)
        {
            return await ProjectToDto(
                _context.Students
                    .Include(s => s.Department)
                    .Where(s => s.Name.Contains(text) ||
                                (s.Department != null && s.Department.Name.Contains(text)))
            ).ToListAsync();
        }

        public async Task<List<StudentDetailsDto>> GetByAgeRangeAsync(int minAge, int maxAge)
        {
            return await ProjectToDto(
                _context.Students
                    .Include(s => s.Department)
                    .Where(s => s.Age >= minAge && s.Age <= maxAge)
                    .OrderBy(s => s.Age)
            ).ToListAsync();
        }

        private static string? Validate(string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name is required.";
            if (age < 18 || age > 60)
                return "Age must be between 18 and 60.";
            return null;
        }
    }
}