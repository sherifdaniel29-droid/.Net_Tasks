using StudentManagement.Api.Models;
using StudentManagement.Api.Dtos;

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private static List<Department> _departments = new List<Department>
        {
            new Department { Id = 1, Name = "IT" },
            new Department { Id = 2, Name = "HR" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "Sales" }
        };

        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, Name = "Ahmed Ali", Age = 20, DepartmentId = 1 },
            new Student { Id = 2, Name = "Sara Mohamed", Age = 22, DepartmentId = 2 },
            new Student { Id = 3, Name = "Ahmed Hassan", Age = 19, DepartmentId = 1 },
            new Student { Id = 4, Name = "Mona Adel", Age = 21, DepartmentId = 3 }
        };

        public List<Department> GetAll() => _departments;

        public Department? GetById(int id) =>
            _departments.FirstOrDefault(d => d.Id == id);

        public Department Add(string name)
        {
            int newId = _departments.Any() ? _departments.Max(d => d.Id) + 1 : 1;
            var department = new Department { Id = newId, Name = name };
            _departments.Add(department);
            return department;
        }

        public (Department? Result, string? Error) Update(int id, string name)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
                return (null, "NotFound");

            department.Name = name;
            return (department, null);
        }

        public bool Delete(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department == null) return false;

            _departments.Remove(department);
            return true;
        }

        public List<DepartmentStatisticsDto> GetStatistics()
        {
            return _departments.Select(d =>
            {
                var studentsInDept = _students.Where(s => s.DepartmentId == d.Id).ToList();

                return new DepartmentStatisticsDto
                {
                    DepartmentName = d.Name,
                    StudentCount = studentsInDept.Count,
                    AverageAge = studentsInDept.Any() ? studentsInDept.Average(s => s.Age) : 0,
                    OldestAge = studentsInDept.Any() ? studentsInDept.Max(s => s.Age) : 0,
                    YoungestAge = studentsInDept.Any() ? studentsInDept.Min(s => s.Age) : 0
                };
            }).ToList();
        }

        public (DepartmentStatisticsDto? Highest, DepartmentStatisticsDto? Lowest) GetHighestLowest()
        {
            var stats = GetStatistics();
            if (!stats.Any()) return (null, null);

            var highest = stats.OrderByDescending(s => s.StudentCount).First();
            var lowest = stats.OrderBy(s => s.StudentCount).First();

            return (highest, lowest);
        }
    }
}