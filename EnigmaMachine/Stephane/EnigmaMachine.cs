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

        private readonly RotorSocket _fastRotor;
        private readonly RotorSocket _middleRotor;
        private readonly RotorSocket _slowRotor;

        private readonly RotorSocket _reflector;

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
                _slowRotor.Advance();
            }
            if (_fastRotor.IsSocketInNotchPosition || slowRotorIncremented)
                _middleRotor.Advance();
            
            _fastRotor.Advance();
        }


        public void SetupPlugboard(string mappings)
        {
            _plugboard = new Plugboard(mappings);
        }

        public void SetupRotors(RotorInfo[] rotorInfos)
        {
            _slowRotor.SetupRotor(rotorInfos[0]);
            _middleRotor.SetupRotor(rotorInfos[1]);
            _fastRotor.SetupRotor(rotorInfos[2]);
        }

        public void SetupReflector(string type)
        {
            _reflector.SetupRotor(ReflectorClass.Create(type));
        }

        public char[] GetCurrentRotorRingLetters()
        {
            return new[] { _slowRotor.CurrentRingLetter, _middleRotor.CurrentRingLetter, _fastRotor.CurrentRingLetter };
        }

        public void SetCurrentRotorRingLetters(char[] letters)
        {
            _slowRotor.SetRingLetter(letters[0]);
            _middleRotor.SetRingLetter(letters[1]);
            _fastRotor.SetRingLetter(letters[2]);
        }

        public void ResetRotors()
        {
            _fastRotor.Reset();
            _middleRotor.Reset();
            _slowRotor.Reset();
        }
    }
}
