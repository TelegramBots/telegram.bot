# .NET Client for Telegram Bot API

[![package](https://img.shields.io/nuget/vpre/Telegram.Bot.svg?label=Telegram.Bot&style=flat-square)](https://www.nuget.org/packages/Telegram.Bot)
[![Bot API Version](https://img.shields.io/badge/Bot%20API-5.2%20(June%2025,%202021)-f36caf.svg?style=flat-square)](https://core.telegram.org/bots/api)
[![documentations](https://img.shields.io/badge/Documentations-Book-orange.svg?style=flat-square)](https://telegrambots.github.io/book/)
[![telegram chat](https://img.shields.io/badge/Support_Chat-Telegram-blue.svg?style=flat-square)](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA)

[![build](https://img.shields.io/azure-devops/build/tgbots/14f9ab3f-313a-4339-8534-e8b96c7763cc/6?style=flat-square&label=master)](https://dev.azure.com/tgbots/Telegram.Bot/_build/latest?definitionId=6&branchName=master)
[![build](https://img.shields.io/azure-devops/build/tgbots/14f9ab3f-313a-4339-8534-e8b96c7763cc/10/develop?style=flat-square&label=develop)](https://dev.azure.com/tgbots/Telegram.Bot/_build/latest?definitionId=10&branchName=develop)
[![downloads](https://img.shields.io/nuget/dt/Telegram.Bot.svg?style=flat-square&label=Package%20Downloads)](https://www.nuget.org/packages/Telegram.Bot)
[![contributors](https://img.shields.io/github/contributors/TelegramBots/Telegram.Bot.svg?style=flat-square&label=Contributors)](https://github.com/TelegramBots/Telegram.Bot/graphs/contributors)
[![license](https://img.shields.io/github/license/TelegramBots/telegram.bot.svg?style=flat-square&maxAge=2592000&label=License)](https://raw.githubusercontent.com/TelegramBots/telegram.bot/master/LICENSE)

**Telegram.Bot** is the most popular .NET Client for 🤖 [Telegram Bot API].

The Bot API is an HTTP-based interface created for developers keen on building bots for [Telegram].

Check 👉 [_Bots: An introduction for developers_] 👈 to understand what a Telegram bot is and what it can do.

We, the [Telegram Bots team], mainly focus on developing multiple [NuGet packages] for creating chatbots.

|Package|Documentation|News Channel|Team|Group Chat|
|:-----:|:-----------:|:----------:|:--:|:--------:|
| [![package](docs/logo-nuget.png)](https://www.nuget.org/packages/Telegram.Bot) | [![documentations](docs/logo-docs.png)](https://telegrambots.github.io/book/) | [![News Channel](docs/logo-channel.jpg)](https://t.me/tgbots_dotnet) | [![Team](docs/logo-gh.png)](https://github.com/orgs/TelegramBots/people) | [![Group Chat](docs/logo-chat.jpg)](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA) |
| This package on NuGet | Telegram bots book | Subscribe to 📣 [`@tgbots_dotnet`] channel to get our latest news | The team contributing to this work | [Join our chat] 💬 to talk about bots and ask questions |

## 🔨 Getting Started

Please check ➡️ [the Quickstart guide].

## 🚧 Supported Platforms

Project targets **.NET Standard 2.0** and **.NET Core 3.1** at minimum.

## 📦 Extension Packages

This project is referred to as the core package and is always required for developing Telegram bots.
There are extension packages that you can include in your bot projects for additional functionality:

- [Telegram.Bot-Tools.TelegramCommandHandler]
- [Telegram.Bot-Tools.TelegramInteractivityHelper]
- [Telegram.Bot.Extensions.LoginWidget]
- [Telegram.Bot.Extensions.Passport]

## ✅ Correctness & Testing

This project is fully tested using Unit tests and Systems Integration tests before each release.
In fact, our test cases are self-documenting and serve as examples for Bot API methods.
Once you learn the basics of Telegram chatbots, you will be able to easily understand the code in examples and
use it in your own bot projects.

## 🗂 References

- [Changelog](CHANGELOG.md)
- [Documentation](https://telegrambots.github.io/book/)
- [Examples](https://github.com/TelegramBots/telegram.bot.examples)

<!-- ---- -->

[Telegram Bot API]: https://core.telegram.org/bots/api
[Telegram]: https://www.telegram.org/
[_Bots: An introduction for developers_]: https://core.telegram.org/bots
[Telegram Bots team]: https://github.com/orgs/TelegramBots/people
[NuGet packages]: https://www.nuget.org/profiles/TelegramBots
[`@tgbots_dotnet`]: https://t.me/tgbots_dotnet
[Join our chat]: https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA
[the Quickstart guide]: https://telegrambots.github.io/book/1/quickstart.html
[Telegram.Bot.Extensions.LoginWidget]: https://github.com/TelegramBots/Telegram.Bot.Extensions.LoginWidget
[Telegram.Bot.Extensions.Passport]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport
[Telegram.Bot-Tools.TelegramCommandHandler]: https://github.com/AliBdeir/Telegram.Bot-Tools/tree/master/TelegramCommandHandler
[Telegram.Bot-Tools.TelegramInteractivityHelper]: https://github.com/AliBdeir/Telegram.Bot-Tools/tree/master/TelegramInteractivityHelper
