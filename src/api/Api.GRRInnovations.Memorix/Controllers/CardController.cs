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
            var card = await _cardService.GetCardForUserAsync(cardId, options);
                
            if (card == null)
            {
                return NotFound(Result<string>.Failure(Error.NotFound("Card")));
            }

            var response = await WrapperOutCard.From(card);
            return Ok(Result<WrapperOutCard>.SuccessResult(response));
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

            var deck = await _deckService.GetDeckForUserAsync(deckId, deckOptions);
            if (deck == null)
            {
                return NotFound(Result<string>.Failure(Error.NotFound("Deck")));
            }

            var cardOptions = CardOptions.Create()
                .WithFilterDeckId(deckId)
                .Build();
            var cards = await _cardService.GetCardsAsync(cardOptions);

            var response = await WrapperOutCard.From(cards);
            return Ok(Result<IEnumerable<WrapperOutCard>>.SuccessResult(response));
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
            var deck = await _deckService.GetDeckForUserAsync(deckId, deckOptions);
            if (deck == null)
            {
                return NotFound(Result<string>.Failure(Error.NotFound("Deck")));
            }

            var cardModel = await wrapperInCard.Result();
            if (cardModel == null)
            {
                return BadRequest(Result<string>.Failure(Error.Validation("Invalid card data.")));
            }

            var createdCard = await _cardService.AddCardAsync(cardModel, deck);

            var response = await WrapperOutCard.From(createdCard);
            return Ok(Result<WrapperOutCard>.SuccessResult(response));
        }
    }
}
