using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IStudentService
    {
        Task<List<StudentDetailsDto>> GetAllAsync();
        Task<StudentDetailsDto?> GetByIdAsync(int id);
        Task<(StudentDetailsDto? Result, string? Error)> AddAsync(StudentCreateDto dto);
        Task<(StudentDetailsDto? Result, string? Error)> UpdateAsync(int id, StudentUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<StudentDetailsDto>> SearchAsync(string text);
        Task<List<StudentDetailsDto>> GetByAgeRangeAsync(int minAge, int maxAge);
    }
}