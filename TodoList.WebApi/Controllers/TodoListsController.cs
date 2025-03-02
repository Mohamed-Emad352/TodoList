using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.Features.TodoItems.Commands.DeleteTodoItem;
using TodoList.Application.Features.TodoItems.Commands.MarkTodoItemCommand;
using TodoList.Application.Features.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

namespace TodoList.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public TodoListsController(IMediator mediator, IMapper mapper, IApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _dbContext = dbContext;
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

    [HttpPut]
    [Route("/api/[controller]/mark")]
    public async Task<IActionResult> MarkTodoItem([FromBody] MarkTodoItemCommand command, CancellationToken token)
    {
        var isCompleted = await _mediator.Send(command, token);

        return Ok(isCompleted);
    }
}