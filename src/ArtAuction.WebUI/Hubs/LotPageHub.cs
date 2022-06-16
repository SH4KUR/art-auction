using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.WebUI.Hubs
{
    public class LotPageHub : Hub
    {
        public async Task JoinChatAuctionRoom(int auction)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, auction.ToString());
        }

        public async Task SendMessageToChat(int auction)
        {
            await Clients.Group(auction.ToString()).SendAsync("ReceiveChatMessages");
        }

        public async Task RefreshLotPrice(int auction)
        {
            await Clients.Group(auction.ToString()).SendAsync("RefreshCurrentPrice");
        }
    }
}