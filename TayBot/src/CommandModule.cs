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

        [Command("guess")]
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
            string board = string.Empty;
            if (user.IsGameOver())
            {
                board = user.GetGameBoardString();
                await ReplyAsync("Current game has already finished:\n" + board);
            }

            if (!Wordle.IsValidGuess(guess))
            {
                board = user.GetGameBoardString();
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
            board = user.GetGameBoardString();
            user.WriteToSave();

            await ReplyAsync("Your guess result:\n" + board);
        }
    }
}
