using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TayBot
{
    public class TayBot
    {
        private DiscordSocketClient _discordClient;

        private string _tokenFilePath = "C:\\Users\\yello\\Desktop\\TayBotToken.txt";
        public static void Main(string[] args) => new TayBot().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            _discordClient = new DiscordSocketClient();
            _discordClient.Log += Log;
            var token = GetBotToken();

            await _discordClient.LoginAsync(Discord.TokenType.Bot, token);
            await _discordClient.StartAsync();

            _discordClient.Ready += OnClientReady;

            await Task.Delay(-1);
        }

        private Task Log(Discord.LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task OnClientReady()
        {
            CommandService commandService = new CommandService();
            CommandHandler commandHandler = new CommandHandler(_discordClient, commandService);
            await commandHandler.InstallCommandsAsync();
            Wordle.SetWordleAnswer(Wordle.CalculateWordleIndex());
        }

        private string GetBotToken()
        {
            string token = string.Empty;
            using (StreamReader reader = new StreamReader(File.Open(_tokenFilePath, FileMode.Open, FileAccess.Read)))
            {
                token = reader.ReadLine();
            }
            return token;
        }
    }
}
