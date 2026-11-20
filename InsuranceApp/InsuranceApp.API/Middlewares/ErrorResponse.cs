namespace InsuranceApp.API.Middlewares;

public sealed class ErrorResponse
{
    public string ErrorType { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
    public IDictionary<string, string[]>? Errors { get; set; }
}