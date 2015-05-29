namespace EnigmaMachine.Stephane
{
    public class LetterMapper
    {
        private const int AlphabetLength = 26;

        public enum MappingDirection
        {
            RightToLeft,
            LeftToRight
        };

        private readonly string _mapping;
        private readonly int _offset;

        public LetterMapper(string mapping, char offset = 'A')
        {
            _mapping = mapping;
            _offset = offset - 'A';
        }

        public char GetMappedLetter(char letter, MappingDirection dir = MappingDirection.RightToLeft)
        {
            if (dir == MappingDirection.RightToLeft)
                return _mapping[(letter - 'A' + _offset) % AlphabetLength];

            return (char)('A' + ((_mapping.IndexOf(letter) - _offset + AlphabetLength) % AlphabetLength));
        }
    }
}
