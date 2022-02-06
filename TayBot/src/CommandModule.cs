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

        [Command("kys")]
        [Summary("Check to see if taybot is awake")]
        public async Task Kys()
        {
            await ReplyAsync("noo don't say that.");
        }

        [Command("wordle")]
        [Summary("Play wordle")]
        public async Task GuessWordle(
            [Summary("Guess the 5 letter long wordle answer")]
            string guess)
        {
            guess = guess.ToLower();

            ulong userId = Context.User.Id;

            WordleUser user;
            if (Wordle._UserDict.ContainsKey(userId))
            {
                user = Wordle._UserDict[userId];
            }
            else
            {
                user = new WordleUser(userId);
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
                    await ReplyAsync(embed: user.GetDiscordWordleEmbed(true));
                    return;
                }
            }

            if (!Context.IsPrivate)
            {
                await ReplyAsync(embed: user.GetDiscordWordleEmbed(false));
                return;
            }

            if (guess.Length <= 0)
            {
                await ReplyAsync(embed: user.GetDiscordWordleEmbed(true));
                return;
            }


            if (!Wordle.IsValidGuess(guess))
            {
                await ReplyAsync("Invalid guess, please only use letters, max size of 5.\n\n");
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

            await ReplyAsync(embed: user.GetDiscordWordleEmbed(true));
        }
    }
}
