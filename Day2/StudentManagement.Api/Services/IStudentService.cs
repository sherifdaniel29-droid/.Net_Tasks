using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public interface IStudentService
    {
        List<StudentDetailsDto> GetAll();
        StudentDetailsDto? GetById(int id);
        (StudentDetailsDto? Result, string? Error) Add(StudentCreateDto dto);
        (StudentDetailsDto? Result, string? Error) Update(int id, StudentUpdateDto dto);
        bool Delete(int id);
        List<StudentDetailsDto> SearchByName(string name);
        List<StudentDetailsDto> GetByAge();

    }
}