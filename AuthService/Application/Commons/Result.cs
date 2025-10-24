namespace Application.Commons;

public class Result
{
    public bool IsSuccess { get; }
    public ServiceError? Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, ServiceError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(ServiceError error) => new(false, error);
}

public class Result<T> : Result
{
    public T? Data{ get; }

    private Result(bool isSuccess, T? value, ServiceError? error)
        : base(isSuccess, error)
    {
        Data = value;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static new Result<T> Failure(ServiceError error) => new(false, default, error);
}
