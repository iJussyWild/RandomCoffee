using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RandomCoffee.Services
{
	public class TelegramBot : BackgroundService
	{
		private readonly TelegramBotClient _client;
		private readonly UpdateHandler _updateHandler;

		private int _nextUpdateId;
		private bool _isFirstPass = true;

		public TelegramBot(IConfiguration config, UpdateHandler updateHandler)
		{
			_updateHandler = updateHandler;
			_client = new TelegramBotClient(config.GetValue("BotToken", string.Empty));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _client.SetWebhookAsync(string.Empty, cancellationToken: stoppingToken);

			try
			{
				while (true)
				{
					var updates = await GetUpdatesAsync(stoppingToken);
					await HandleUpdatesAsync(updates, stoppingToken);
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

		private async Task<Update[]> GetUpdatesAsync(CancellationToken stoppingToken)
		{
			var updates = await _client.GetUpdatesAsync(_nextUpdateId, cancellationToken:stoppingToken);

			var isFirstPass = _isFirstPass;
			_isFirstPass = false;
			if (updates == null || updates.Length == 0)
				return Array.Empty<Update>();

			_nextUpdateId = updates.Last().Id + 1;

			if (isFirstPass)
			{
				Log.Information("Skip first pass");
				return Array.Empty<Update>();
			}

			return updates;
		}

		private async Task HandleUpdatesAsync(Update[] updates, CancellationToken stoppingToken)
		{
			foreach(var update in updates)
			{
				var response = await _updateHandler.HandleUpdate(update);
				await _client.SendTextMessageAsync(
					update.Message.Chat.Id,
					response,
					replyToMessageId: update.Message.MessageId,
					cancellationToken: stoppingToken);
			}
		}
	}
}
