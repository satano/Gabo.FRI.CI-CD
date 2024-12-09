using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;

namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class Setup
{
    public static IServiceCollection AddContacts(this IServiceCollection services)
    {
        services.AddSingleton<IContactRepository, ContactRepository>();
        services.AddScoped<IValidator<UpdateContactRequest.UpdateContact>, UpdateContactRequest.UpdateContactValidator>();
        services.AddScoped<IValidator<CreateContactRequest.CreateContact>, CreateContactRequest.CreateContactValidator>();

        services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            limiterOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
            {
                options.PermitLimit = 2;
                options.Window = TimeSpan.FromSeconds(10);
            });
        });

        services.AddOutputCache(options =>
        {
            options.AddPolicy("Expire5", builder =>
                builder.Expire(TimeSpan.FromSeconds(5)));
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IEndpointConventionBuilder MapContacts(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/contacts")
            .WithTags("Contacts");

        group.MapContactsGet();
        group.MapContactGet()
            .CacheOutput("Expire5");
        group.MapContactPost()
            .RequireRateLimiting("fixed")
            .ProducesProblem(StatusCodes.Status429TooManyRequests);

        group.MapContactPut();
        group.MapContactDelete();

        return group;
    }
}
