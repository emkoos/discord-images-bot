using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordMemeBot
{
    class Program
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "{bot_token_here}",
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = false,
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                var DailyTime = "15:20:00";
                var timeParts = DailyTime.Split(new char[1] { ':' });

                if (e.Message.Content.StartsWith("!botStart"))
                {
                    while (true)
                    {
                        var dateNow = DateTime.Now;
                        var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                                   int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
                        TimeSpan ts;
                        if (date > dateNow)
                            ts = date - dateNow;
                        else
                        {
                            date = date.AddDays(1);
                            ts = date - dateNow;
                        }

                        var random = new Random();
                        var list = new List<string> { "meme1", "meme2", "meme3", "meme4", "meme5", "meme6", "meme7", "meme8", "meme9", "meme10" };
                        int index = random.Next(list.Count);

                        await Task.Delay(ts);

                        await e.Message.RespondWithFileAsync("../Images/" + list[index] + ".jpg", "Its your meme for today!");
                    }
                }
            };

            discord.DebugLogger.LogMessageReceived += DebugLogger_LogMessageReceived;

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static private void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
            => Console.WriteLine($"[{e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Application}] [{e.Level}] {e.Message}");


    }
}
