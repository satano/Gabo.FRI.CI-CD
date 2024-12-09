namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public static class GetContactsRequest
{
    public static IEndpointConventionBuilder MapContactsGet(this IEndpointRouteBuilder endpoints)
        => endpoints.MapGet("/", (IContactRepository repository) => repository.GetAll())
            .WithDescription("Get all contacts");
}
