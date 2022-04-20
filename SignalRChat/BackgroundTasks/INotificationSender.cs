namespace SignalRChat.BackgroundTasks
{
	internal interface INotificationSender
	{
		Task Send(string message);
	}
}