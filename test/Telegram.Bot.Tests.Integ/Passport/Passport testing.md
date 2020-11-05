1) Make sure that appsetings.Development.json exist and filled
2) Use https://core.telegram.org/passport/example to be sure that your TG client can handle passport at all
3) Use the /setpublickey command with @BotFather to connect public key (located in tests) with your test bot.
4) READ text and click on inline button and continue. (some test cases require specific number of photos)
5) Some tests invalidate your photos so be ready to re-upload some of it. (you would not be able to share data with bot). Hopefully you can do it in client just before sharing data with bot.
6) There is some checks on image size, so don`t use tiny images.
7) You can upload same photo everywhere and multiple times if you want.
