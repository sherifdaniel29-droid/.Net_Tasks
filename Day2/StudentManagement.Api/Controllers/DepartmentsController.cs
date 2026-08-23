using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Services;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_departmentService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var department = _departmentService.GetById(id);
            if (department == null) return NotFound($"Department with id {id} was not found.");
            return Ok(department);
        }

        [HttpPost]
        public IActionResult Add([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required.");
            var department = _departmentService.Add(name);
            return Ok(department);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string name)
        {
            var (result, error) = _departmentService.Update(id, name);
            if (error == "NotFound") return NotFound($"Department with id {id} was not found.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _departmentService.Delete(id);
            if (!deleted) return NotFound($"Department with id {id} was not found.");
            return Ok($"Department with id {id} was deleted.");
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics() => Ok(_departmentService.GetStatistics());

        [HttpGet("highest-lowest")]
        public IActionResult GetHighestLowest()
        {
            var (highest, lowest) = _departmentService.GetHighestLowest();
            return Ok(new { Highest = highest, Lowest = lowest });
        }
    }
}