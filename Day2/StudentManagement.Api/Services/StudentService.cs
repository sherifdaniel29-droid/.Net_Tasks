using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public class StudentService : IStudentService
    {
        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, Name = "Ahmed Ali", Age = 20, DepartmentId = 1 },
            new Student { Id = 2, Name = "Sara Mohamed", Age = 22, DepartmentId = 2 },
            new Student { Id = 3, Name = "Ahmed Hassan", Age = 19, DepartmentId = 1 },
            new Student { Id = 4, Name = "Mona Adel", Age = 21, DepartmentId = 3 }
        };

        private readonly IDepartmentService _departmentService;

        public StudentService(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        private StudentDetailsDto ToDetailsDto(Student s)
        {
            var department = _departmentService.GetById(s.DepartmentId);
            return new StudentDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                DepartmentName = department?.Name ?? "Unknown"
            };
        }

        public List<StudentDetailsDto> GetAll()
        {
            return _students.Select(ToDetailsDto).ToList();
        }

        public StudentDetailsDto? GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return student == null ? null : ToDetailsDto(student);
        }

        public (StudentDetailsDto? Result, string? Error) Add(StudentCreateDto dto)
        {
            var department = _departmentService.GetById(dto.DepartmentId);
            if (department == null)
                return (null, $"Department with id {dto.DepartmentId} does not exist.");

            int newId = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            var student = new Student
            {
                Id = newId,
                Name = dto.Name,
                Age = dto.Age,
                DepartmentId = dto.DepartmentId
            };
            _students.Add(student);

            return (ToDetailsDto(student), null);
        }

        public (StudentDetailsDto? Result, string? Error) Update(int id, StudentUpdateDto dto)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return (null, "NotFound");

            var department = _departmentService.GetById(dto.DepartmentId);
            if (department == null)
                return (null, $"Department with id {dto.DepartmentId} does not exist.");

            student.Name = dto.Name;
            student.Age = dto.Age;
            student.DepartmentId = dto.DepartmentId;

            return (ToDetailsDto(student), null);
        }

        public bool Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return false;

            _students.Remove(student);
            return true;
        }

        public List<StudentDetailsDto> SearchByName(string name)
        {
            return _students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(ToDetailsDto)
                .ToList();
        }

        public List<StudentDetailsDto> GetByAge()
        {
            return _students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .Select(ToDetailsDto)
                .ToList();
        }
    }
}