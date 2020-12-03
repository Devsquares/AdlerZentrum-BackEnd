using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace WebApi.Controllers
{
    [Microsoft.AspNetCore.Cors.EnableCors("_myAllowSpecificOrigins")]
    public class MailingListController : Controller
    {
        private readonly IEmailService _emailService;

        public MailingListController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("AddToMailingList")]
        public async Task<IActionResult> Post(string mail)
        {

            string startupPath = System.IO.Directory.GetCurrentDirectory();

            System.IO.File.AppendAllText($"{startupPath}/emails.txt", mail + "\n");

            return Ok();
        }

    }
}