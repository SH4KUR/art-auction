using System.Linq;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.AuctionCatalog;
using ArtAuction.WebUI.Models.Lot;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class LotController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public LotController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task PlaceBid()
        {

        }

        [HttpPost]
        public async Task SendMessage()
        {

        }
    }
}