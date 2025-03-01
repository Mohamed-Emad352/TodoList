using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;

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
        Console.WriteLine(dbContext);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand command, CancellationToken token)
    {
        var todoItemId = await _mediator.Send(command, token);

        return Ok();
    }
}