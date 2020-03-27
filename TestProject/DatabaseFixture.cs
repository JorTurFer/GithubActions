using DbUp;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private const string ConnectionString = "Server=localhost,1434;Database=test;User Id=sa;Password=P2ssw0rd!;";
        public IDbConnection Connection { get; private set; }

        public DatabaseFixture()
        {


        }

        public async Task InitializeAsync()
        {
            EnsureDatabase.For.SqlDatabase(ConnectionString);

            var upgradeEngine = DeployChanges.To
                .SqlDatabase(ConnectionString)
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseFixture).Assembly)
                .LogToConsole()
                .Build();
            for (var i = 1; i < 5; i++)
            {
                var result = upgradeEngine.PerformUpgrade();
                var done = result.Successful;
                if (done) break;
                else await Task.Delay(i * 2000);
            }

            Connection = new SqlConnection(ConnectionString);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
