using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using PaintERP.Data;

namespace PaintERP.Services;

public class TransactionService : ITransactionService
{
    private readonly PaintErpDbContext _context;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(PaintErpDbContext context, ILogger<TransactionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TransactionResult> ExecuteInTransactionAsync(Func<Task<TransactionResult>> operation, CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            state: _context,
            operation: async (context, ct) =>
            {
                IDbContextTransaction? transaction = null;

                try
                {
                    transaction = await context.Database.BeginTransactionAsync(ct);

                    var result = await operation();

                    if (result.Success)
                    {
                        await transaction.CommitAsync(ct);
                        _logger.LogInformation("Transaction committed successfully");
                    }
                    else
                    {
                        await transaction.RollbackAsync(ct);
                        _logger.LogWarning("Transaction rolled back due to operation failure: {Message}", result.Message);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        await transaction.RollbackAsync(ct);
                        _logger.LogError(ex, "Transaction rolled back due to exception: {Message}", ex.Message);
                    }

                    return TransactionResult.FailureResult(
                        "An error occurred while processing the transaction",
                        ex.Message
                    );
                }
                finally
                {
                    transaction?.Dispose();
                }
            },
            verifySucceeded: null,
            cancellationToken: cancellationToken
        );
    }

    public async Task<TransactionResult<T>> ExecuteInTransactionAsync<T>(Func<Task<TransactionResult<T>>> operation, CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            state: _context,
            operation: async (context, ct) =>
            {
                IDbContextTransaction? transaction = null;

                try
                {
                    transaction = await context.Database.BeginTransactionAsync(ct);

                    var result = await operation();

                    if (result.Success)
                    {
                        await transaction.CommitAsync(ct);
                        _logger.LogInformation("Transaction committed successfully");
                    }
                    else
                    {
                        await transaction.RollbackAsync(ct);
                        _logger.LogWarning("Transaction rolled back due to operation failure: {Message}", result.Message);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        await transaction.RollbackAsync(ct);
                        _logger.LogError(ex, "Transaction rolled back due to exception: {Message}", ex.Message);
                    }

                    return TransactionResult<T>.FailureResult(
                        "An error occurred while processing the transaction",
                        ex.Message
                    );
                }
                finally
                {
                    transaction?.Dispose();
                }
            },
            verifySucceeded: null,
            cancellationToken: cancellationToken
        );
    }
}
