using Npgsql;

namespace AspNetPartialViewsHtmx.Tests.TestConfiguration;

public static class DocumentStoreConfiguration
{
  public static NpgsqlConnectionStringBuilder GetTestConnectionString()
  {
    return new NpgsqlConnectionStringBuilder()
    {
      Pooling = false,
      Port = 5435,
      Host = "localhost",
      CommandTimeout = 20,
      Database = "postgres",
      Password = "123456",
      Username = "postgres"
    };
  }
}
