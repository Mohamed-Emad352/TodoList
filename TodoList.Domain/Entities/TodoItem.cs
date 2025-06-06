﻿using TodoList.Domain.Common;

namespace TodoList.Domain.Entities;

public class TodoItem : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public Guid? ParentId { get; set; }
    public TodoItem? Parent { get; set; }
}