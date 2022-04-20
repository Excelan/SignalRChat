namespace SignalRChat.BackgroundTasks
{
	internal interface ISystemMessageSender
	{
		public Task SendMessage(string user, string message);

	}
}