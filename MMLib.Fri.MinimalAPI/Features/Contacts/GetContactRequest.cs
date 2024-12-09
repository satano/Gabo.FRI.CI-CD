using Microsoft.AspNetCore.Http.HttpResults;

namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class GetContactRequest
{
    public static IEndpointConventionBuilder MapContactGet(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGet("/{id}", OnGet)
            .WithDescription("Get a contact by id");

    private static Results<Ok<Contact>, NotFound> OnGet(int id, IContactRepository repository)
    {
        var contact = repository.GetById(id);
        return contact is null ? TypedResults.NotFound() : TypedResults.Ok(contact);
    }
}