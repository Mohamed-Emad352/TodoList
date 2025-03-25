using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(todoItem => todoItem.Title).HasMaxLength(50).IsRequired();
        builder
            .HasOne(t => t.Parent)
            .WithMany() // ?
            .HasForeignKey(t => t.ParentId) // ?
            .OnDelete(DeleteBehavior.Cascade); // ? parent or child?
    }
}