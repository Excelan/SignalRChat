namespace SignalRChat.BackgroundTasks
{
	/// <summary>
	/// Notifies when configuration changes are made on server.
	/// </summary>
	internal class ConfigurationNotifierService : BackgroundService
	{
		private readonly INotificationSender _sender;

		public ConfigurationNotifierService(INotificationSender sender) =>
			_sender = sender;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			while (!stoppingToken.IsCancellationRequested) {
				await _sender.Send($"Worker running at: {DateTimeOffset.Now}");
				await Task.Delay(10000, stoppingToken);
			};
		}
	}
}
