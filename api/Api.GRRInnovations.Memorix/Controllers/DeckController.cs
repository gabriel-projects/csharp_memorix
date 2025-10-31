using Api.GRRInnovations.Memorix.Application.Interfaces;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Application.Wrappers.Out;
using Api.GRRInnovations.Memorix.Domain.Entities;
using Api.GRRInnovations.Memorix.Application.Wrappers;
using Api.GRRInnovations.Memorix.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.Memorix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IDeckService _deckService;
        private readonly IUserContext _userContext;
        private readonly IUserService _userService;

        public DeckController(IDeckService deckService, IUserContext userContext, IUserService userService)
        {
            _deckService = deckService;
            _userContext = userContext;
            _userService = userService;
        }

        [HttpGet("{deckId}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid deckId)
        {
            var userId = _userContext.RequireUserId();

            var options = DeckOptions.Create()
                .WithFilterUserId([userId])
                .WithFilterIds<DeckOptions.Builder>([deckId])
                .Build();

            var decks = await _deckService.GetDecksAsync(options);
            if (decks?.Any() == false)
            {
                return NotFound(Result<string>.Fail("Deck not found."));
            }

            var deck = decks?.First();

            var response = await WrapperOutDeck.From(deck);
            return Ok(Result<WrapperOutDeck>.Ok(response));
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var userId = _userContext.RequireUserId();

            var options = DeckOptions.Create()
                .WithFilterUserId([userId])
                .Build();

            var decks = await _deckService.GetDecksAsync(options);
            if (decks?.Any() == false)
            {
                return NotFound(Result<string>.Fail("Deck not found."));
            }

            var response = await WrapperOutDeck.From(decks);
            return Ok(Result<List<WrapperOutDeck>>.Ok(response));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] WrapperInDeck<Deck> wrapperInDeck)
        {
            var userId = _userContext.RequireUserId();

            var deckModel = await wrapperInDeck.Result();
            if (deckModel == null)
            {
                return BadRequest("Invalid deck data.");
            }

            var user = await _userService.GetUserByUidAsync(userId);
            if (user == null)
                return NotFound(Result<string>.Fail("User not found."));

            var deck = await _deckService.AddDeckAsync(deckModel, user);

            var response = await WrapperOutDeck.From(deck);
            return Ok(Result<WrapperOutDeck>.Ok(response));
        }
    }
}
