using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using SignalRChat.Hubs;
using NSubstitute;
using System.Linq;

namespace SignalRChat.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup() {
		}

		[Test]
		public async Task TestUsingMoq() {
			var testUserName = Guid.NewGuid().ToString();
			var testMessage = Guid.NewGuid().ToString();
			using (var hub = new ChatHub()) {
				var clients = new Mock<IHubCallerClients>();
				var proxy = new Mock<IClientProxy>();
				var p = proxy.Object;
				clients.Setup(c => c.All).Returns(p);
				hub.Clients = clients.Object;
				try {
					await hub.SendMessage(testUserName, testMessage);
					proxy.Verify(p => p.SendCoreAsync(
							hub.MessageMethod,
							new object[] { testUserName, testMessage },
							default(CancellationToken)
							),
						Times.Once);
				} catch (Exception exception) {
					Assert.Fail("TEst failed: " + exception.Message);
				}
			}
			Assert.Pass();
		}

		[Test]
		public async Task TestUsingSubstitute() {
			var testUserName = Guid.NewGuid().ToString();
			var testMessage = Guid.NewGuid().ToString();
			var sampleMessageArray = new object[] { testUserName, testMessage };
			using (var hub = new ChatHub()) {
				var clients = Substitute.For<IHubCallerClients>();
				var proxy = Substitute.For<IClientProxy>();
				clients.All.Returns(proxy);
				hub.Clients = clients;
				hub.SendMessage(testUserName, testMessage).Wait();
				await proxy.Received()
					.SendCoreAsync(
						hub.MessageMethod,
						Arg.Is<object[]>(a => a.SequenceEqual(sampleMessageArray)),
						Arg.Any<CancellationToken>()
					); ;

			}
			Assert.Pass();
		}
	}
}