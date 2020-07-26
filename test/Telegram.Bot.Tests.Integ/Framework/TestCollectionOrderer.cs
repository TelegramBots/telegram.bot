using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Telegram.Bot.Tests.Integ.Framework
{
    // ReSharper disable once UnusedMember.Global
    public class TestCollectionOrderer : ITestCollectionOrderer
    {
        private readonly string[] _orderedCollections =
        {
            // Tests that require user interaction:
            Constants.TestCollections.CallbackQuery,
            Constants.TestCollections.PrivateChatReplyMarkup,
            Constants.TestCollections.InlineMessageLiveLocation,
            Constants.TestCollections.InlineQuery,
            Constants.TestCollections.EditMessage,
            Constants.TestCollections.EditMessageMedia,
            Constants.TestCollections.DeleteMessage,
            Constants.TestCollections.Stickers,
            Constants.TestCollections.Games,
            Constants.TestCollections.Payment,
            Constants.TestCollections.ChatMemberAdministration,
            Constants.TestCollections.Exceptions,
            Constants.TestCollections.NativePolls,

            // Tests without the need for user interaction:
            Constants.TestCollections.GettingUpdates,
            Constants.TestCollections.BotCommands,
            Constants.TestCollections.Dice,
            Constants.TestCollections.Webhook,
            Constants.TestCollections.SendTextMessage,
            Constants.TestCollections.SendAudioMessage,
            Constants.TestCollections.SendContactMessage,
            Constants.TestCollections.SendVenueMessage,
            Constants.TestCollections.SendDocumentMessage,
            Constants.TestCollections.SendAnimationMessage,
            Constants.TestCollections.SendPhotoMessage,
            Constants.TestCollections.SendVideoMessage,
            Constants.TestCollections.AlbumMessage,
            Constants.TestCollections.ReplyMarkup,
            Constants.TestCollections.LiveLocation,
            Constants.TestCollections.FileDownload,
            Constants.TestCollections.ChatInfo,
            Constants.TestCollections.LeaveChat,
            Constants.TestCollections.GetUserProfilePhotos,
            Constants.TestCollections.EditMessage2,
            Constants.TestCollections.EditMessageMedia2,
            Constants.TestCollections.DeleteMessage2,
            Constants.TestCollections.Games2,
            Constants.TestCollections.GameException,
            Constants.TestCollections.SupergroupAdminBots,
            Constants.TestCollections.ChannelAdminBots,
            Constants.TestCollections.Exceptions2,
        };

        public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
        {
            testCollections = testCollections.OrderBy(FindExecutionOrder);

            foreach (var collection in testCollections)
            {
                yield return collection;
            }
        }

        private int FindExecutionOrder(ITestCollection collection)
        {
            int? order = null;
            for (int i = 0; i < _orderedCollections.Length; i++)
            {
                if (_orderedCollections[i] == collection.DisplayName)
                {
                    order = i;
                    break;
                }
            }

            if (order is null)
            {
                throw new ArgumentException(
                    $"Collection \"{collection.DisplayName}\" not found in execution list.",
                    nameof(collection)
                );
            }

            return (int) order;
        }
    }
}
