using AspNetPartialViewsHtmx.Contacts;

namespace AspNetPartialViewsHtmx.Models;

public class ContactViewModel
{
  public ContactViewModel(Contact contact)
  {
    Id = contact.Id;
    Name = contact.Name;
    (contact switch
    {
      ActiveContact => (Action)(() => Archived = false),
      ArchivedContact => (Action)(() => Archived = true),
      _ => throw new ArgumentOutOfRangeException(nameof(contact))
    })();
  }

  public Guid Id { get; }
  public bool Archived { get; private set; }
  public string Name { get; set; }
}
