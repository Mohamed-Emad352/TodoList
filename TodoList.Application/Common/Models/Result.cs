namespace TodoList.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public IEnumerable<string> Errors { get; }

    internal Result(bool isSuccess, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, new List<string>().ToArray());
    }
    
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}