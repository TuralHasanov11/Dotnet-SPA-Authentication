namespace WebApp.Models.Shared;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    protected Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    protected Result()
    {
        IsSuccess = true;
    }

    public static Result Success() => new();
    public static Result<T> Success<T>(T value) => new(value);
    public static Result Failure(Error error) => new(error);
    public static Result<T> Failure<T>(Error error) => new(error);
    public static Result<T> Create<T>(T value) => new(value);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(Error error) : base(error)
    {}

    protected internal Result(T value)
    {
        Value = value;
    }
}