namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public class ContactRepository : IContactRepository
{
    private readonly Dictionary<int, Contact> _contacts;

    public ContactRepository()
    {
        //initialize the dictionary with real dummy data
        _contacts = new Dictionary<int, Contact>
        {
            {1, new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", PhoneNumber = "555-0101", Address = "123 Main St, Boston, MA" }},
            {2, new Contact { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", PhoneNumber = "555-0102", Address = "456 Oak Ave, Chicago, IL" }},
            {3, new Contact { Id = 3, FirstName = "Michael", LastName = "Johnson", Email = "michael.j@email.com", PhoneNumber = "555-0103", Address = "789 Pine Rd, Seattle, WA" }},
            {4, new Contact { Id = 4, FirstName = "Sarah", LastName = "Williams", Email = "sarah.w@email.com", PhoneNumber = "555-0104", Address = "321 Elm St, Austin, TX" }},
            {5, new Contact { Id = 5, FirstName = "Robert", LastName = "Brown", Email = "robert.b@email.com", PhoneNumber = "555-0105", Address = "654 Maple Dr, Denver, CO" }},
            {6, new Contact { Id = 6, FirstName = "Emily", LastName = "Davis", Email = "emily.d@email.com", PhoneNumber = "555-0106", Address = "987 Cedar Ln, Portland, OR" }},
            {7, new Contact { Id = 7, FirstName = "David", LastName = "Miller", Email = "david.m@email.com", PhoneNumber = "555-0107", Address = "147 Birch Ave, Miami, FL" }},
            {8, new Contact { Id = 8, FirstName = "Lisa", LastName = "Wilson", Email = "lisa.w@email.com", PhoneNumber = "555-0108", Address = "258 Walnut St, San Diego, CA" }},
            {9, new Contact { Id = 9, FirstName = "James", LastName = "Taylor", Email = "james.t@email.com", PhoneNumber = "555-0109", Address = "369 Cherry Rd, Phoenix, AZ" }},
            {10, new Contact { Id = 10, FirstName = "Jennifer", LastName = "Anderson", Email = "jennifer.a@email.com", PhoneNumber = "555-0110", Address = "741 Spruce Ct, Nashville, TN" }}
        };

    }

    public IEnumerable<Contact> GetAll() => _contacts.Values;
    public Contact? GetById(int id) => _contacts.TryGetValue(id, out var contact) ? contact : null;
    public Task Add(Contact contact)
    {
        contact.Id = _contacts.Count + 1;
        _contacts[contact.Id] = contact;
        return Task.CompletedTask;
    }

    public Task Update(Contact contact)
    {
        _contacts[contact.Id] = contact;
        return Task.CompletedTask;
    }

    public Task<bool> Delete(int id)
    {
        return Task.FromResult(_contacts.Remove(id));
    }
}
