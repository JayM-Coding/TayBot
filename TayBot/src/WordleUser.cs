using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TayBot
{
    class WordleUser
    {
        private static string USER_FILE_PATH = "C:\\Users\\yello\\Downloads\\Documents\\Taybot\\";
        WordleData _user;
        private string _filePath = string.Empty;


        private static int MAX_GUESSES = 6;

        public WordleUser(ulong userId, string username)
        {
            _filePath = USER_FILE_PATH + username + userId + ".json";
            if (File.Exists(_filePath))
            {
                _user = ReadFromSave(_filePath);
            }
            else
            {
               CreateNewSave(userId, username);
            }
        }

        public void AddGuessResults(string guessResults, string guessStrings)
        {
            _user.GuessResults.Add(guessResults);
            _user.GuessStrings.Add(guessStrings);
            _user.GuessesLeft--;
        }

        public void UpdateGameOver(bool won)
        {
            _user.IsCurrentGameOver = true;
            _user.IsCurrentGameWon = won;
            _user.GamesPlayed += 1;
            if (won) { _user.GamesWon += 1; }
        }

        public void ResetNewGame()
        {
            _user.IsCurrentGameOver = false;
            _user.IsCurrentGameWon = false;
            _user.GuessResults = new List<string>();
            _user.GuessStrings = new List<string>();
            _user.GuessesLeft = MAX_GUESSES;
            _user.CurrentWordIndex = Wordle.CalculateWordleIndex();
        }

        public int GuessesLeft()
        {
            return _user.GuessesLeft;
        }

        public bool IsGameOver()
        {
            return _user.IsCurrentGameOver;
        }

        public int GetWordIndex()
        {
            return _user.CurrentWordIndex;
        }

        public string GetGameBoardString(bool showSpoilers)
        {
            string board = string.Empty;
            int guessCount = _user.GuessResults.Count;
            for (int i = 0; i < guessCount; i++)
            {
                if (showSpoilers)
                {
                    board += Wordle.MakeStringSpacedOut(_user.GuessStrings[i]) + "  \n";
                }   
                board += _user.GuessResults[i] + "\n";
            }
            for (int i = 0; i < (MAX_GUESSES) - guessCount; i++)
            {
                if (showSpoilers)
                {
                    board += Wordle.MakeStringSpacedOut("-----") + "  \n";
                }
               
                for (int j = 0; j < 5; j++)
                {
                    board += Wordle.GetEmojiFromResult(LetterResult.E) + " ";
                }
                board += "\n";
            }

            return board;
        }

        public Embed GetDiscordWordleEmbed(bool showSpoilers)
        {
            EmbedBuilder builder = new EmbedBuilder
            {
                Title = "WORDLE #" + Wordle.CalculateWordleIndex(),
                Description = "Wordle stats for " + _user.Username + ":"
            };
            
            if (_user.IsCurrentGameOver)
            {
                builder.AddField("GUESSES: ", GetGameBoardString(showSpoilers), false);
                if (_user.IsCurrentGameWon)
                {
                    builder.AddField("YOU'VE WON", GetWinStatsString(), false);
                }
                else
                {
                    builder.AddField("YOU'VE LOST", GetWinStatsString(), false);
                }
            }
            else
            {
                builder.AddField("GUESSES (" + (MAX_GUESSES - _user.GuessesLeft) + "/" + MAX_GUESSES + "):", GetGameBoardString(showSpoilers), false);
            }

            return builder.Build();
        }

        public void WriteToSave()
        {
            string json = JsonConvert.SerializeObject(_user);

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                sw.Write(json);
            }
        }

        private string GetWinStatsString()
        {
            string results = "Games won: " + _user.GamesWon + "\n";
            results += "Games played: " + _user.GamesPlayed + "\n";
            results += "Win percentage: " + Math.Round(((float)_user.GamesWon / (float)_user.GamesPlayed), 2) * 100f + "%\n";
            return results;
        }

        private WordleData ReadFromSave(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string json = sr.ReadToEnd();

                return JsonConvert.DeserializeObject<WordleData>(json);
            }
        }

        private void CreateNewSave(ulong userId, string username)
        {
            _user = new WordleData();
            _user.DiscordUserId = userId;
            _user.IsCurrentGameOver = false;
            _user.IsCurrentGameWon = false;
            _user.GamesPlayed = 0;
            _user.GamesWon = 0;
            _user.CurrentWordIndex = Wordle.CalculateWordleIndex();
            _user.GuessesLeft = MAX_GUESSES;
            _user.GuessResults = new List<string>();
            _user.GuessStrings = new List<string>();
            _user.CorrectLetters = new List<string>();
            _user.IncorrectLetters = new List<string>();
            _user.PotentialLetters = new List<string>();
            _user.MaybeLetters = new List<string>();
            _user.Username = username;
            WriteToSave();
            return;
        }
    }
}
