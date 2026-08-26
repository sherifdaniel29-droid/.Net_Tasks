using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Services;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _studentService.GetAllAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string text) =>
            Ok(await _studentService.SearchAsync(text));

        [HttpGet("filter-by-age")]
        public async Task<IActionResult> FilterByAge() =>
            Ok(await _studentService.GetByAgeRangeAsync(18, 22));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound($"Student with id {id} was not found.");
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentCreateDto dto)
        {
            var (result, error) = await _studentService.AddAsync(dto);
            if (error != null) return BadRequest(error);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
        {
            var (result, error) = await _studentService.UpdateAsync(id, dto);
            if (error == "NotFound") return NotFound($"Student with id {id} was not found.");
            if (error != null) return BadRequest(error);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted) return NotFound($"Student with id {id} was not found.");
            return Ok($"Student with id {id} was deleted.");
        }
    }
}