namespace SignalRChat.BackgroundTasks
{
	internal class SignalRNotificationSender : INotificationSender
	{
		private readonly IEnumerable<ISystemMessageSender> senders;

		public SignalRNotificationSender(IEnumerable<ISystemMessageSender>  senders) {
			this.senders = senders;
		}

		public async Task Send(string message) {

			var tasks = senders.Select(async s => await s.SendMessage($"*** SYSTEM *** ", message));
			await Task.WhenAll(tasks);
		}
	}
}
