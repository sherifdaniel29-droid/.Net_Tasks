using System;
namespace StudentManagement.Api.Models;
public class StudentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}
