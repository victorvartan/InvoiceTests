using InvoiceTest.Models;
using InvoiceTest.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceTest.UnitTests
{
    [Collection("Invoicing")]
    public class InvoiceNoteTests
    {
        public readonly DatabaseFixture _databaseFixture;
        public readonly UserService _userService;

        public InvoiceNoteTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _userService = new UserService(_databaseFixture.DataContext);
        }

        [Fact]
        public async Task AllUsersCanAddAndViewInvoiceNotes()
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

            var invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceNoteResultForAdmin = await invoiceNoteService.Create(new InvoiceNote
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                Text = "note by admin123"
            });
            createInvoiceNoteResultForAdmin.ShouldNotBeNull();
            createInvoiceNoteResultForAdmin.Code.ShouldBe(ServiceActionResultCode.Success);

            var user1 = await _userService.GetByApiKey("user123");
            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user1.Id, user1.Role);
            invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceNoteResultForUser = await invoiceNoteService.Create(new InvoiceNote
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                Text = "note by user123"
            });
            createInvoiceNoteResultForAdmin.ShouldNotBeNull();
            createInvoiceNoteResultForAdmin.Code.ShouldBe(ServiceActionResultCode.Success);


            var user2 = await _userService.GetByApiKey("user345");
            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user2.Id, user2.Role);
            invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createdInvoiceNotes = await invoiceNoteService.GetByInvoiceId(invoiceId);

            createdInvoiceNotes.ShouldNotBeNull();
            createdInvoiceNotes.Count.ShouldBe(2);
        }

        [Fact]
        public async Task AllUsersCanUpdateOwnInvoiceNotes()
        {
            var admin = await _userService.GetByApiKey("admin123");

            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin.Id, admin.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceId = Guid.NewGuid();
            var createInvoiceResultForAdmin = await invoiceService.Create(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.23m
            });
            createInvoiceResultForAdmin.ShouldNotBeNull();
            createInvoiceResultForAdmin.Code.ShouldBe(ServiceActionResultCode.Success);

            var invoiceNoteIdForAdmin = Guid.NewGuid();
            var invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceNoteResultForAdmin = await invoiceNoteService.Create(new InvoiceNote
            {
                Id = invoiceNoteIdForAdmin,
                InvoiceId = invoiceId,
                Text = "note by admin123"
            });
            createInvoiceNoteResultForAdmin.ShouldNotBeNull();
            createInvoiceNoteResultForAdmin.Code.ShouldBe(ServiceActionResultCode.Success);
            createInvoiceNoteResultForAdmin.Entity.Text.ShouldBe("note by admin123");

            var updateInvoiceNoteResultForAdmin = await invoiceNoteService.Update(new InvoiceNote
            {
                Id = invoiceNoteIdForAdmin,
                Text = "note by admin123 - updated"
            });
            updateInvoiceNoteResultForAdmin.ShouldNotBeNull();
            updateInvoiceNoteResultForAdmin.Code.ShouldBe(ServiceActionResultCode.Success);
            updateInvoiceNoteResultForAdmin.Entity.Text.ShouldBe("note by admin123 - updated");

            var user = await _userService.GetByApiKey("user345");

            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);
            invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceNoteIdForUser = Guid.NewGuid();
            var createInvoiceNoteResultForUser = await invoiceNoteService.Create(new InvoiceNote
            {
                Id = invoiceNoteIdForUser,
                InvoiceId = invoiceId,
                Text = "note by user345"
            });
            createInvoiceNoteResultForUser.ShouldNotBeNull();
            createInvoiceNoteResultForUser.Code.ShouldBe(ServiceActionResultCode.Success);
            createInvoiceNoteResultForUser.Entity.Text.ShouldBe("note by user345");

            var updateInvoiceNoteResultForUser = await invoiceNoteService.Update(new InvoiceNote
            {
                Id = invoiceNoteIdForUser,
                Text = "note by user345 - updated"
            });
            updateInvoiceNoteResultForUser.ShouldNotBeNull();
            updateInvoiceNoteResultForUser.Code.ShouldBe(ServiceActionResultCode.Success);
            updateInvoiceNoteResultForUser.Entity.Text.ShouldBe("note by user345 - updated");
        }

        [Fact]
        public async Task UsersCannotUpdateInvoicesNotOwned()
        {
            var admin1 = await _userService.GetByApiKey("admin123");

            var authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin1.Id, admin1.Role);
            var invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);

            var invoiceId = Guid.NewGuid();
            var createInvoiceResultForAdmin1 = await invoiceService.Create(new Invoice
            {
                Id = invoiceId,
                Identifier = "INV-01",
                Amount = 1.23m
            });
            createInvoiceResultForAdmin1.ShouldNotBeNull();
            createInvoiceResultForAdmin1.Code.ShouldBe(ServiceActionResultCode.Success);

            var invoiceNoteId = Guid.NewGuid();
            var invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);
            var createInvoiceNoteResultForAdmin1 = await invoiceNoteService.Create(new InvoiceNote
            {
                Id = invoiceNoteId,
                InvoiceId = invoiceId,
                Text = "note by admin123"
            });
            createInvoiceNoteResultForAdmin1.ShouldNotBeNull();
            createInvoiceNoteResultForAdmin1.Code.ShouldBe(ServiceActionResultCode.Success);
            createInvoiceNoteResultForAdmin1.Entity.Text.ShouldBe("note by admin123");

            var admin2 = await _userService.GetByApiKey("admin345");

            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(admin2.Id, admin2.Role);
            invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);
            invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);

            var updateInvoiceNoteResultForAdmin2 = await invoiceNoteService.Update(new InvoiceNote
            {
                Id = invoiceNoteId,
                Text = "update by admin345 should fail"
            });
            updateInvoiceNoteResultForAdmin2.ShouldNotBeNull();
            updateInvoiceNoteResultForAdmin2.Code.ShouldBe(ServiceActionResultCode.OnlyOwnersAllowed);

            var user = await _userService.GetByApiKey("user123");

            authenticatedPrincipal = Utils.MockAuthenticatedPrincipal(user.Id, user.Role);
            invoiceService = new InvoiceService(_databaseFixture.DataContext, authenticatedPrincipal);
            invoiceNoteService = new InvoiceNoteService(_databaseFixture.DataContext, authenticatedPrincipal);

            var updateInvoiceNoteResultForUser = await invoiceNoteService.Update(new InvoiceNote
            {
                Id = invoiceNoteId,
                Text = "update by user123 should fail"
            });
            updateInvoiceNoteResultForUser.ShouldNotBeNull();
            updateInvoiceNoteResultForUser.Code.ShouldBe(ServiceActionResultCode.OnlyOwnersAllowed);
        }
    }
}
