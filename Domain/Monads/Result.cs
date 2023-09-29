using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Domain.Monads;

public readonly struct Result<TSuccess, TFailure>
{
    private readonly TFailure? _failure;
    private readonly TSuccess? _success;

    public Result(TSuccess success)
    {
        _success = success;
        _failure = default;
        IsSuccess = true;
    }

    public Result(TFailure failure)
    {
        _failure = failure;
        _success = default;
        IsSuccess = false;
    }

    public TSuccess Success
    {
        get
        {
            if (IsFailure) throw new InvalidOperationException("Can't access success. Result is failure");
            return _success;
        }
    }

    [MemberNotNullWhen(true, nameof(_success))]
    [MemberNotNullWhen(false, nameof(_failure))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(_failure))]
    [MemberNotNullWhen(false, nameof(_success))]
    public bool IsFailure => !IsSuccess;

    public TFailure Failure
    {
        get
        {
            if (IsSuccess) throw new InvalidOperationException("Can't access failure. Result is success");
            return _failure;
        }
    }

    public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
    {
        return new Result<TSuccess, TFailure>(success);
    }

    public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
    {
        return new Result<TSuccess, TFailure>(failure);
    }
    
    public void LogResult(ILogger logger)
    {
        if (IsFailure && Failure != null)
            logger.LogWarning("Result created with failure: {@Failure}", Failure);
        
        else if (IsSuccess && Success != null)
            logger.LogWarning("Result created with success: {@Success}", Success);
    }
}