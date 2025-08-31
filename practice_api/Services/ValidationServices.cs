using FluentValidation;
using FluentValidation.Results;
using practice_api.Contracts;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Dictionary<string, string[]>?> ValidateAsync<T>(T dto) where T : class
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
            throw new InvalidOperationException($"No validator found for {typeof(T).Name}");

        ValidationResult result = await validator.ValidateAsync(dto);

        return result.IsValid
            ? null
            : CamelCasedErrors(result.Errors);
    }

    public Dictionary<string, string[]>? Validate<T>(T dto) where T : class
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
            throw new InvalidOperationException($"No validator found for {typeof(T).Name}");

        ValidationResult result = validator.Validate(dto);

        return result.IsValid
            ? null
            : CamelCasedErrors(result.Errors);
    }

    private Dictionary<string, string[]> CamelCasedErrors(List<ValidationFailure> errors)
    {
        return errors
            .GroupBy(e => char.ToLowerInvariant(e.PropertyName[0]) + e.PropertyName.Substring(1))
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}
