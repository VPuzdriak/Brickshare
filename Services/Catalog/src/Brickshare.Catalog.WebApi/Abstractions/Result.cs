using System.Diagnostics.CodeAnalysis;

namespace Brickshare.Catalog.WebApi.Abstractions;

public static class Result
{
    public static Result<Empty> Empty => new(new Empty());
}

public sealed record Result<T>
{
    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public T? Data { get; }
    public Failure? Error { get; }

    public Result(T? data)
    {
        Success = true;
        Data = data;
    }

    public Result(Failure error)
    {
        Success = false;
        Error = error;
    }

    public static implicit operator Result<T>(T data) => new(data);
    public static implicit operator Result<T>(Failure error) => new(error);
}

public struct Empty;

public record Failure(string Code, string Message);