﻿using Microsoft.Extensions.Configuration;
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
using Bot.Models;

namespace Bot
{
    internal class TelegramBot
    {
        private TelegramBotClient _telegramBotClient;
        private readonly TelegramBotOptions _options;
        private readonly ILogger<TelegramBot> _logger;
        private CancellationTokenSource _cts;
        private readonly ResponseProvider _responseProvider;
       
        public TelegramBot(ILogger<TelegramBot> logger, TelegramBotOptions options, ResponseProvider responseProvider)
        {
            _logger = logger;
            _options = options;
            _responseProvider = responseProvider;
        }

        public async Task InitBot()
        {
            if(_telegramBotClient != null)
            {
                return;
            }

            _telegramBotClient = new TelegramBotClient(_options.Token);

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
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            {
                return;
            }

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
           
            _logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

            string response = _responseProvider.GetResponseMessage(messageText);            

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: response,
                cancellationToken: cancellationToken);

            _logger.LogInformation($"Sent reply to chat {chatId}.");
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
