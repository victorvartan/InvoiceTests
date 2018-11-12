using InvoiceTest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InvoiceTest.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IUserService _userService;

        public HealthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var anyAdmins = await _userService.Any();
            if (!anyAdmins) throw new Exception(Resources.ExceptionMessages.NoUsersInDatabase);
            return Ok("healthy!");
        }
    }
}
