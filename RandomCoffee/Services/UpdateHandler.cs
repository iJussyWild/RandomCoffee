using System;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Types;

namespace RandomCoffee.Services
{
	public class UpdateHandler
	{
		private readonly MeetingService _meetingService;

		public UpdateHandler(MeetingService meetingService)
		{
			_meetingService = meetingService;
		}

		public async Task<string> HandleUpdate(Update update)
		{
			var message = update.Message;
			var response = "Bad command";

			if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
				return response;

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

			return response;
		}
	}
}
