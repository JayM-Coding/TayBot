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
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GuessesLeft { get; set; }
        public List<string> GuessResults { get; set; }
        public List<string> GuessStrings { get; set; }
    }
}
