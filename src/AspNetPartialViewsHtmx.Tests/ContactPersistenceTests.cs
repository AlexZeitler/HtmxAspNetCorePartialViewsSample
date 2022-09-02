using AspNetPartialViewsHtmx.Contacts;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using static AspNetPartialViewsHtmx.Tests.TestConfiguration.TestServices;

namespace AspNetPartialViewsHtmx.Tests;

public class ContactPersistenceTests
{
  [Fact]
  public async Task ShouldStoreAndLoadActiveContact()
  {
    var provider = await GetTestServiceProvider();
    var store = provider.GetService<IDocumentStore>();
    await using var session = store.LightweightSession();
    var contactId = Guid.NewGuid();
    var email = "mail@tempuri.org";
    var contact = new ActiveContact(contactId, email);
    session.Store<Contact>(contact);
    await session.SaveChangesAsync();

    var c = await session.LoadAsync<Contact>(contactId);
    Assert.IsType<ActiveContact>(c);

    c = new ArchivedContact(c as ActiveContact ?? throw new InvalidOperationException());
    session.Store<Contact>(c);
    await session.SaveChangesAsync();
  }
}
