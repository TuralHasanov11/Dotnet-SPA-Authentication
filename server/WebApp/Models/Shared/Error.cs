namespace WebApp.Models.Shared;

public record Error
{
    public string Message { get; }

    public string Code { get; }

    public ErrorType ErrorType { get; }

    private Error(string code, string message, ErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

    public static explicit operator string(Error error) => error.Code;

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
}