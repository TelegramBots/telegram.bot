using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Tests.Integ.Framework
{
    public class UpdateReceiver
    {
        public ICollection<string> AllowedUsernames { get; }

        private readonly ITelegramBotClient _botClient;

        public UpdateReceiver(ITelegramBotClient botClient, IEnumerable<string> allowedUsernames)
        {
            _botClient = botClient;
            AllowedUsernames = new List<string>(allowedUsernames);
        }

        public async Task DiscardNewUpdatesAsync(CancellationToken cancellationToken = default)
        {
            CancellationTokenSource cts = default;

            try
            {
                if (cancellationToken == default)
                {
                    cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    cancellationToken = cts.Token;
                }

                int offset = -1;

                while (!cancellationToken.IsCancellationRequested)
                {
                    var updates = await _botClient.GetUpdatesAsync(
                        offset,
                        allowedUpdates: Array.Empty<UpdateType>(),
                        cancellationToken: cancellationToken
                    );

                    if (updates.Length == 0) break;

                    offset = updates.Last().Id + 1;
                }
            }
            finally
            {
                cts?.Dispose();
            }
        }

        public async Task<Update> GetUpdateAsync(
            Func<Update, bool> predicate = default,
            int offset = default,
            CancellationToken cancellationToken = default,
            params UpdateType[] updateTypes) =>
            (await GetUpdatesAsync(predicate, offset, cancellationToken, updateTypes))
            .First();

        public async Task<Update[]> GetUpdatesAsync(
            Func<Update, bool> predicate = default,
            int offset = default,
            CancellationToken cancellationToken = default,
            params UpdateType[] updateTypes)
        {
            CancellationTokenSource cts = default;
            predicate ??= PassthroughPredicate;

            try
            {
                if (cancellationToken == default)
                {
                    cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));
                    cancellationToken = cts.Token;
                }

                Update[] matchingUpdates = Array.Empty<Update>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    Update[] updates = await GetOnlyAllowedUpdatesAsync(
                        offset,
                        cancellationToken,
                        updateTypes
                    );

                    matchingUpdates = updates
                        .Where(u => updateTypes.Contains(u.Type) && predicate(u))
                        .ToArray();

                    if (matchingUpdates.Any()) break;

                    offset = updates.LastOrDefault()?.Id + 1 ?? 0;
                    await Task.Delay(TimeSpan.FromSeconds(1.5), cancellationToken);
                }

                return matchingUpdates;
            }
            finally
            {
                cts?.Dispose();
            }

            bool PassthroughPredicate(Update _) => true;
        }

        public async Task<Update> GetCallbackQueryUpdateAsync(
            int messageId = default,
            string data = default,
            bool discardNewUpdates = true,
            CancellationToken cancellationToken = default)
        {
            Func<Update, bool> predicate = u =>
                (messageId == default || u.CallbackQuery!.Message!.MessageId == messageId) &&
                (data == default || u.CallbackQuery!.Data == data);

            var updates = await GetUpdatesAsync(predicate,
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.CallbackQuery);

            if (discardNewUpdates)
                await DiscardNewUpdatesAsync(cancellationToken);

            var update = updates.First();
            return update;
        }

        public async Task<Update> GetInlineQueryUpdateAsync(
            bool discardNewUpdates = true,
            CancellationToken cancellationToken = default)
        {
            var updates = await GetUpdatesAsync(
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.InlineQuery);

            if (discardNewUpdates)
                await DiscardNewUpdatesAsync(cancellationToken);

            var update = updates.First();
            return update;
        }

        /// <summary>
        /// Receive the chosen inline query result and the message that was sent to chat. Use this
        /// method only after bot answers to an inline query.
        /// </summary>
        /// <param name="messageType">
        /// Type of message for chosen inline query e.g. Text message for article results
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Message update generated for chosen result, and the update for chosen inline query result
        /// </returns>
        public async Task<(Update MessageUpdate, Update ChosenResultUpdate)> GetInlineQueryResultUpdates(
            MessageType messageType,
            CancellationToken cancellationToken = default)
        {
            Update messageUpdate = default;
            Update chosenResultUpdate = default;

            while (
                !cancellationToken.IsCancellationRequested &&
                (messageUpdate is null || chosenResultUpdate is null)
            )
            {
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                var updates = await GetUpdatesAsync(
                    u => u.Message?.Type == messageType || u.ChosenInlineResult != null,
                    cancellationToken: cancellationToken,
                    updateTypes: new [] { UpdateType.Message, UpdateType.ChosenInlineResult }
                );

                messageUpdate = updates.SingleOrDefault(u => u.Message?.Type == messageType);
                chosenResultUpdate = updates.SingleOrDefault(u => u.Type == UpdateType.ChosenInlineResult);
            }

            return (messageUpdate, chosenResultUpdate);
        }

        private async Task<Update[]> GetOnlyAllowedUpdatesAsync(
            int offset,
            CancellationToken cancellationToken,
            params UpdateType[] types)
        {
            var updates = await _botClient.GetUpdatesAsync(
                offset,
                allowedUpdates: types,
                cancellationToken: cancellationToken);

            var allowedUpdates = updates.Where(IsAllowed).ToArray();
            return allowedUpdates;
        }

        private bool IsAllowed(Update update)
        {
            if (AllowedUsernames is null || AllowedUsernames.All(string.IsNullOrWhiteSpace))
                return true;

            bool isAllowed;
            switch (update.Type)
            {
                case UpdateType.Message:
                case UpdateType.InlineQuery:
                case UpdateType.CallbackQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.ShippingQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.PollAnswer:
                    isAllowed = AllowedUsernames.Contains(
                        update.GetUser().Username,
                        StringComparer.OrdinalIgnoreCase
                    );
                    break;
                case UpdateType.Poll:
                    isAllowed = true;
                    break;
                case UpdateType.EditedMessage:
                case UpdateType.ChannelPost:
                case UpdateType.EditedChannelPost:
                    isAllowed = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return isAllowed;
        }
    }
}
