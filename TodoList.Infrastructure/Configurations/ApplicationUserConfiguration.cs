using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Infrastructure.Auth;

namespace TodoList.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(50).IsRequired();
    }
}