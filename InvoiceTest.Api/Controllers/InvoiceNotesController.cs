using InvoiceTest.Api.Security;
using InvoiceTest.Models;
using InvoiceTest.Services;
using InvoiceTest.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceTest.Api.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize(AuthenticationSchemes = CustomAuthenticationExtensions.AUTHENTICATION_SCHEME)]
    public class InvoiceNotesController : ControllerBase
    {
        private readonly IInvoiceNoteService _invoiceNoteService;

        public InvoiceNotesController(IInvoiceNoteService invoiceNoteService)
        {
            _invoiceNoteService = invoiceNoteService;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<InvoiceNote>> GetById([FromRoute] Guid id)
        {
            var invoiceNote = await _invoiceNoteService.GetById(id);
            return Ok(invoiceNote);
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceNote>>> GetByInvoiceId([FromQuery] Guid invoiceId)
        {
            var invoiceNotes = await _invoiceNoteService.GetByInvoiceId(invoiceId);
            return Ok(invoiceNotes);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceActionResult<InvoiceNote>>> Create([FromBody] InvoiceNote invoiceNote)
        {
            var result = await _invoiceNoteService.Create(invoiceNote);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceActionResult<InvoiceNote>>> Update([FromBody] InvoiceNote invoiceNote)
        {
            var result = await _invoiceNoteService.Update(invoiceNote);
            return Ok(result);
        }
    }
}
