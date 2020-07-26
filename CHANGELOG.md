# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/)
and this project adheres to [Semantic Versioning](https://semver.org/).

<!--

## [Unreleased]

### Added

### Changed

### Fixed

### Removed

-->

## [Unreleased]

### Added

- Constructor to `ApiRequestEventArgs`
- Constructor to `ApiRequestEventArgs`
- Property `bool IRequest.IsWebhookResponse`
- New base exception type `RequestException`
- Method `ITelegramBotClient.SendRequestAsync` which returns `ApiResponse<T>` as is without throwing
exception `ApiRequestException`
- Interface `IExceptionParser` for implementing custom parsers for exception, default
implementation of `IExceptionParser` throws all correct errors from Bot Api as `ApiRequestException`
- Type `Telegram.Bot.Exceptions.ApiResponse`
- Property `ITelegramBotClient.ExceptionParser`
- Interface `IUserTargetable`
- Request classes with a property `int UserId { get; }` implement `IUserTargetable` 

### Changed
- All optional Bot API properties are made nullable 
- Optional properties with reference types are made nullable (C# 8.0 is needed to make use of this 
feature)
- Type `Animation` extends `FileBase`
- `MessageEntityType.Unknown` moved to the top and made default enum value
- `fileName` parameter in constructor `InputFileStream` is made optional
- `fileName` parameter in constructor `InputOnlineFile` is made optional
- `fileName` parameter in constructor `InputTelegramFile` is made optional
- Default constructor in `LabeledPrice` is made private
- Default constructor in `KeyboardButton` is made private
- Default constructor in `KeyboardButtonPollType` is made private
- Default constructor in `ReplyKeyboardMarkup` is made private
- All methods from `ITelegramBotClient` that map to Bot API requests moved into
`TelegramClientExtensions` as extension methods
- Parameter order in `SendDiceAsync`
- Parameter order in `SendPollAsync`
- Parameter order in `SendAudioAsync`
- Parameter order in `SendDocumentAsync`
- Parameter order in `SendVideoNoteAsync`
- Parameter order in `SendVenueAsync`
- Parameter order in `SendContactAsync`
- `ApiRequestException` extends `RequestException`
- All error responses from Bot API which contain correct JSON string are now thrown as
`ApiRequestException` from `ITelegramBotClient.MakeRequestException`
- All other exceptions during request sending logic are wrapped inside `RequestException` 
except `TaskCancelledException` that is triggered by token cancellation and errors from 
`MakingApiRequest` and `ApiResponseReceived` event handlers
- Interface `IChatMessage` renamed into `IChatTargetable`
- Requests with a property `long ChatId { get; }` explicitly implement `IChatTargetable`
- Changed error handling in `DownloadFileAsync`: `ApiRequestException` will be thrown when 
correct error response is received from Bot API, otherwise `RequestException` is thrown,
`TaskCancelledException` is thrown when cancellation is requested
- All properties that accept `float` values changed to accept `double`

### Removed
- Enum member `ParseMode.Default`
- Following interfaces:
    - `IChatMessage`
    - `IFormattableMessage`
    - `IChatMessage`
    - `IInlineMessage`
    - `IInlineReplyMarkupMessage`
    - `INotifiableMessage`
    - `IReplyMarkupMessage`
    - `IReplyMessage`
    - `IThumbMediaMessage`
    - `ICaptionInlineQueryResult`
    - `IInputMessageContentResult`
    - `ILocationInlineQueryResult`
    - `IThumbnailInlineQueryResult`
    - `IThumbnailUrlInlineQueryResult`
    - `ITitleInlineQueryResult`
- Constructor `InputFileStream` that accepted only `Stream` parameter
- Constructor `InputOnlineFile` that accepted only `Stream` parameter
- Constructor `InputTelegramFile` that accepted only `Stream` parameter
- Obsolete `SendMediaGroupAsync` that accepted an array of `InputMediaBase`
- All methods, properties and events related to polling, including:
    - Property `ITelegramBotClient.IsReceiving`
    - Property `ITelegramBotClient.MessageOffset `
    - Event `OnUpdate`
    - Event `OnMessage`
    - Event `OnMessageEdited`
    - Event `OnInlineQuery`
    - Event `OnInlineResultChose`
    - Event `OnCallbackQuery`
    - Event `OnReceiveError`
    - Event `OnReceiveGeneralError`
    - Method `StartReceiving`
    - Method `StopReceiving`
- All exception types except `ApiRequestException`
- Property `Chat.AllMembersAreAdministrators`
- Property `Message.IsForwarded`

## [15.7.1] - 2020-06-18

### Added
- Source Link support
- Fully deterministic build

## [15.7.0] - 2020-06-13

### Added
- Enum member `Emoji.Basketball`
- Property `InlineQueryResultGif.ThumbMimeType`
- Property `InlineQueryResultMpeg4Gif.ThumbMimeType`
- Property `Message.ViaBot`

## [15.6.0] - 2020-05-30

### Added
- Enum `Emoji`
- Property `Poll.Explanation`
- Property `Poll.ExplanationEntities`
- Property `Poll.OpenPeriod`
- Property `Poll.CloseDate`
- Property `Dice.Emoji`
- Following optional properties to `SendPollRequest`:
    - `Explanation`
    - `ExplanationParseMode`
    - `OpenPeriod`
    - `CloseDate`
- Optional property `Emoji` to `SendDiceRequest`

### Changed
- Following optional parameters to `ITelegramBotClient.SendPollAsync`:
     - `explanation`
     - `explanationParseMode`
     - `openPeriod`
     - `closeDate`
- Optional parameter `emoji` to `ITelegramBotClient.SendDiceAsync`

## [15.5.1] - 2020-04-02

### Fixed
- Implementation of `ITelegramBotClient.CreateNewAnimatedStickerSetAsync`

## [15.5.0] - 2020-04-02

### Added
- Requests
    - `GetMyCommandsRequest`
    - `SetMyCommandsRequest`
    - `CreateNewAnimatedStickerSetRequest`
    - `AddNewAnimatedStickerToSetRequest`
    - `SendDiceRequest`
    - `SetStickerSetThumbRequest`
- Methods:
    - `ITelegramBotClient.SendDiceAsync`
    - `ITelegramBotClient.CreateNewAnimatedStickerSetAsync`
    - `ITelegramBotClient.AddNewAnimatedStickerToSetAsync`
    - `ITelegramBotClient.SetStickerSetThumbAsync`
    - `ITelegramBotClient.GetMyCommandsAsync`
    - `ITelegramBotClient.SetMyCommandsAsync`
- Type `Dice`
- Type `BotCommand`
- Enum member `MessageType.Dice`
- Property `Message.Dice`
- Property `StickerSet.Thumb`

## [15.4.0] - 2020-02-22

### Added
- Property `SendInvoiceRequest.SendPhoneNumberToProvider`
- Property `SendInvoiceRequest.SendEmailToProvider `
- Optional parameter `sendPhoneNumberToProvider` to method `ITelegramBotClient.SendInvoiceAsync`
- Optional parameter `sendEmailToProvider` to method `ITelegramBotClient.SendInvoiceAsync`

## [15.3.0] - 2020-01-31

### Added

- Type `KeyboardButtonPollType`
- Static method `KeyboardButton.WithRequestPoll`
- Type `PollAnswer`
- Property `KeyboardButton.RequestPoll`
- Enum `PollType`
- Property `MessageEntity.Language`
- Following properties to type `Poll`:
    - `bool? IsAnonymous`
    - `string Type`
    - `bool? AllowsMultipleAnswers`
    - `int? CorrectOptionId`
- Following properties to type `SendPollRequest`:
    - `bool? IsAnonymous`
    - `string Type`
    - `bool? AllowsMultipleAnswers`
    - `int? CorrectOptionId`
    - `bool? IsClosed`
- Property `Update.PollAnswer`
- Enum member `UpdateType.PollAnswer`
- Following properties to type `User`:
    - `bool? CanJoinGroups`
    - `bool? CanReadAllGroupMessages`
    - `bool? SupportsInlineQueries`

### Changed

- Method `ITelegramBotClient.SendPollAsync`, added following optional parameters:
    - `bool? isAnonymous`
    - `string type`
    - `bool? allowsMultipleAnswers`
    - `int? correctOptionId`
    - `bool? isClosed`
- Method `User.Equals` takes into account new properties

## [15.2.1] - 2020-01-23

### Changed

- All base request classes use explicit opt-in serialization strategy
- All non Bot API properties in base request classes are annotated by `JsonIgnoreAttribute`
- Type `ContactRequestException` is made obsolete due to Telegram changed it's error message

### Fixed
- A bug that prevented requests to be serialized when default `JsonSerializerSettings` were set

## [15.2.0] - 2020-01-03

### Added

- Type `SetChatAdministratorCustomTitleRequest`
- Method `ITelegramBotClient.SetChatAdministratorCustomTitleAsync`
- Property `FileBase.FileUniqueId`
- Property `Animation.FileUniqueId`
- Property `ChatPhoto.BigFileUniqueId`
- Property `ChatPhoto.SmallFileUniqueId`
- Property `Chat.SlowModeDelay`
- Property `ChatMember.CustomTitle`
- Enum value `ParseMode.MarkdownV2`
- Enum value `MessageEntityType.Underline`
- Enum value `MessageEntityType.Strikethrough`

## [15.1.0] - 2019-11-29

### Added

- Property `RequestBase.IsWebhookResponse`
- The client outputs `method` property in the resulting HTTP body with the value from `IRequest<T>.MethodName` when `RequestBase.IsWebhookResponse` is set to true
- Constructor with `Uri` param for `InputOnlineFile` (overloaded)

## [15.0.0] - 2019-08-07

### Added

- Type `ChatPermissions`
- Type `SetChatPermissionsRequest`
- Property `Sticker.IsAnimated`
- Property `StickerSet.IsAnimated`
- Property `Chat.Permissions`
- Property `ChatMember.CanSendPolls`

### Changed

- Individual permission properties in `RestrictChatMemberRequest` changed to `Permissions` property of type `ChatPermissions`
- Individual permission parameters in `ITelegramBotClient.RestrictChatMemberAsync` changed to a single parameter of type `ChatPermissions`
- Marked `Chat.AllMembersAreAdministrators` as obsolete

### Fixed

- XML doc comments about caption maximum length

## [14.12.0] - 2019-06-10

### Added

- Type `LoginUrl`
- Property `Message.ReplyMarkup`
- Property `InlineKeyboardButton.LoginUrl`
- Method `InlineKeyboardButton.WithLoginUrl`

## [14.11.0] - 2019-04-23

### Added

- Type `Poll`
- Type `PollOption`
- Type `SendPollRequest`
- Type `StopPollRequest`
- Method `SendPollAsync`
- Method `StopPollAsync`
- Property `Update.Poll`
- Property `Message.Poll`
- Property `Message.ForwardSenderName`
- Property `ChatMember.IsMember`
- Enum value `UpdateType.Poll`
- Enum value `MessageType.Poll`

### Changed

- Marked `InvalidQueryIdException` as obsolete

## [14.10.0] - 2018-09-04

### Added

- Telegram Passport support
- Type `EncryptedCredentials`
- Type `EncryptedPassportElement`
- Type `PassportData`
- Type `PassportFile`
- Property `ITelegramBotClient.BotId`
- Property `Message.PassportData`

### Changed

- Marked `DownloadFileAsync(string,CancellationToken)` obsolete

## [14.9.0] - 2018-08-06

### Added

- Method `SendAnimationAsync`
- Parameter `thumb` to method `SendAudioAsync`
- Parameter `thumb` to method `SendDocumentAsync`
- Parameter `thumb` to method `SendVideoAsync`
- Parameter `thumb` to method `SendVideoNoteAsync`
- Type `IChatMessage`
- Type `IThumbMediaMessage`
- Type `SendAnimationRequest`
- Property `SendAudioRequest.Thumb`
- Property `SendDocumentRequest.Thumb`
- Property `SendVideoRequest.Thumb`
- Property `SendVideoNoteRequest.Thumb`
- Property `Audio.Thumb`

## [14.8.0] - 2018-07-29

### Added

- Parameter `vCard` to method `SendContactAsync`
- Parameter `foursquareType` to method `SendVenueAsync`
- Property `SendContactRequest.Vcard`
- Property `SendVenueRequest.FoursquareType`
- Property `InlineQueryResultContact.Vcard`
- Property `InlineQueryResultVenue.FoursquareType`
- Property `InputContactMessageContent.Vcard`
- Property `InputVenueMessageContent.FoursquareType`
- Property `Contact.Vcard`
- Property `Venue.FoursquareType`
- Enum value `MessageEntityType.Cashtag`

### Changed

- Marked `MessageType.Animation` as obsolete

## [14.7.0] - 2018-07-29

### Added

- Partial support for Bot API v4.0
- Method `SendMediaGroupAsync` (overloaded)
- Method `EditMessageMediaAsync`
- Method `EditMessageMediaAsync`
- Type `EditMessageMediaRequest`
- Type `EditInlineMessageMediaRequest`
- Type `InputMediaAudio`
- Type `InputMediaDocument`
- Type `InputMediaAnimation`
- Type `IInputMedia`
- Type `IAlbumInputMedia`
- Type `IInputMediaThumb`
- Property `Animation.Width`
- Property `Animation.Height`
- Property `Animation.Duration`
- Enum member `MessageType.Animation`
- Property `InputMediaVideo.Thumb`
- Property `Message.Animation`
- Constructor with required parameters for `InputMediaPhoto` (overloaded)
- Constructor with required parameters for `InputMediaVideo` (overloaded)
- Constructor with required parameters for `SendMediaGroupRequest` (overloaded)

### Fixed

- Serialization error of `InlineQueryResultDocument.Description` being required
- Incorrect serialization of `InputMediaVideo`

### Changed

- Marked method `SendMediaGroupAsync` as obsolete. An overload is provided.
- Marked parameterless constructor of `InputMediaPhoto` as obsolete. An overload is provided.
- Marked parameterless constructor of `InputMediaVideo` as obsolete. An overload is provided.

## [14.6.0] - 2018-06-12

### Added

- Property `ParseMode` to requests with a caption
  - `EditMessageCaptionRequest`
  - `EditInlineMessageCaptionRequest`
- Parameter `parseMode` to method `ITelegramBotClient.EditMessageCaptionAsync`

## [14.5.0] - 2018-06-06

### Added

- New enum member `MessageEntityType.PhoneNumber`
- New enum member `MessageEntityType.Unknown`

### Fixed

- Exception during deserialization of unknown message entity type

## [14.4.0] - 2018-05-17

### Changed

- `MakeRequestAsync` throws `ApiRequestException` with `ErrorCode = HttpStatusCode.Unauthorized` and `Message = apiResponse.Description` ("Unauthorized"), to be consistent with Telegram Bot API
- `TelegramBotClient` ctor does not check API token format: Telegram Bot API does not provide token format specification
- `TestApiAsync` return `false` when `ApiRequestException.ErrorCode == 401` (API Token is modified or recalled)
- Stop catching user exceptions from event handlers
- Make `Message.IsForward` property obsolete

### Fixed

- `EditMessageTextAsync` pass `ParseMode` to request

## [14.3.0] - 2018-05-05

### Added

- Implicit cast of `IEnumerable<InlineKeyboardButton>[]` to `InlineKeyboardMarkup`
- Implicit cast of `InlineKeyboardButton[]` to `InlineKeyboardMarkup`
- Exception `InvalidGameShortNameException`
- Exception `InvalidQueryIdException`

### Changed

- Made `callbackGame` parameter of `InlineKeyboardButton.WithCallBackGame` optional
- `Newtonsoft.Json` updated to version `11.0.2`

### Fixed

- Assigining param `foursquareId` of `SendInvoiceAsync` method to its request
- Access modifier of abstract class `BadRequestException` and `ForbiddenException` ctors to `protected`

### Removed

- Parameterless ctor of `InlineKeyboardMarkup`
- Exception `BotBlockedException`
- Exception `BotRestrictedException`
- Exception `MissingParameterException`
- Exception `NotEnoughRightsException`
- Exception `WrongChatTypeException`

## [14.2.0-rc-452] - 2018-02-24

### Added

- Property `Message.MediaGroupId`
- Property `ICaptionInlineQueryResult.ParseMode`
- Property `ParseMode` to inline query results with a caption
  - `InlineQueryResultPhoto`
  - `InlineQueryResultGif`
  - `InlineQueryResultCachedMpeg4Gif`
  - `InlineQueryResultVideo`
  - `InlineQueryResultAudio`
  - `InlineQueryResultVoice`
  - `InlineQueryResultDocument`
  - `InlineQueryResultCachedPhoto`
  - `InlineQueryResultCachedGif`
  - `InlineQueryResultCachedMpeg4Gif`
  - `InlineQueryResultCachedDocument`
  - `InlineQueryResultVideo`
  - `InlineQueryResultCachedVoice`
  - `InlineQueryResultCachedAudio`

## [14.1.0-rc-424] - 2018-02-24

### Added

- Support for Bot API v3.6
- Override equality comparison for `User` type
- Property `ParseMode` to file requests with a caption
  - `SendVideoRequest`
  - `SendPhotoRequest`
  - `SendAudioRequest`
  - `SendDocumentRequest`
  - `SendVoiceRequest`
- Property `InputMediaBase.ParseMode`
- Property `SendVideoRequest.SupportsStreaming`
- Property `InputMediaVideo.SupportsStreaming`
- Property `Message.ConnectedWebsite`
- Parameter `parseMode` to methods
  - `ITelegramBotClient.SendVideoAsync`
  - `ITelegramBotClient.SendAudioAsync`
  - `ITelegramBotClient.SendPhotoAsync`
  - `ITelegramBotClient.SendDocumentAsync`
  - `ITelegramBotClient.SendVoiceAsync`
- Parameter `supportsStreaming` to method `ITelegramBotClient.SendVideoAsync`
- New members to enum `MessageType`
  - `WebsiteConnected`
  - `ChatMembersAdded`
  - `ChatMemberLeft`
  - `ChatTitleChanged`
  - `ChatPhotoChanged`
  - `MessagePinned`
  - `ChatPhotoDeleted`
  - `GroupCreated`
  - `SupergroupCreated`
  - `ChannelCreated`
  - `MigratedToSupergroup`
  - `MigratedFromGroup`
- Exception `MessageIsNotModifiedException`

### Changed

- Changed `InputMessageContent` to abstract class `InputMessageContentBase`
- Access modifier of parameterless ctors of all `InlineQueryResult` and `InputMessageContent` types to `private`

### Fixed

- Renamed `InputVenueMessageContent.Name` to `InputVenueMessageContent.Title`
- Property `Message.Type` returns correct value after group chat migration

### Removed

- Enum member `MessageType.Service`

## [14.0.0-rc-367] - 2018-01-04

### Added

- Type `InvalidParameterException`
- Type `FileBase`

### Changed

- Moved all types in namespace `Telegram.Bot.Types.InputMessageContents` to namespace `Telegram.Bot.Types.InlineQueryResults`
- Value names of enums `MessageType` and `UpdateType`

### Removed

- Property `File.Stream`
- Property `FilePath` from Types `Audio`, `Document`, `PhotoSize`, `Sticker`, `Video`, `VideoNote`, and `Voice`
- Property `Message.NewChatMember`

## [14.0.0-beta-342] - 2018-01-03

### Added

- More `PaymentTests` cases
- Type `IKeyboardButton`
- Implicit cast of `string[]` to `ReplyKeyboardMarkup`
- Implicit cast of `string[][]` to `ReplyKeyboardMarkup`
- Inline query results
  - Type `ICaptionInlineQueryResult`
  - Type `IInputMessageContentResult`
  - Type `ILocationInlineQueryResult`
  - Type `IThumbnailInlineQueryResult`
  - Type `IThumbnailUrlInlineQueryResult`
  - Type `ITitleInlineQueryResult`
  - Constructor with required parameters in `InlineQueryResult` and all derived classes
  - Property `InlineQueryResultVoice.Caption`

### Changed

- All keyboard button classes inherit `IKeyboardButton` interface
- All keyboard buttons moved to namespace `Telegram.Bot.Types.ReplyMarkups.Buttons`
- Renamed type `ReplyMarkup` to `ReplyMarkupBase`
- Renamed type `ForceReply` to `ForceReplyMarkup`
- `InlineQueryResult` to abstract

### Removed

- All specific `KeyboardButton` types
- All specific `InlineKeyboardButton` types
- Implicit cast of `InlineKeyboardButton` to `KeyboardButton`
- Type `InlineQueryResultNew`
- Type `InlineQueryResultCached`
- Property `InlineQueryResult.Title`
- Property `InlineQueryResult.InputMessageContent`
- Property `InlineQueryResultAudio.FileId`
- JSON serialization attribute `Required.Always` of property `InlineQueryResultAudio.Duration`

### Fixed

- Invalid default value for `SwitchInlineQueryCurrentChat` in `InlineKeyboardSwitchInlineQueryCurrentChatButton` constructor

## [14.0.0-alpha0] - 2017-12-31

### Added

- Request classes
  - Type `GetUpdatesRequest`
  - Type `SetWebhookRequest`
  - Type `DeleteWebhookRequest`
  - Type `GetWebhookInfoRequest`
  - Type `GetMeRequest`
  - Type `SendMessageRequest`
  - Type `ForwardMessageRequest`
  - Type `SendPhotoRequest`
  - Type `SendAudioRequest`
  - Type `SendDocumentRequest`
  - Type `SendVideoRequest`
  - Type `SendVoiceRequest`
  - Type `SendVideoNoteRequest`
  - Type `SendLocationRequest`
  - Type `EditMessageLiveLocationRequest`
  - Type `EditInlineMessageLiveLocationRequest`
  - Type `StopMessageLiveLocationRequest`
  - Type `StopInlineMessageLiveLocationRequest`
  - Type `SendVenueRequest`
  - Type `SendContactRequest`
  - Type `SendChatActionRequest`
  - Type `GetUserProfilePicturesRequest`
  - Type `KickChatMemberRequest`
  - Type `UnbanChatmemberRequest`
  - Type `RestrictChatMemberRequest`
  - Type `PromoteChatMemberRequest`
  - Type `ExportChatInviteLinkRequest`
  - Type `SetChatPhotoRequest`
  - Type `DeleteChatPhotoRequest`
  - Type `SetChatTitleRequest`
  - Type `SetChatDescriptionRequest`
  - Type `PinChatMessageRequest`
  - Type `UnpinChatMessageRequest`
  - Type `LeaveChatRequest`
  - Type `GetChatRequest`
  - Type `GetChatAdministratorsRequest`
  - Type `GetChatMembersCountRequest`
  - Type `GetChatMemberRequest`
  - Type `SetChatStickerSetRequest`
  - Type `DeleteChatStickerSetRequest`
  - Type `AnswerCallbackQueryRequest`
  - Type `EditMessageTextRequest`
  - Type `EditInlineMessageTextRequest`
  - Type `EditMessageCaptionRequest`
  - Type `EditInlineMessageCaptionRequest`
  - Type `EditMessageReplyMarkupRequest`
  - Type `EditInlineMessageReplyMarkupRequest`
  - Type `DeleteMessageRequest`
  - Type `SendStickerRequest`
  - Type `GetStickerSetRequest`
  - Type `SetStickerPositionInSetRequest`
  - Type `AnswerInlineQueryRequest`
  - Type `DeleteStickerFromSetRequest`
  - Type `AddStickerToSetRequest`
  - Type `CreateNewStickerSetRequest`
  - Type `SendInvoiceRequest`
  - Type `AnswerShippingQueryRequest`
  - Type `AnswerPreCheckoutQueryRequest`
  - Type `SendGameRequest`
  - Type `SetGameScoreRequest`
  - Type `SetInlineGameScoreRequest`
  - Type `GetGameHighScoresRequest`
  - Type `GetInlineGameHighScoresRequest`
  - Type `GetFileRequest`
  - Type `FileRequestBase`
  - Type `UploadStickerFileRequest`
  - Type `INotifiableMessage`
  - Type `IReplyMessage`
  - Type `IFormattableMessage`
  - Type `IInlineMessage`
  - Type `IReplyMarkupMessage`
  - Type `IInlineReplyMarkupMessage`
- Type `IInputFile`
- Type `InputFileStream`
- Type `InputTelegramFile`
- Type `InputOnlineFile`
- Type `InputFileConverter`

### Changed

- Type of parameter `allowedUpdates` in method `SetWebhookAsync` changed to `IEnumerable<UpdateType>`
- Type of parameter `allowedUpdates` in method `GetUpdatesAsync` changed to `IEnumerable<UpdateType>`
- Type of parameter `offset` in method `GetUserProfilePhotosAsync` changed to `int`
- Type of parameter `replyMarkup` in methods `EditMessageLiveLocationAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in methods `StopMessageLiveLocationAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in method `EditMessageTextAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in method `EditMessageCaptionAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in method `EditMessageReplyMarkupAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in method `SendGameAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `replyMarkup` in method `SendInvoiceAsync` changed to `InlineKeyboardMarkup`
- Type of parameter `prices` in method `SendInvoiceAsync` changed to `IEnumerable<LabeledPrice>`
- Type of parameter `shippingOptions` in method `AnswerShippingQueryAsync` changed to `IEnumerable<ShippingOption>`
- Type of parameter `chatId` in method `SendInvoiceAsync` changed to `int`
- Type of parameter `chatId` in method `SendGameAsync` changed to `long`
- Type of parameter `chatId` in methods `SetGameScoreAsync` changed to `long`
- Type of parameter `chatId` in methods `GetGameHighScoresAsync` changed to `long`
- Type parameter of type `SendMediaGroupRequest` changed to `Message[]`
- Return type of method `GetChatIdFromTesterAsync` in `TestFixture` changed to `Task<long>`
- Type of property `TesterPrivateChatId` on type `PaymentTestsFixture` changed to `long`
- Replace method `EditInlineMessageTextAsync` with overload for `EditMessageTextAsync`
- Replace method `EditInlineMessageCaptionAsync` with overload for `EditMessageCaptionAsync`
- Replace method `EditInlineMessageReplyMarkupAsync` with overload for `EditMessageReplyMarkupAsync`
- Reorder parameters of method `SendInvoiceAsync`
- Divide `AnswerShippingQueryAsync` method into two overloads
- Divide `AnswerPreCheckoutQueryAsync` method into two overloads
- Method `CreateNewStickerSetAsnyc` renamed to `CreateNewStickerSetAsync`
- Method return type changed from `Task<bool>` to `Task`:
  - Method `DeleteWebhookAsync`
  - Method `KickChatMemberAsync`
  - Method `LeaveChatAsync`
  - Method `UnbanChatMemberAsync`
  - Method `AnswerCallbackQueryAsync`
  - Method `RestrictChatMemberAsync`
  - Method `PromoteChatMemberAsync`
  - Method `StopMessageLiveLocationAsync`
  - Method `EditMessageTextAsync`
  - Method `EditMessageCaptionAsync`
  - Method `EditMessageReplyMarkupAsync`
  - Method `EditMessageLiveLocationAsync`
  - Method `DeleteMessageAsync`
  - Method `AnswerInlineQueryAsync`
  - Method `AnswerShippingQueryAsync`
  - Method `AnswerPreCheckoutQueryAsync`
  - Method `SetGameScoreAsync`
  - Method `CreateNewStickerSetAsync`
  - Method `AddStickerToSetAsync`
  - Method `SetStickerPositionInSetAsync`
  - Method `DeleteStickerFromSetAsync`
  - Method `SetChatPhotoAsync`
  - Method `DeleteChatPhotoAsync`
  - Method `SetChatTitleAsync`
  - Method `SetChatDescriptionAsync`
  - Method `PinChatMessageAsync`
  - Method `UnpinChatMessageAsync`
  - Method `SetChatStickerSetAsync`
  - Method `DeleteChatStickerSetAsync`
- Type of `StickerSet.Stickers` from `List<Sticker>` to `Sticker[]`
- Type of `ChatMember` properties to nullable e.g. `bool` to `bool?`
- Type of `Message.Entities` from `List<MessageEntity>` to `MessageEntity[]`
- Type of `Message.CaptionEntities` from `List<MessageEntity>` to `MessageEntity[]`
- Type of `Message.EntityValues` from `List<string>` to `IEnumerable<string>`
- Type `InputMediaType` to `InputMedia`
- Type `InputMediaTypeConverter` to `InputMediaConverter`
- Changed member type from `FileToSend` to an implementation of `IInputFile`
  - Parameter `certificate` of method `SetWebhookAsync`
  - Parameter `photo` of method `SendPhotoAsync`
  - Parameter `video` of method `SendVideoAsync`
  - Parameter `videoNote` of method `SendVideoNoteAsync`
  - Parameter `document` of method `SendDocumentAsync`
  - Parameter `sticker` of method `SendStickerAsync`
  - Parameter `pngSticker` of method `UploadStickerFileAsync`
  - Parameter `pngSticker` of method `CreateNewStickerSetAsync`
  - Parameter `pngSticker` of method `AddStickerToSetAsync`
  - Parameter `photo` of method `SetChatPhotoAsync`
  - Parameter `audio` of method `SendAudioAsync`

### Removed

- Type `FileToSend`
- Interface `IResponse`
- Type parameter constraint from interface `IRequest`
- Redundant parameter `editMessage` in methods `SetGameScoreAsync`
- Redundant custom converter `ParseModeConverter`
- Value `All` from `UpdateType` enum
- Value `Unknown` from `FileType` enum
- Default value of parameter `url` of method `SetWebhookAsync`
- Property `Width` of `VideoNote`
- Property `Height` of `VideoNote`
- Property `Zoom` of `MaskPosition`

### Fixed

- Method `SetWebhookAsync` interface `ITelegramBotClient` returns `bool` on success
- Passing wrong `chatId` value in method `GetGameHighScoresAsync`
- Passing wrong `chatId` value in method `SendGameAsync`
- Passing wrong `chatId` value in method `SendInvoiceAsync`
- Passing wrong `replyMarkup` value in method `SendGameAsync`
- Passing wrong `replyMarkup` value in method `SendInvoiceAsync`
- Passing wrong `replyMarkup` value in method `EditMessageLiveLocationAsync`
- Passing wrong `replyMarkup` value in method `EditMessageReplyMarkupAsync`
- Passing wrong `replyMarkup` value in method `EditMessageCaptionAsync`
- Passing wrong `replyMarkup` value in method `EditMessageTextAsync`
- Passing wrong `replyMarkup` value in method `StopMessageLiveLocationAsync`
- Passing wrong `pngSticker` value in method `UploadStickerFileAsync`
- Passing wrong `photo` value in method `SetChatPhotoAsync`
- Passing wrong `certificate` value in method `SetWebhookAsync`
- Incorrect spelling of the method `CreateNewStickerSetAsync`
- `TestApiAsync()` throws exception instead of returning `false`

## [13.4.0] - 2017-12-07

### Added

- `Stickers` test cases
- `StickerOwnerUserId` parameter to Systems Integration Tests settings
- Exception `ChatNotFoundException`
- Exception `ContactRequestException`
- Exception `InvalidUserIdException`
- Exception `UserNotFoundException`
- Exception `InvalidStickerSetNameException`
- Exception `InvalidStickerEmojisException`
- Exception `InvalidStickerDimensionsException`
- Exception `StickerSetNameExistsException`
- Exception `StickerSetNotModifiedException`

### Changed

- Include XML docs in NuGet package

## [13.4.0-rc2] - 2017-11-26

### Fixed

- Default snake-cased property name serialization

## [13.4.0-rc1] - 2017-11-26

### Added

- Support for Bot API 3.5
- Method `SendMediaGroupAsync`
- Types `InputFileBase` and `InputMediaBase`
- Test Collection `AlbumMessageTests`
- Test Collection `ChannelAdminBotTests`

## [13.3.0-rc1]

> ToDo

## [13.2.2-rc]

> ToDo

## [13.2.1] - 2017-09-04

### Added

- Systems integrations test project

### Changed

- Sandcastle project moved to `docs` branch
- TravisCI configurations to run systems integrations tests

### Fixed

- Quoting the string containing channel ids
- Double escaping the escape character in the caption of messages
- Serialization errors of `PhotoSize`
- Sending wrong filenames containing non-ASCII characters
- Deserialization of `PhotoSize`

## [13.1.0] - 2017-07-23

### Added

- Method `KickChatMemberAsync` parameter `untilDate`
- Method `RestrictChatMemberAsync`
- Method `PromoteChatMemberAsync`
- Method `ExportChatInviteLinkAsync`
- Method `SetChatPhotoAsync`
- Method `DeleteChatPhotoAsync`
- Method `SetChatTitleAsync`
- Method `SetChatDescriptionAsync`
- Method `PinChatMessageAsync`
- Method `UnpinChatMessageAsync`
- Method `SendVideoNoteAsync` parameter `length`
- Method `GetStickerSetAsync`
- Method `UploadStickerFileAsync`
- Method `CreateNewStickerSetAsync`
- Method `AddStickerToSetAsync`
- Method `SetStickerPositionInSetAsync`
- Method `DeleteStickerFromSetAsync`
- Type `VideoNote` property `Length`
- Type `Chat`properties `Photo`, `Description`, `InviteLink`
- Type `ChatMember` properties `UntilDate`, `Can*`
- Type `Sticker` properties `SetName` and `MaskPosition`
- Type `MaskPosition`
- Type `StickerSet`
- Type `ChatPhoto`
- Types `InlineKeyboard*Button`
- Enum `MaskPositionPoint`

### Changed

- User and Chat Ids reverted to base types
- DateTimes are now in local time zone
- Splitedd Keyboardbuttons in `InlineKeyboardCallbackButton`, `InlineKeyboardCallbackGameButton`, `InlineKeyboardPayButton`, `InlineKeyboardSwitchCallbackQueryCurrentButton`, `InlineKeyboardSwitchInlineQueryButton` and `InlineKeyboardUrlButton`

### Fixed

- Inline messge editing
- InlineQueryResult* `ThumbHight` and `ThumbWidth`
- Method `SetWebHookAsync` parameter `max_connections`
- Method `SetGameStoreAsync`
- Type `CallbackQuery` Property `Data` optimal
- Type `Message` can now be a `VideoNoteMessage`

## [12.0.0] - Beta only

### Added

- Method `DeleteMessageAsync`
- Method `SendVideoNoteAsync`
- Method `SendInvoiceAsync`
- Method `AnswerShippingQueryAsync`
- Method `AnswerPreCheckoutQueryAsync`
- Type `Invoice`
- Type `LabeledPrice`
- Type `ShippingAddress`
- Type `ShippingOption`
- Type `ShippingQuery`
- Type `SuccessfulPayment`
- Type `OrderInfo`
- Type `PreCheckoutQuery`
- Type `VideoNote`
- Type `Message` properties `VideoNote`, `Invoice` and `SuccessfulPayment`
- Type `User` poperty `LanguageCode`
- Type `Update` properties `ShippingQuery` and `PreCheckoutQuery`
- Type `InlineQueryResultGif` porperty `Duration`
- Type `InlineQueryResultMpeg4Gif` porperty `Duration`
- Type `InlineeyboardButton` property `Pay`
- Enum `ChatAction` members `RecordVideoNote` and `UplaodVideoNote`
- Enum `UpdateType` members `ShippingQuery` and `PreCheckoutQuery`

### Changed

- Property `NewChatMember` replaced with `NewChatMembers` on Type `Message`

## [Past]

### Added

- Method `SetWebHookAsync` parameters `maxConnections`, `allowedUpdates`
- Method `AnswerCallbackQueryAsync` parameter `cacheTime`
- Method `StartReceiving` parameter `allowedUpdates`
- Method `DeleteWebhookAsync`
- Method `GetWebhookInfoAsync`
- Type `ApiExceptions` property `Parameters`
- Type `InlineKeyboardButton` property `SwitchInlineQueryCurrentChat`
- Type `ResponseParameters`
- Type `WebhookInfo`
- Type `ChatId`
- Type `Chat` property `AllMembersAreAdministrators`
- Type `Message`property `ForwardFromMessageId`
- Type `Update` property `ChannelPost`, `EditedChannelPost`
- Event `OnReceiveGeneralError`
- Enum `UpdateType` member `ChannelPost`, `EditedChannelPost`
- Enum `FileType`
- Game Support
  - Method `SetGameScoreAsync`
  - Method `SendGameAsync`
  - Method `GetGameHighScoresAsync`
  - Method `AnswerCallbackQueryAsync` parameter `url`
  - Type `Animation`
  - Type `CallBackGame`
  - Type `CallBackquery` properties `ChatInstance`, `GameShortName`
  - Type `GameHighScore`
  - Type `InlineKeyboardButton` propertiy `CallbackGame`
  - Type `InlineQueryResults`
  - Type `Message` property `Game`
  - Enum `InlineQueryResultType` member `Game`
  - Enum `MesageType` member `Game`

### Changed

- Now the `HttpClient` will be reused
- Consolidated timeouts
- To use a proxy, use the constructor
- Unified the `chatId` parameters
- Replaced `ReplyKeyboardHide` with `ReplyKeyboardRemove`
- Replaced all file sending overloads with `FileToSend`

### Removed

- Removed deprecated API class
- Removed deprecated methods and events
- Removed `StartReceiving` overload with `timeout` parameter
