namespace MMLib.Fri.MinimalAPI.Features.Contacts;

public interface IContactRepository
{
    IEnumerable<Contact> GetAll();
    Contact? GetById(int id);
    Task Add(Contact contact);
    Task Update(Contact contact);
    Task<bool> Delete(int id);
}
