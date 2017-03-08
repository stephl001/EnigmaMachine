namespace EnigmaMachine.Stephane
{
    public class LetterMapper
    {
        public enum MappingDirection
        {
            RightToLeft,
            LeftToRight
        };
        
        public LetterMapper(string mapping)
        {
            Mapping = mapping;
        }

        protected string Mapping { get; }

        public virtual char GetMappedLetter(char letter, MappingDirection dir = MappingDirection.RightToLeft)
        {
            if (dir == MappingDirection.RightToLeft)
                return Mapping[letter - 'A'];

            return (char) (Mapping.IndexOf(letter) + 'A');
        }
    }
}
