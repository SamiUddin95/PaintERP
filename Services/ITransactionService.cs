namespace PaintERP.Services;

public interface ITransactionService
{
    Task<TransactionResult> ExecuteInTransactionAsync(Func<Task<TransactionResult>> operation, CancellationToken cancellationToken = default);
    Task<TransactionResult<T>> ExecuteInTransactionAsync<T>(Func<Task<TransactionResult<T>>> operation, CancellationToken cancellationToken = default);
}

public class TransactionResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public object? Data { get; set; }

    public static TransactionResult SuccessResult(string message = "", object? data = null)
    {
        return new TransactionResult
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static TransactionResult FailureResult(string message, params string[] errors)
    {
        return new TransactionResult
        {
            Success = false,
            Message = message,
            Errors = errors.ToList()
        };
    }

    public static TransactionResult FailureResult(string message, List<string> errors)
    {
        return new TransactionResult
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

public class TransactionResult<T> : TransactionResult
{
    public new T? Data { get; set; }

    public static TransactionResult<T> SuccessResult(T data, string message = "")
    {
        return new TransactionResult<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static new TransactionResult<T> FailureResult(string message, params string[] errors)
    {
        return new TransactionResult<T>
        {
            Success = false,
            Message = message,
            Errors = errors.ToList()
        };
    }
}
