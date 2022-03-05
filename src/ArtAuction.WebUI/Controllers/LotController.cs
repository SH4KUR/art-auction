using System.Threading.Tasks;
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
            return View();
        }
    }
}