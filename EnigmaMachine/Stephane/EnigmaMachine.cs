using ReflectorClass = EnigmaMachine.Stephane.Reflector;

namespace EnigmaMachine.Stephane
{
    public class EnigmaMachine : IEnigmaMachine
    {
        public EnigmaMachine()
        {
            _plugboard = new Plugboard();
            _fastRotor = new RotorSocket(Rotor.Create("I"));
            _middleRotor = new RotorSocket(Rotor.Create("II"));
            _slowRotor = new RotorSocket(Rotor.Create("III"));
            _reflector = new RotorSocket(ReflectorClass.Create("Reflector B"));
        }

        private Plugboard _plugboard;

        private RotorSocket _fastRotor;
        private RotorSocket _middleRotor;
        private RotorSocket _slowRotor;

        private RotorSocket _reflector;

        public char PressKey(char key)
        {
            MoveRotors();
            char cypherLetter = GetCypherLetter(key);
            return cypherLetter;
        }

        private char GetCypherLetter(char key)
        {
            char mappedLetter = _plugboard.GetMappedLetter(key);
            mappedLetter = _fastRotor.GetMappedLetter(mappedLetter);
            mappedLetter = _middleRotor.GetMappedLetter(mappedLetter);
            mappedLetter = _slowRotor.GetMappedLetter(mappedLetter);
            mappedLetter = _reflector.GetMappedLetter(mappedLetter);
            mappedLetter = _slowRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = _middleRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = _fastRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = _plugboard.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);

            return mappedLetter;
        }

        private void MoveRotors()
        {
            bool slowRotorIncremented = false;
            if (_middleRotor.IsSocketInNotchPosition)
            {
                slowRotorIncremented = true;
                _slowRotor = _slowRotor.Advance();
            }
            if (_fastRotor.IsSocketInNotchPosition || slowRotorIncremented)
                _middleRotor = _middleRotor.Advance();

            _fastRotor = _fastRotor.Advance();
        }


        public void SetupPlugboard(string mappings)
        {
            _plugboard = new Plugboard(mappings);
        }

        public void SetupRotors(RotorInfo[] rotorInfos)
        {
            _slowRotor = _slowRotor.SetupRotor(rotorInfos[0]);
            _middleRotor = _middleRotor.SetupRotor(rotorInfos[1]);
            _fastRotor = _fastRotor.SetupRotor(rotorInfos[2]);
        }

        public void SetupReflector(string type)
        {
            _reflector = _reflector.SetupRotor(ReflectorClass.Create(type));
        }

        public char[] GetCurrentRotorRingLetters()
        {
            return new[] { _slowRotor.CurrentRingLetter, _middleRotor.CurrentRingLetter, _fastRotor.CurrentRingLetter };
        }

        public void SetStartupRotorRingLetters(char[] letters)
        {
            _slowRotor = _slowRotor.SetStartingRingLetter(letters[0]);
            _middleRotor = _middleRotor.SetStartingRingLetter(letters[1]);
            _fastRotor = _fastRotor.SetStartingRingLetter(letters[2]);
        }

        public void ResetRotors()
        {
            _fastRotor = _fastRotor.Reset();
            _middleRotor = _middleRotor.Reset();
            _slowRotor = _slowRotor.Reset();
        }
    }
}
