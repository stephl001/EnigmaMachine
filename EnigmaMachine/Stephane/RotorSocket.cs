namespace EnigmaMachine.Stephane
{
    public sealed class RotorSocket
    {
        private char _initialLetter;
        private Rotor _rotor;

        public RotorSocket(Rotor rotor, char startingLetter = 'A')
        {
            SetupRotor(rotor, startingLetter);
        }

        public char CurrentRingLetter { get; private set; }

        public void SetupRotor(Rotor rotor, char startingLetter = 'A')
        {
            SetRingLetter(startingLetter);
            _rotor = rotor;
        }

        public void SetupRotor(RotorInfo rotorInfo)
        {
            SetupRotor(Rotor.Create(rotorInfo.Type, rotorInfo.RingSettingOffset), rotorInfo.StartingOffset);
        }

        public void SetRingLetter(char letter)
        {
            CurrentRingLetter = _initialLetter = letter;
        }

        public void Advance()
        {
            CurrentRingLetter = IncrementLetter(CurrentRingLetter);
        }

        private char IncrementLetter(char letter)
        {
            return letter.AddOffset(1);
        }

        public void Reset()
        {
            CurrentRingLetter = _initialLetter;
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

        public bool IsSocketInNotchPosition
        {
            get { return _rotor.IsNotch(CurrentRingLetter); }
        }
    }
}
