using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sending.API.HttpClients;

namespace Sending.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController : ControllerBase
    {
        private readonly ITestClient _client;
        private readonly IWelcomeClient _welcomeClient;

        public WelcomeController(ITestClient client, IWelcomeClient welcomeClient)
        {
            _client = client;
            _welcomeClient = welcomeClient;
        }

        /// <summary>
        /// GetAsync nothing special right here
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /welcome
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <returns>string</returns>
        /// <response code="200">On success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _client.PostAsync();
            return Ok(result);
        }
    }
}
