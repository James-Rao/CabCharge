using CabCharge.Models;
using CabCharge.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CabCharge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailer _emailer;

        public EmailController(IEmailer emailer)
        {
            _emailer = emailer;
        }

        [HttpPost]
        public async Task<EmailResponse> Post([FromBody] EmailRequest email)
        {
            return await _emailer.SendEmail(email);
        }
    }
}