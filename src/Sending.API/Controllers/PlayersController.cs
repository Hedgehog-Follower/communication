using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sending.API.HttpClients;
using Sending.Domain;

namespace Sending.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IWelcomeClient _client;
        private readonly ApplicationContext _context;

        public PlayersController(IWelcomeClient client, ApplicationContext context)
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Get a Player
        /// </summary>
        /// <remarks>
        ///     GET /players/{playerId:long}
        ///     {
        ///         "id": 1,
        ///         "firstName": "Abel",
        ///         "lastName": "Powell"
        ///     }
        /// </remarks>
        /// <param name="playerId"></param>
        /// <returns>Player with all associated data</returns>
        /// <response code="200">Returns Player when exist</response>
        /// <response code="404">When Player of a given playerId not exist</response>
        [HttpGet("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(long playerId)
        {
            var playerOrNothing = await _context.Players.SingleOrDefaultAsync(x => x.Id == playerId);
            if (playerOrNothing == null)
            {
                return NotFound(new { playerId });
            }

            return Ok(playerOrNothing);
        }
    }
}
