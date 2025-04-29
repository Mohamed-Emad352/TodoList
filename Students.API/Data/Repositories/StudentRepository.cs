using System.Reflection;
using MongoDB.Driver;
using Students.API.Interfaces;
using Students.API.Models;

namespace Students.API.Data.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly IMongoCollection<Student> _studentsCollection;

    public StudentRepository()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("School");
        _studentsCollection = database.GetCollection<Student>("Students");
    }

    public async Task<List<Student>> GetAllAsync() =>
        await _studentsCollection.Find(_ => true).ToListAsync();

    public async Task<Student?> GetByIdAsync(string id) =>
        await _studentsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Student student) =>
        await _studentsCollection.InsertOneAsync(student);

    public async Task UpdateAsync(string id, Dictionary<string, object> fields)
    {
        if (fields.Count == 0)
            throw new ArgumentException("Fields to update cannot be empty", nameof(fields));

        var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
        var updatedFields = fields.Select(f => Builders<Student>.Update.Set(f.Key, f.Value)).ToArray();
        var update = Builders<Student>.Update.Combine(updatedFields);
        
        await _studentsCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAsync(string id) =>
        await _studentsCollection.DeleteOneAsync(s => s.Id == id);
}