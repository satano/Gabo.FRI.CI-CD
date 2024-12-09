using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class UpdateContactRequest
{
    public class UpdateContact
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateContactValidator : AbstractValidator<UpdateContact>
    {
        public UpdateContactValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public static IEndpointConventionBuilder MapContactPut(this IEndpointRouteBuilder endpoints)
        => endpoints.MapPut("/{id}", OnPut)
        .WithValidation<UpdateContact>()
        .WithDescription("Update a contact");

    static async Task<Results<Ok<Contact>, NotFound>> OnPut(int id, UpdateContact request, IContactRepository repository)
    {
        var contact = repository.GetById(id);

        if (contact is null)
        {
            return TypedResults.NotFound();
        }

        contact = request.Adapt(contact);

        await repository.Update(contact);
        return TypedResults.Ok(contact);
    }
}
