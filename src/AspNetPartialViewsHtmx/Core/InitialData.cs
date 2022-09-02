using AspNetPartialViewsHtmx.Contacts;
using Marten;
using Marten.Schema;

namespace AspNetPartialViewsHtmx.Core;

public class InitialData : IInitialData
{
  private readonly object[] _initialData;

  public InitialData(params Contact[] initialData)
  {
    _initialData = initialData;
  }

  public async Task Populate(IDocumentStore store, CancellationToken cancellation)
  {
    await using var session = store.LightweightSession();
    // Marten UPSERT will cater for existing records
    session.Store(_initialData);
    await session.SaveChangesAsync(cancellation);
  }
}

public static class InitialDatasets
{
  public static readonly Contact[] Contacts =
  {
    new ActiveContact(Guid.Parse("2219b6f7-7883-4629-95d5-1a8a6c74b244"), "Netram Ltd."),
    new ActiveContact(Guid.Parse("642a3e95-5875-498e-8ca0-93639ddfebcd"), "Acme Inc.")
  };
};
