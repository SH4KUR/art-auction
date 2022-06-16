using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.WebUI.Hubs
{
    public class LotPageHub : Hub
    {
        public async Task JoinAuctionLotRoom(int auction)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, auction.ToString());
        }

        public async Task RefreshChatMessages(int auction)
        {
            await Clients.Group(auction.ToString()).SendAsync("RefreshChatMessages");
        }

        public async Task RefreshLotPrice(int auction)
        {
            await Clients.Group(auction.ToString()).SendAsync("RefreshCurrentPrice");
        }
    }
}