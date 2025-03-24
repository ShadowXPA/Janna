using Janna.Utils;

namespace Janna;

class Program
{
    static async Task Main(string[] args)
    {
        string discordBotToken = Constants.GetEnvironmentVariableOrThrow(Constants.DISCORD_BOT_TOKEN);

        var bot = new DiscordBot(discordBotToken);

        await bot.ConnectAsync();
        await Task.Delay(-1);
    }
}
