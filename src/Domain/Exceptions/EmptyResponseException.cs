namespace Domain.Exceptions;

public sealed class EmptyResponseException : Exception
{
    public EmptyResponseException() : base() { }
    public EmptyResponseException(string message) : base(message) { }
}