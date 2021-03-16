using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Receiving.API.Dto;

namespace Receiving.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Store a Player
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /players
        ///     {
        ///         "identifier": "A6303EE5-E8FC-4B01-B2E4-C4D9642DF4BB",
        ///         "firstName": "Abel",
        ///         "lastName": "Powell"
        ///     }
        /// </remarks>
        /// <param name="player"></param>
        /// <returns>Player identifier with successful message</returns>
        /// <response code="201">Player successfully stored</response>
        /// <response code="400">Player already exist</response>
        /// /// <response code="422">Validation problem</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Store(PlayerDto player)
        {
            _logger.LogWarning($"Player has been stored with identifier:{player.Identifier}");
            return Created(string.Empty, new  { playerIdentifier = player.Identifier });
        }
    }
}
