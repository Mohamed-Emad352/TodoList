using Students.API.Data.Repositories;
using Students.API.Interfaces;
using Students.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/students", async (IStudentRepository studentRepository) =>
{
    var students = await studentRepository.GetAllAsync();
    return Results.Ok(students);
});

app.MapPost("/students", async (IStudentRepository studentRepository, Student student) =>
{
    await studentRepository.CreateAsync(student);
    return Results.Created($"/students/{student.Id}", student);
});

app.MapGet("/students/{id}", async (IStudentRepository studentRepository, string id) =>
{
    var student = await studentRepository.GetByIdAsync(id);
    return student is not null ? Results.Ok(student) : Results.NotFound();
});

app.MapPut("/students/{id}", async (IStudentRepository studentRepository, string id, Dictionary<string, object> fields) =>
{
    await studentRepository.UpdateAsync(id, fields);
    return Results.NoContent();
});

app.MapDelete("/students/{id}", async (IStudentRepository studentRepository, string id) =>
{
    await studentRepository.DeleteAsync(id);
    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
