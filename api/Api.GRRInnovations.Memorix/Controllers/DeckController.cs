using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IDeckService _deckService;

        public DeckController(IDeckService deckService)
        {
            _deckService = deckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //todo: add jwt auth

            var decks = await _deckService.GetDecksAsync();
            return Ok(decks);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] WrapperInDeck<Deck> wrapperInDeck)
        {
            var deckModel = await wrapperInDeck.Result();
            if (deckModel == null)
            {
                return BadRequest("Invalid deck data.");
            }

            await _deckService.AddDeckAsync(deckModel, new User() { 
                Uid = Guid.NewGuid(),
                Email = new Domain.ValueObjects.Email("ga"),
                PasswordHash = "123",
                Name = "gab"
            });

            return new OkObjectResult(wrapperInDeck);
        }
    }
}
