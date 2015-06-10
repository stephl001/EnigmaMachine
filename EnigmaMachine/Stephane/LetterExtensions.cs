using System;

namespace EnigmaMachine.Stephane
{
    public static class LetterExtensions
    {
        private const int AlphabetLength = 26;

        public static char AddOffset(this char letter, int offset)
        {
            if (!char.IsLetter(letter))
                throw new ArgumentOutOfRangeException("letter", "The input character must be a letter.");
            if (!char.IsUpper(letter))
                throw new ArgumentException("Letter must be uppercase.", "letter");

            return (char) (((letter - 'A' + offset + AlphabetLength)%26) + 'A');
        }

        public static char AddOffset(this char letter, char letterOffset)
        {
            if (!char.IsLetter(letter))
                throw new ArgumentOutOfRangeException("letter", "The input character must be a letter.");
            if (!char.IsUpper(letter))
                throw new ArgumentException("Letter must be uppercase.", "letter");
            if (!char.IsLetter(letterOffset))
                throw new ArgumentOutOfRangeException("letterOffset", "The input character must be a letter.");
            if (!char.IsUpper(letterOffset))
                throw new ArgumentException("Letter must be uppercase.", "letterOffset");

            return AddOffset(letter, (letterOffset - 'A'));
        }

        public static char RemoveOffset(this char letter, char letterOffset)
        {
            if (!char.IsLetter(letter))
                throw new ArgumentOutOfRangeException("letter", "The input character must be a letter.");
            if (!char.IsUpper(letter))
                throw new ArgumentException("Letter must be uppercase.", "letter");
            if (!char.IsLetter(letterOffset))
                throw new ArgumentOutOfRangeException("letterOffset", "The input character must be a letter.");
            if (!char.IsUpper(letterOffset))
                throw new ArgumentException("Letter must be uppercase.", "letterOffset");

            return AddOffset(letter, -(letterOffset - 'A'));
        }
    }
}
