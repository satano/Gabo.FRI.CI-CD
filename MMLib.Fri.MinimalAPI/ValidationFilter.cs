using FluentValidation;

internal class ValidationFilter<T> : IEndpointFilter
    where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // 👇 this can be more complicated
        if (context.Arguments.FirstOrDefault(a => a is T) is not T model)
        {
            throw new InvalidOperationException("Model is null");
        }

        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}

internal static class ValidationFilter
{
    public static IEndpointConventionBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class
        => builder.AddEndpointFilter<ValidationFilter<T>>()
            .ProducesValidationProblem();
}