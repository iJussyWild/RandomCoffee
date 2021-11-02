using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

namespace RandomCoffee.Services
{
	public class TelegramBot : BackgroundService
	{
		private static TelegramBotClient _client;
		private static int _offset;

		private readonly MeetingService _meetingService;

		public TelegramBot(IConfiguration config, MeetingService meetingService)
		{
			_meetingService = meetingService;
			_client = new TelegramBotClient(config.GetValue("BotToken", string.Empty));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _client.SetWebhookAsync(string.Empty, cancellationToken: stoppingToken);

			try
			{
				while (true)
				{
					await CheckUpdates(stoppingToken);
					await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
				}
			}
			catch (OperationCanceledException e)
			{
				Log.Error(e, "Canceled");
				throw;
			}
			catch (Exception e)
			{
				Log.Error(e, "Error");
				throw;
			}
		}

		private async Task CheckUpdates(CancellationToken stoppingToken)
		{
			var updates = await _client.GetUpdatesAsync(_offset, cancellationToken:stoppingToken);
			foreach(var update in updates)
			{
				var message = update.Message;
				if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
				{
					var response = string.Empty;
					if (message.Text == "/start")
					{
						response = "Hello";
					}
					else if (message.Text == "/help")
					{
						response = $"Commands:{Environment.NewLine} /meeting - create meeting";
					}
					else if (message.Text == "/meeting")
					{
						try
						{
							var meeting = await _meetingService.CreateMeetingAsync();
							foreach (var person in meeting.Persons)
								response += $"{person.Name} {person.LastName}{Environment.NewLine}";
						}
						catch (Exception e)
						{
							Log.Error(e, "Error");
						}
					}

					await _client.SendTextMessageAsync(
						message.Chat.Id,
						response,
						replyToMessageId: message.MessageId,
						cancellationToken: stoppingToken);
				}
				_offset = update.Id + 1;
			}
		}
	}
}
