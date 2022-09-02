using AspNetPartialViewsHtmx.Contacts;
using Marten;
using Weasel.Core;

namespace AspNetPartialViewsHtmx.Core;

public static class DocumentStoreConfiguration
{
  public static IServiceCollection UseDocumentStore(this IServiceCollection services,
    IConfigurationRoot config)
  {
    var martenConfig = config.GetSection("EventStore")
      .Get<EventStoreConfiguration>();
    services.AddMarten(ConfigureOptions(martenConfig))
      .InitializeWith(new InitialData(InitialDatasets.Contacts));
    return services;
  }

  public static IDocumentStore GetDocumentStore(EventStoreConfiguration config)
  {
    return DocumentStore.For(ConfigureOptions(config));
  }

  private static Action<StoreOptions> ConfigureOptions(EventStoreConfiguration config)
  {
    return options =>
    {
      options.Connection(config.ConnectionString);
      options.AutoCreateSchemaObjects = AutoCreate.All;
      options.UseDefaultSerialization(nonPublicMembersStorage: NonPublicMembersStorage.NonPublicDefaultConstructor);
      options.Schema.For<Contact>()
        .AddSubClass<ActiveContact>()
        .AddSubClass<ArchivedContact>();
    };
  }
}

public class EventStoreConfiguration
{
  public string ConnectionString { get; set; }
}
