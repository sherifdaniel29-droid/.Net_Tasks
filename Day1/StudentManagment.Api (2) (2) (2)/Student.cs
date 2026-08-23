namespace StudentManagement.Api.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
