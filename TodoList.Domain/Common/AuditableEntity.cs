namespace TodoList.Domain.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}