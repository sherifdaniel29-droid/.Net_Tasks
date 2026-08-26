using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("CONNECTION STRING = " + builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
