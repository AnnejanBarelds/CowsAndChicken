using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CowsAndChicken.ApplicationServices;
using CowsAndChicken.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CowsAndChicken.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private ICowsAndChickenGameService _gameService;

        public PlayersController(ICowsAndChickenGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Player))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] Player player)
        {
            try
            {
                var playerDto = await _gameService.RegisterPlayerAsync(player);
                return Ok(playerDto);
            }
            catch
            {
                // TODO: Add some validation so that we can also return errors in the 400 range for specific mistakes on the client side
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/game/turn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TakeTurnAsync(Guid id, [FromBody] int guess)
        {
            try
            {
                var result = await _gameService.TakeTurnAsync(id, guess);
                return Ok(result);
            }
            catch
            {
                // TODO: Add some validation so that we can also return errors in the 400 range for specific mistakes on the client side
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/game")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameInfo))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StartGameAsync(Guid id, [FromBody] int nrOfDigits)
        {
            try
            {
                var result = await _gameService.StartGameAsync(id, nrOfDigits);
                return Ok(result);
            }
            catch
            {
                // TODO: Add some validation so that we can also return errors in the 400 range for specific mistakes on the client side
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/efficiency")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<int,Stats>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEfficiencyStats(Guid id)
        {
            try
            {
                var result = await _gameService.GetEfficiencyStatsAsync(id);
                return Ok(result);
            }
            catch
            {
                // TODO: Add some validation so that we can also return errors in the 400 range for specific mistakes on the client side
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/speed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Stats))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSpeedStats(Guid id)
        {
            try
            {
                var result = await _gameService.GetSpeedStatsAsync(id);
                return Ok(result);
            }
            catch
            {
                // TODO: Add some validation so that we can also return errors in the 400 range for specific mistakes on the client side
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
