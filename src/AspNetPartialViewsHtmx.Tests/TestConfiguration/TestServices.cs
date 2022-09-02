using AspNetPartialViewsHtmx.Core;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AspNetPartialViewsHtmx.Tests.TestConfiguration;

public class TestServices
{
  public static async Task<ServiceProvider> GetTestServiceProvider()
  {
    var testConnectionString = GetTestConnectionString("postgres");
    var myConfiguration = new Dictionary<string, string>
    {
      { "EventStore:ConnectionString", testConnectionString },
    };
    var configuration = new ConfigurationBuilder()
      .AddInMemoryCollection(myConfiguration)
      .Build();

    var services = new ServiceCollection();
    services.UseDocumentStore(configuration);
    return services.BuildServiceProvider();
  }

  public static string GetTestConnectionString(string dbName)
  {
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
    {
      Pooling = false,
      Port = 5435,
      Host = "localhost",
      CommandTimeout = 20,
      Database = dbName,
      Password = "123456",
      Username = "postgres"
    };
    var pgTestConnectionString = connectionStringBuilder.ToString();

    return pgTestConnectionString;
  }
}
