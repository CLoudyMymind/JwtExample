namespace WebApi.ViewModels;

public sealed class ErrorViewModel
{
    public ErrorViewModel(Exception exception) : this(exception.Message)
    {
    }

    private ErrorViewModel(string message)
    {
        Message = message;
    }

    public string Message { get; }
}