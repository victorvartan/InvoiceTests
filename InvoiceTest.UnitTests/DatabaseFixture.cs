using InvoiceTest.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace InvoiceTest.UnitTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                           .UseInMemoryDatabase(databaseName: "InvoiceTest")
                           .Options;
            DataContext = new DataContext(dbOptions);
            Utils.AddTestUsers(DataContext);
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public DataContext DataContext { get; private set; }
    }

    [CollectionDefinition("Invoicing")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
