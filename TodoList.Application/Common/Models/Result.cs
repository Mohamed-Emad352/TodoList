namespace TodoList.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }

    internal Result(bool isSuccess, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors.ToArray();
    }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }
    
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}