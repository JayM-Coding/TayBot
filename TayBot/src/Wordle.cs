using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TayBot
{
    enum LetterResult
    {
        Y, // yes (green, 100% correct)
        N, // no (black, letter not in word)
        M, // maybe (yellow, letter in word, wrong position)
        E // empty (grey, used in empty guesses on the board)
    }

    class Wordle
    {
        public static Dictionary<ulong, WordleUser> _UserDict = new Dictionary<ulong, WordleUser>();

        private static string[] _answers = new string[]{ "testy", "angel", "other" };
        private static string _todaysAnswer = _answers[1];

        // return false if the guess was syntactically invalid
        public static string HandleGuess(string guess)
        {
            if (!IsValidGuess(guess)) return "Invalid guess, please only use letters, max size of 5";
            LetterResult[] letterResults = GetLetterResults(guess);
            string resultEmojis = GetResultEmojis(letterResults);

            return resultEmojis;
        }

        public static bool IsValidGuess(string guess)
        {
            if (guess.Length != 5) return false;
            if (!guess.All(Char.IsLetter)) return false;
            return true;
        }

        public static bool IsWinningGuess(string guess)
        {
            for (int i = 0; i < _todaysAnswer.Length; i++)
            {
                if (guess[i] != _todaysAnswer[i]) { return false; }
            }
            return true;
        }

        private static LetterResult[] GetLetterResults(string guess)
        {
            LetterResult[] results = new LetterResult[5];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = GetLetterResult(guess, i);
            }
            return results;
        }

        private static LetterResult GetLetterResult(string guess, int index)
        {
            if (guess[index] == _todaysAnswer[index]) return LetterResult.Y;

            // If the letter isn't 100% correct, 
            // and the letter was already guessed in any of the previous letters,
            // it HAS to be considered wrong.
            for (int i = 0; i < index; i++)
            {
                if (guess[index] == guess[i]) return LetterResult.N;
            }

            if (_todaysAnswer.Contains(guess[index])) return LetterResult.M;
            return LetterResult.N;
        }

        private static string GetResultEmojis(LetterResult[] letters)
        {
            string emojiString = "";
            for (int i = 0; i < letters.Length; i++)
            {
                emojiString += GetEmojiFromResult(letters[i]) + " ";
            }
            return emojiString;
        }

        private static string GetEmojiFromResult(LetterResult result)
        {
            switch (result)
            {
                case LetterResult.Y: return "🟩";
                case LetterResult.N: return "⬛";
                case LetterResult.M: return "🟨";
                case LetterResult.E: return "⬜";
                default: return "⬜";
            }
        }
    }
}
