using Students.API.Models;

namespace Students.API.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(string id);
    Task CreateAsync(Student student);
    Task UpdateAsync(string id, Dictionary<string, object> fields);
    Task DeleteAsync(string id);
}