using FluentValidation.Results;
using TodoList.Application.Common.Exceptions;
using NUnit.Framework;

namespace Tests.ApplicationUnitTests.Exceptions;

[TestFixture]
public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructor_ShouldInitializeEmptyErrorsDictionary()
    {
        var exception = new ValidationException();
        var errors = exception.Errors;
        
        Assert.That(errors.Keys, Is.Empty);
    }

    [Test]
    public void Constructor_WithErrors_ShouldCreateCorrectMultipleKeysDictionary()
    {
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required"),
            new ValidationFailure("Email", "Email format is invalid"),
            new ValidationFailure("Password", "Password is too short"),
            new ValidationFailure("Username", "Username is required")
        };

        var exception = new ValidationException(validationFailures);    
        
        Assert.That(exception.Errors, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception.Errors.Keys, Is.EquivalentTo(new [] { "Password", "Email", "Username" }));

            Assert.That(exception.Errors["Email"], Is.EqualTo(new[] { "Email is required", "Email format is invalid" }));

            Assert.That(exception.Errors["Password"], Is.EqualTo(new[] { "Password is too short" }));

            Assert.That(exception.Errors["Username"], Is.EqualTo(new[] { "Username is required" }));
        });
    }
}