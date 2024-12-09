using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class CreateContactRequest
{
    public class CreateContact
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

    public class CreateContactValidator : AbstractValidator<CreateContact>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public static IEndpointConventionBuilder MapContactPost(this IEndpointRouteBuilder endpoints)
        => endpoints.MapPost("/", OnPost)
        .WithValidation<CreateContact>()
        .WithDescription("Create a new contact");

    static async Task<Results<Created<Contact>, BadRequest>> OnPost(CreateContact request, IContactRepository repository)
    {
        var contact = request.Adapt<Contact>();

        await repository.Add(contact);

        return TypedResults.Created($"/api/contacts/{contact.Id}", contact);
    }
}
