using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public ProfileController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}