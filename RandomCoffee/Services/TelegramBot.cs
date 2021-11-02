﻿using System;
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
					var response = "Bad command";
					switch (message.Text)
					{
						case "/start":
							response = "Hello";
							break;
						case "/help":
							response = $"Commands:{Environment.NewLine}" +
							           $"/meeting - create meeting {Environment.NewLine}" +
							           $"/show - show next meeting (for test)";
							break;
						case "/create" or "/show":
							try
							{
								response = string.Empty;
								var isNeedSave = message.Text == "/create";
								var meeting = await _meetingService.CreateMeetingAsync(isNeedSave);
								foreach (var person in meeting.Persons)
									response += $"{person.Name} {person.LastName}{Environment.NewLine}";
							}
							catch (Exception e)
							{
								response = "Error";
								Log.Error(e, "Error");
							}
							break;
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
