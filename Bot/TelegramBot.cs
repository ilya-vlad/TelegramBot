using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Bot.Services;

namespace Bot
{
    internal class TelegramBot
    {
        private TelegramBotClient _telegramBotClient;
        private readonly IConfiguration _config;
        private readonly ILogger<TelegramBot> _logger;
        private CancellationTokenSource _cts;
        private ResponseProvider _responseProvider;

        public TelegramBot(ILogger<TelegramBot> logger, IConfiguration config, ResponseProvider responseProvider)
        {
            _logger = logger;
            _config = config;
            _responseProvider = responseProvider;
        }

        public async Task InitBot()
        {
            if(_telegramBotClient != null)
            {
                return;
            }

            _telegramBotClient = new TelegramBotClient(_config.GetValue<string>("Telegram:Token"));

            _cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, 
                receiverOptions, cancellationToken: _cts.Token);

            var me = await _telegramBotClient.GetMeAsync();

            _logger.LogInformation($"Start listening for bot - @{me.Username}");

            Console.ReadLine();
            _cts.Cancel();            
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;            
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
           
            _logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

            string response = _responseProvider.GetResponseMessage(chatId, messageText);            

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: response,
                cancellationToken: cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(ErrorMessage);
            
            return Task.CompletedTask;
        }
    }
}
