namespace TodoList.Domain.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastModified { get; set; }
    public DateTimeOffset? Deleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}