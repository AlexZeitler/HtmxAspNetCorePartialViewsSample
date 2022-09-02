using Marten;

namespace AspNetPartialViewsHtmx.Contacts;

public static class ContactsConfiguration
{
  public static IServiceCollection AddContacts(this IServiceCollection services)
  {
    return services.AddTransient<Contacts>();
  }
}

public class Contacts
{
  private readonly IDocumentStore _store;

  public Contacts(IDocumentStore store)
  {
    _store = store;
  }

  public async Task StoreContact(Contact contact)
  {
    await using var session = _store.LightweightSession();
    session.Store<Contact>(contact);
    await session.SaveChangesAsync();
  }

  public async Task<IReadOnlyCollection<Contact>> LoadContacts()
  {
    await using var session = _store.LightweightSession();
    return session.Query<Contact>().OrderBy(c => c.Name).ToList().AsReadOnly();
  }

  public async Task<Contact?> LoadContact(Guid id)
  {
    await using var session = _store.LightweightSession();
    return await session.LoadAsync<Contact>(id);
  }
}

public abstract record Contact()
{
  public Guid Id { get; init; }
  public string Name { get; init; }
}

public record ActiveContact : Contact
{
  private ActiveContact()
  {
  }

  public ActiveContact(Guid id, string name)
  {
    Id = id;
    Name = name;
  }

  public ActiveContact(ArchivedContact archivedContact)
  {
    Name = archivedContact.Name;
    Id = archivedContact.Id;
  }

  public ArchivedContact Archive()
  {
    return new ArchivedContact(this);
  }
}

public record ArchivedContact : Contact
{
  private ArchivedContact()
  {
  }

  public ArchivedContact(ActiveContact activeContact)
  {
    Name = activeContact.Name;
    Id = activeContact.Id;
  }

  public ActiveContact Unarchive()
  {
    return new ActiveContact(this);
  }
}
