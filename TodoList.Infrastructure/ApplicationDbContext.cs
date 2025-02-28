using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}