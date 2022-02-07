using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TayBot
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {

        [Command("poke")]
        [Summary("Check to see if taybot is awake")]
        public async Task Poke()
        {
            await ReplyAsync("Hey.");
        }

        [Command("setprefix")]
        [Summary("Sets the command prefix used for TayBot")]
        public async Task SetPrefix(
            char prefix
            )
        {
            char currentPrefix = CommandHandler._CharPrefix;
            CommandHandler._CharPrefix = prefix;
            await ReplyAsync("Changed command prefix from " + currentPrefix + " to " + prefix);
        }

        [Command("help")]
        [Summary("Shows the commands for TayBot")]
        public async Task Help()
        {
            await ReplyAsync(embed: GetAboutEmbed());
        }

        [Command("wordle")]
        [Summary("Play wordle")]
        public async Task GuessWordle(
            [Summary("Guess the 5 letter long wordle answer")]
            string guess)
        {
            guess = guess.ToLower();

            ulong userId = Context.User.Id;
            string username = Context.User.Username;

            WordleUser user;
            if (Wordle._UserDict.ContainsKey(userId))
            {
                user = Wordle._UserDict[userId];
            }
            else
            {
                user = new WordleUser(userId, username);
                Wordle._UserDict[userId] = user;
            }

            if (user.IsGameOver())
            {
                // Check to see if the word used has changed or not.
                int wordIndex = Wordle.CalculateWordleIndex();
                if (wordIndex != user.GetWordIndex())
                {
                    // Reset the game if the word has changed
                    user.ResetNewGame();
                }
                else
                {
                    // Print the current guesses if the game is over and the word hasn't changed.
                    await ReplyAsync(embed: user.GetDiscordWordleEmbed(Context.IsPrivate));
                    return;
                }
            }

            if (!Context.IsPrivate)
            {
                await ReplyAsync(embed: user.GetDiscordWordleEmbed(Context.IsPrivate));
                return;
            }

            if (guess.Length <= 0)
            {
                await ReplyAsync(embed: user.GetDiscordWordleEmbed(Context.IsPrivate));
                return;
            }

            if (!Wordle.IsValidGuess(guess))
            {
                await ReplyAsync("Invalid guess, please only use letters, max size of 5.\n\n");
                return;
            }

            if (!Wordle.IsGuessAcceptableWord(guess))
            {
                await ReplyAsync("The word " + guess + " is not an accepted word.");
                return;
            }

            string result = Wordle.HandleGuess(guess);
            user.AddGuessResults(result, guess);
            bool won = Wordle.IsWinningGuess(guess);
            if (won)
            {
                user.UpdateGameOver(true);
            }
            else if (user.GuessesLeft() < 0)
            {
                user.UpdateGameOver(false);
            }
            user.WriteToSave();

            await ReplyAsync(embed: user.GetDiscordWordleEmbed(Context.IsPrivate));
        }

        public Embed GetAboutEmbed()
        {
            EmbedBuilder builder = new EmbedBuilder
            {
                Title = "ABOUT TAYBOT",
                Description = "Commands Tay currently supports: "
            };
            builder.AddField("wordle [5 letter guess]", "Guess the current word of the day. Please note that you can only guess in DMs to avoid spoilers in public channels.");
            builder.AddField("mywordle", "TayBot will show your current wordle stats and guesses, without spoilers! Can use in public channels.");
            builder.AddField("setprefix [prefix character]", "Sets TayBot's prefix for commands.");
            builder.AddField("help", "displays this... well, how else did you see this?");
            return builder.Build();
        }
    }
}
