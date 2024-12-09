using Microsoft.AspNetCore.Http.HttpResults;

namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class DeleteContactRequest
{
    public static IEndpointConventionBuilder MapContactDelete(this IEndpointRouteBuilder endpoints)
        => endpoints.MapDelete("/{id}", OnDelete).WithDescription("Delete a contact");

    private static async Task<Results<NoContent, NotFound>> OnDelete(int id, IContactRepository repository)
    {
        var isDeleted = await repository.Delete(id);
        return isDeleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
