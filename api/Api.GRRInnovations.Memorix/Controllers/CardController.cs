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
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUserContext _userContext;
        private readonly IDeckService _deckService;

        public CardController(ICardService cardService, IUserContext userContext, IDeckService deckService)
        {
            _cardService = cardService;
            _userContext = userContext;
            _deckService = deckService;
        }

        [HttpGet("{cardId}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid cardId)
        {
            var userId = _userContext.RequireUserId();

            var options = CardOptions.Create()
                .WithFilterUserId(userId)
                .WithFilterIds<CardOptions.Builder>([cardId])
                .Build();

            var card = await _cardService.GetCardAsync(cardId, options);
            if (card == null)
            {
                return NotFound(Result<string>.Fail("Card not found."));
            };

            var response = await WrapperOutCard.From(card);
            return Ok(Result<WrapperOutCard>.Ok(response));
        }

        [HttpGet("deck/id/{deckId}/all")]
        [Authorize]
        public async Task<IActionResult> All(Guid deckId)
        {
            var userId = _userContext.RequireUserId();

            var deckOptions = DeckOptions.Create()
                .WithFilterUserId([userId])
                .WithFilterIds<DeckOptions.Builder>([deckId])
                .Build();

            var decks = await _deckService.GetDecksAsync(deckOptions);
            if (decks?.Any() == false)
            {
                return NotFound(Result<string>.Fail("Deck not found."));
            }

            var cardOptions = CardOptions.Create()
                .WithFilterDeckId(deckId)
                .Build();
            var cards = await _cardService.GetCardsAsync(cardOptions);

            var response = await WrapperOutCard.From(cards);
            return Ok(Result<IEnumerable<WrapperOutCard>>.Ok(response));
        }

        [HttpPost("deck/id/{deckId}")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] WrapperInCard<Card> wrapperInCard, Guid deckId)
        {
            var userId = _userContext.RequireUserId();

            var deckOptions = DeckOptions.Create()
                 .WithFilterUserId([userId])
                 .WithFilterIds<DeckOptions.Builder>([deckId])
                 .Build();
            var decks = await _deckService.GetDecksAsync(deckOptions);
            if (decks?.Any() == false)
            {
                return NotFound(Result<string>.Fail("Deck not found."));
            }

            var deck = decks.First();
            var cardModel = await wrapperInCard.Result();

            var createdCard = await _cardService.AddCardAsync(cardModel, deck);

            var response = await WrapperOutCard.From(createdCard);
            return Ok(Result<WrapperOutCard>.Ok(response));
        }
    }
}
