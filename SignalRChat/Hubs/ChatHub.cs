using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
	internal class ChatHub : Hub
	{
		internal string MessageMethod { get; } = "ReceiveMessage";

		public async Task SendMessage(string user, string message) {
			if (Clients?.All == null) {
				await Task.CompletedTask;
			} else {
				await Clients.All.SendAsync(MessageMethod, user, message);
			}
			
		}
	}
}
