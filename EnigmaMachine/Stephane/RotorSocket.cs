namespace EnigmaMachine.Stephane
{
    public sealed class RotorSocket
    {
        private readonly char _initialLetter;
        private readonly Rotor _rotor;

        public RotorSocket(Rotor rotor, char startingLetter = 'A')
            : this(rotor, startingLetter, startingLetter)
        {
        }

        private RotorSocket(Rotor rotor, char currentLetter, char startingLetter)
        {
            _rotor = rotor;
            _initialLetter = startingLetter;
            CurrentRingLetter = currentLetter;
            IsSocketInNotchPosition = _rotor.IsNotch(currentLetter);
        }

        public char CurrentRingLetter { get; }

        public RotorSocket SetupRotor(Rotor rotor, char startingLetter = 'A')
        {
            return new RotorSocket(rotor, startingLetter);
        }

        public RotorSocket SetupRotor(RotorInfo rotorInfo)
        {
            return SetupRotor(Rotor.Create(rotorInfo.Type, rotorInfo.RingSettingOffset), rotorInfo.StartingOffset);
        }

        public RotorSocket SetRingLetter(char letter)
        {
            return new RotorSocket(_rotor, letter, _initialLetter);
        }

        public RotorSocket SetStartingRingLetter(char letter)
        {
            return new RotorSocket(_rotor, letter);
        }

        public RotorSocket Advance()
        {
            return SetRingLetter(IncrementLetter(CurrentRingLetter));
        }

        private char IncrementLetter(char letter)
        {
            return letter.AddOffset(1);
        }

        public RotorSocket Reset()
        {
            return SetRingLetter(_initialLetter);
        }

        public char GetMappedLetter(char letter, LetterMapper.MappingDirection direction = LetterMapper.MappingDirection.RightToLeft)
        {
            var entryLetter = GetEntryLetter(letter);
            char mappedLetter = _rotor.GetMappedLetter(entryLetter, direction);

            return GetOutputLetter(mappedLetter);
        }

        private char GetEntryLetter(char letter)
        {
            return letter.AddOffset(CurrentRingLetter);
        }

        private char GetOutputLetter(char letter)
        {
            return letter.RemoveOffset(CurrentRingLetter);
        }

        public bool IsSocketInNotchPosition { get; }
    }
}
