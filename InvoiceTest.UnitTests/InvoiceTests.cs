using InvoiceTest.Models;
using InvoiceTest.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceTest.UnitTests
{
    [Collection("Invoicing")]
    public class InvoiceTests
    {
        public readonly DatabaseFixture _databaseFixture;
        public readonly UserService _userService;

        public InvoiceTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _userService = new UserService(_databaseFixture.DataContext);
        }

        [Fact]
        public async Task AdminsCanAddInvoices()
        {
            var user = await _userService.GetByApiKey("admin123");

            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceResult = await invoiceService.Create(new Invoice
            {
                Id = Guid.NewGuid(),
                Identifier = "INV-01",
                Amount = 1.23m
            });

            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Code.ShouldBe(ServiceActionResultCode.Success);
        }

        [Fact]
        public async Task RegularUsersCannotAddInvoices()
        {
            var user = await _userService.GetByApiKey("user123");

            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceResult = await invoiceService.Create(new Invoice
            {
                Id = Guid.NewGuid(),
                Identifier = "INV-01",
                Amount = 1.23m
            });

            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Code.ShouldBe(ServiceActionResultCode.OnlyAdminsAllowed);
        }

        [Fact]
        public async Task AdminsCanUpdateOwnInvoices()
        {
            var user = await _userService.GetByApiKey("admin123");

            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceId = Guid.NewGuid();
            var createInvoiceResult = await invoiceService.Create(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.23m
            });
            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Entity.Amount.ShouldBe(1.23m);

            var updateInvoiceResult = await invoiceService.Update(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.33m
            });
            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Code.ShouldBe(ServiceActionResultCode.Success);
            createInvoiceResult.Entity.Amount.ShouldBe(1.33m);
        }

        [Fact]
        public async Task AdminsCannotUpdateInvoicesNotOwned()
        {
            var admin1 = await _userService.GetByApiKey("admin123");
            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin1.Id, admin1.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceId = Guid.NewGuid();
            var createInvoiceResult = await invoiceService.Create(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.23m
            });
            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Code.ShouldBe(ServiceActionResultCode.Success);

            var admin2 = await _userService.GetByApiKey("admin345");
            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin2.Id, admin2.Role);
            invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var updateInvoiceResult = await invoiceService.Update(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.33m
            });
            updateInvoiceResult.ShouldNotBeNull();
            updateInvoiceResult.Code.ShouldBe(ServiceActionResultCode.OnlyOwnersAllowed);
        }

        [Fact]
        public async Task RegularUsersCannotUpdateInvoices()
        {
            var admin = await _userService.GetByApiKey("admin123");
            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin.Id, admin.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceId = Guid.NewGuid();
            var createInvoiceResult = await invoiceService.Create(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.23m
            });
            createInvoiceResult.ShouldNotBeNull();
            createInvoiceResult.Code.ShouldBe(ServiceActionResultCode.Success);

            var user = await _userService.GetByApiKey("user123");
            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var updateInvoiceResult = await invoiceService.Update(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.33m
            });
            updateInvoiceResult.ShouldNotBeNull();
            updateInvoiceResult.Code.ShouldBe(ServiceActionResultCode.OnlyAdminsAllowed);
        }
    }
}
