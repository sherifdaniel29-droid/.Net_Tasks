using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services;
namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("welcome")]
        public IActionResult Welcome() => Ok("Welcome to Student Management API");

        [HttpGet]
        public IActionResult GetAll() => Ok(_studentService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
            {
                return NotFound($"Student with id {id} was not found");
            }
            return Ok(student);
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            return Ok(_studentService.SearchByName(name));
        }
        [HttpGet("filter-by-age")]
        public IActionResult FilterByAge()
        {
            return Ok(_studentService.GetByAge());
        }
        [HttpPost]
        public IActionResult Add([FromBody] StudentCreateDto newStudent)
        {
            var (result, error) = _studentService.Add(newStudent);
            if (error != null) return BadRequest(error);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StudentUpdateDto updatedStudent)
        {
            var (result, error) = _studentService.Update(id, updatedStudent);
            if (error == "NotFound") return NotFound($"Student with id {id} was not found.");
            if (error != null) return BadRequest(error);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        { 
            var deleted = _studentService.Delete(id);
            if (!deleted) return NotFound($"Student with id {id} was not found.");
            return Ok($"Student with id {id} was deleted.");
        }
    }
}
