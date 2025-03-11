using Microsoft.EntityFrameworkCore;
using Moq;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;
using TodoList.Domain.Entities;

namespace Tests.ApplicationUnitTests.Features.TodoItems.CreateTodoItem;

[TestFixture]
public class CreateTodoItemCommandHandlerTests
{
    private CreateTodoItemCommandHandler _handler;
    private Mock<IApplicationDbContext> _mockDbContext;

    [SetUp]
    public void Setup()
    {
        _mockDbContext = new Mock<IApplicationDbContext>();
        _handler = new CreateTodoItemCommandHandler(_mockDbContext.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreateNewTodoItemAndReturnId()
    {
        var mockTodoItems = new Mock<DbSet<TodoItem>>();
        _mockDbContext.Setup(c => c.TodoItems).Returns(mockTodoItems.Object);

        var command = new CreateTodoItemCommand
        {
            Title = "Test",
            Description = "Test Description",
        };

        var result =  await _handler.Handle(command, CancellationToken.None);
        
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task Handle_TitleExceedMaxLength_ShouldThrowValidationException()
    {
        var command = new CreateTodoItemCommand
        {
            Title = new string('A', 51),
            Description = "Test Description",
        };
        
        var validator = new CreateTodoItemCommandValidator();
        
        var validationResults = await validator.ValidateAsync(command);

        Assert.That(validationResults.IsValid, Is.False);
        Assert.That(validationResults.Errors,  Has.Some.Property("PropertyName").EqualTo("Title"));
    }
}