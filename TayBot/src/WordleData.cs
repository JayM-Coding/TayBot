using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TayBot
{
    class WordleData
    {
        public ulong DiscordUserId { get; set; }
        public bool IsCurrentGameOver { get; set; }
        public bool IsCurrentGameWon { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GuessesLeft { get; set; }
        public int CurrentWordIndex { get; set; }
        public string Username { get; set; }
        public List<string> GuessResults { get; set; }
        public List<string> GuessStrings { get; set; }
        public List<string> CorrectLetters { get; set; }
        public List<string> IncorrectLetters { get; set; }
        public List<string> MaybeLetters { get; set; }
        public List<string> PotentialLetters { get; set; }
    }
}
