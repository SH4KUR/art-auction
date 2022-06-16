using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.WebUI.Hubs
{
    public class ChatLotHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatLotHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task JoinChatAuctionRoom(int auction)
        {
            //await this.Groups.AddToGroupAsync(chatRoomName).ConfigureAwait(false);

            //Dictionary<string, string> messages = await this.DatabaseManager.GetChatHistory(chatRoomName).ConfigureAwait(false);

            //await this.Clients.Group(chatRoomName).BroadcastMessageAsync(messages);
        }

        public async Task SendMessageToChat(int auction, string message)
        {
            //await this.DatabaseManager.SaveChatHistory(chatRoomName, message).ConfigureAwait(false);

            //await this.Clients.Group(chatRoomName).BroadcastMessageAsync(message);
        }
    }
}