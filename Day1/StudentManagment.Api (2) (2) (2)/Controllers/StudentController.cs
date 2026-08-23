using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;

namespace StudentManagment.Api.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private static List<Student> _students = new List<Student>
{
            new Student {  Id= 1,Name= "Ahmed Ali", Age= 20, DepartmentName="Computer Science" },
            new Student { Id= 2,Name = "Sara Mohamed", Age = 22, DepartmentName="Information Systems" },
            new Student { Id=3, Name = "Ahmed Hassan", Age = 19, DepartmentName="IT" },
            new Student { Id= 4, Name ="Mona Adel", Age = 21, DepartmentName="Computer Science" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_students);
        }
        [HttpGet("Welcome")]
        public IActionResult Welcome()
        {
            return Ok("Welcome to Student Managment API");
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with id {id} was not found");
            }
            return Ok(student);
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            var results = _students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(results);
        }
        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            var results = _students
                .Where(s => s.Age >= 18 && s.Age <= 22)
                .OrderBy(s => s.Age)
                .ToList();

            return Ok(results);
        }
        [HttpPost]
        public IActionResult Add([FromBody] StudentCreateDto newStudent)
        {
            int newId = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            var student = new Student
            {
                Id = newId,
                Name = newStudent.Name,
                Age = newStudent.Age,
                DepartmentName = newStudent.DepartmentName
            };

            _students.Add(student);

            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StudentCreateDto updatedStudent)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound($"Student with id {id} was not found.");

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.DepartmentName = updatedStudent.DepartmentName;

            return Ok(student);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound($"Student with id {id} was not found.");

            _students.Remove(student);

            return Ok($"Student with id {id} was deleted.");
        }
    }
}
