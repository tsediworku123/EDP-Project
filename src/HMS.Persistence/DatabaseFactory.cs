using Microsoft.EntityFrameworkCore;
using HMS.Core.Persistence.Context;

namespace HMS.Core.Persistence
{
    public static class DatabaseFactory
    {
        public static string ConnectionString { get; set; } = @"Data Source=(localdb)\MAPToolkit;Initial Catalog=HMSDatabase;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Application Name=""SQL Server Management Studio""";

        public static HMSDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HMSDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString, options => options.EnableRetryOnFailure());
            return new HMSDbContext(optionsBuilder.Options);
        }
    }
}
