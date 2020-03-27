using System;
using System.Threading.Tasks;
using Dapper;
using Xunit;

namespace TestProject
{
    public class Tests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public Tests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }


        [Theory]
        [InlineData("uno")]
        [InlineData("dos")]
        [InlineData("tres")]
        [InlineData("cuatro")]
        public async Task Test1(string input)
        {
            var sql1 = $"Insert into TestTable (content) values ('{input}')";
            await _fixture.Connection.ExecuteAsync(sql1);

            var sql2 = "Select content from TestTable";
            var result = await _fixture.Connection.QueryFirstAsync<string>(sql2);

            Assert.Equal(input, result);
        }
        
        public void Dispose()
        {
            var sql1 = "Delete from TestTable";
            _fixture.Connection.Execute(sql1);
        }
    }
}
