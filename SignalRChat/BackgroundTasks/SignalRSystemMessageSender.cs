using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;

namespace SignalRChat.BackgroundTasks
{
	internal class SignalRSystemMessageSender : ISystemMessageSender
	{
		private readonly IHubContext<ChatHub> hub;

		public SignalRSystemMessageSender(IHubContext<ChatHub> hub) {
			this.hub = hub;
		}

		public async Task SendMessage(string user, string message) {
			await hub.Clients.All.SendAsync("ReceiveMessage", user, message);
		}
	}
}
