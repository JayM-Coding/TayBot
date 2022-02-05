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

        public WordleUser(ulong userId)
        {
            _filePath = USER_FILE_PATH + userId + ".json";
            if (File.Exists(_filePath))
            {
                _user = ReadFromSave(_filePath);
            }
            else
            {
               CreateNewSave(userId);
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
            _user.IsCurrentGameOver = won;
            _user.GamesPlayed += 1;
            if (won) { _user.GamesWon += 1; }
        }

        public void ResetNewGame()
        {
            _user.IsCurrentGameOver = false;
            _user.GuessResults = new List<string>();
            _user.GuessStrings = new List<string>();
            _user.GuessesLeft = MAX_GUESSES;
        }

        public int GuessesLeft()
        {
            return _user.GuessesLeft;
        }

        public bool IsGameOver()
        {
            return _user.IsCurrentGameOver;
        }

        public string GetGameBoardString()
        {
            string board = string.Empty;
            for (int i = 0; i < _user.GuessResults.Count; i++)
            {
                board += _user.GuessStrings[i] + "\n";
                board += _user.GuessResults[i] + "\n";
            }
            return board;
        }

        public void WriteToSave()
        {
            string json = JsonConvert.SerializeObject(_user);

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                sw.Write(json);
            }
        }

        private WordleData ReadFromSave(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string json = sr.ReadToEnd();

                return JsonConvert.DeserializeObject<WordleData>(json);
            }
        }

        private void CreateNewSave(ulong userId)
        {
            _user = new WordleData();
            _user.DiscordUserId = userId;
            _user.IsCurrentGameOver = false;
            _user.GamesPlayed = 0;
            _user.GamesWon = 0;
            _user.GuessesLeft = MAX_GUESSES;
            _user.GuessResults = new List<string>();
            _user.GuessStrings = new List<string>();
            WriteToSave();
            return;
        }
    }
}
