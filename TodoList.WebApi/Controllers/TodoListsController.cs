﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.Features.TodoItems.Commands.DeleteTodoItem;
using TodoList.Application.Features.TodoItems.Commands.MarkTodoItemCommand;
using TodoList.Application.Features.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.Features.TodoItems.Queries.GetTodoItemById;
using TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

namespace TodoList.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoListsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand command, CancellationToken token)
    {
        var todoItemId = await _mediator.Send(command, token);

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTodoItems([FromQuery] GetTodoItemsQuery query, CancellationToken token)
    {
        var todoItems = await _mediator.Send(query, token);

        return Ok(todoItems);
    }

    [HttpDelete]
    [Route("/api/[controller]/{id:guid}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id, CancellationToken token)
    {
        var deleteTodoItemCommand = new DeleteTodoItemCommand
        {
            Id = id
        };
        await _mediator.Send(deleteTodoItemCommand, token);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodoItem([FromBody] UpdateTodoItemCommand command, CancellationToken token)
    {
        var todoItemId = await _mediator.Send(command, token);

        return Ok(todoItemId);
    }

    [HttpGet]
    [Route("/api/[controller]/${id:guid}")]
    public async Task<IActionResult> GetTodoItemById(Guid id, CancellationToken token)
    {
        var query = new GetTodoItemByIdQuery
        {
            Id = id
        };

        var todoItem = await _mediator.Send(query);
        return Ok(todoItem);
    }

    [HttpPut]
    [Route("/api/[controller]/mark")]
    public async Task<IActionResult> MarkTodoItem([FromBody] MarkTodoItemCommand command, CancellationToken token)
    {
        var isCompleted = await _mediator.Send(command, token);

        return Ok(isCompleted);
    }
}