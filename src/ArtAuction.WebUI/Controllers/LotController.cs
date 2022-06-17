using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Hubs;
using ArtAuction.WebUI.Models.AuctionCatalog;
using ArtAuction.WebUI.Models.Lot;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.WebUI.Controllers
{
    [Route("[controller]")]
    public class LotController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHubContext<LotPageHub> _hubContext;
        
        public LotController(IMediator mediator, IMapper mapper, IHubContext<LotPageHub> hubContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet("{auctionNumber}")]
        public async Task<IActionResult> Index(int auctionNumber)
        {
            var lot = await _mediator.Send(new GetAuctionLotCommand(auctionNumber));

            var model = new AuctionLotViewModel
            {
                AuctionLot = _mapper.Map<AuctionViewModel>(lot.AuctionLot),
                Bids = lot.Bids.Select(b => _mapper.Map<BidViewModel>(b)),
                Messages = lot.Messages.Select(m => _mapper.Map<MessageViewModel>(m))
            };

            model.AuctionLot.Image = lot.AuctionLot.Photo;
            
            return View(model);
        }
        
        [HttpPost("{auctionNumber}/SendMessage")]
        public async Task SendMessage([FromBody] MessageModel model)
        {
            await _mediator.Send(new AddAuctionMessageCommand
            {
                Login = User?.FindFirst(ClaimTypes.Name)?.Value,
                AuctionNumber = model.AuctionNumber,
                Message = model.MessageText
            });

            await _hubContext.Clients.Group(model.AuctionNumber.ToString()).SendAsync("RefreshChatMessages");
        }

        [HttpGet("{auctionNumber}/GetMessages")]
        public async Task<JsonResult> GetMessages(int auctionNumber)
        {
            var messages = await _mediator.Send(new GetAuctionMessagesCommand { AuctionNumber = auctionNumber });
            return Json(messages.OrderBy(m => m.DateTime));
        }
    }

    public class MessageModel
    {
        public int AuctionNumber { get; set; }
        public string MessageText { get; set; }
    }
}