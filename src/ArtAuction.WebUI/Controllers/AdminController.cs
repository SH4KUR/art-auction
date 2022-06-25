using System.Linq;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Profile;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AdminController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var complaints = await _mediator.Send(new GetComplaintsCommand());
            return View(complaints.Select(c => _mapper.Map<ComplaintViewModel>(c)));
        }
    }
}