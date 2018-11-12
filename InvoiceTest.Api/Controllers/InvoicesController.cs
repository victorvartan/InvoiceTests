using InvoiceTest.Api.Security;
using InvoiceTest.Models;
using InvoiceTest.Services;
using InvoiceTest.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InvoiceTest.Api.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize(AuthenticationSchemes = CustomAuthenticationExtensions.AUTHENTICATION_SCHEME)]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<Invoice>> GetById([FromRoute] Guid id)
        {
            var invoice = await _invoiceService.GetById(id);
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceActionResult<Invoice>>> Create([FromBody] Invoice invoice)
        {
            var result = await _invoiceService.Create(invoice);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceActionResult<Invoice>>> Update([FromBody] Invoice invoice)
        {
            var result = await _invoiceService.Update(invoice);
            return Ok(result);
        }
    }
}
