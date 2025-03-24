using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Common;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Auth;

namespace TodoList.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public ApplicationDbContext() : this(new DbContextOptions<ApplicationDbContext>()) { }
    
    public DbSet<TodoItem> TodoItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ApplyGlobalSoftDeleteQueryFilter(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=todolists;Username=postgres;Password=123;");
        }
    }

    private void ApplyGlobalSoftDeleteQueryFilter(ModelBuilder builder)
    {
        var auditableEntities = builder.Model.GetEntityTypes()
            .Where(entity => typeof(AuditableEntity).IsAssignableFrom(entity.ClrType));
        
        foreach (var entity in auditableEntities)
        {
            var method = typeof(ApplicationDbContext)
                .GetMethod(nameof(ApplySoftDeleteQueryFilter), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(entity.ClrType);
        
            method.Invoke(null, [builder]);
        }
    }

    private static void ApplySoftDeleteQueryFilter<TEntity>(ModelBuilder builder)
     where TEntity : AuditableEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(entity => !entity.DeletedAt.HasValue);
    }
}