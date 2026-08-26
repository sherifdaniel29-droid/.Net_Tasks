using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Dtos;
using StudentManagement.Api.Services;

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
        public async Task<IActionResult> GetAll() => Ok(await _departmentService.GetAllAsync());

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics() => Ok(await _departmentService.GetStatisticsAsync());

        [HttpGet("highest-lowest")]
        public async Task<IActionResult> GetHighestLowest()
        {
            var (highest, lowest) = await _departmentService.GetHighestLowestAsync();
            return Ok(new { Highest = highest, Lowest = lowest });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound($"Department with id {id} was not found.");
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DepartmentDto dto)
        {
            var (result, error) = await _departmentService.AddAsync(dto.Name);
            if (error != null) return BadRequest(error);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto dto)
        {
            var (result, error) = await _departmentService.UpdateAsync(id, dto.Name);
            if (error == "NotFound") return NotFound($"Department with id {id} was not found.");
            if (error != null) return BadRequest(error);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id);
            if (!deleted) return NotFound($"Department with id {id} was not found.");
            return Ok($"Department with id {id} was deleted.");
        }
    }
}