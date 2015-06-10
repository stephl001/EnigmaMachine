using System;
using System.Collections.Generic;
using ReflectorClass = EnigmaMachine.Stephane.Reflector;

namespace EnigmaMachine.Stephane
{
    public class EnigmaMachine
    {
        public EnigmaMachine()
        {
            Plugboard = new Plugboard();
            FastRotor = new RotorSocket(Rotor.Create("I"));
            MiddleRotor = new RotorSocket(Rotor.Create("II"));
            SlowRotor = new RotorSocket(Rotor.Create("III"));
            Reflector = new RotorSocket(ReflectorClass.Create("Reflector B"));
        }

        public Plugboard Plugboard { get; set; }

        public RotorSocket FastRotor { get; private set; }
        public RotorSocket MiddleRotor { get; private set; }
        public RotorSocket SlowRotor { get; private set; }

        public RotorSocket Reflector { get; private set; }

        public char PressKey(char key)
        {
            MoveRotors();
            char cypherLetter = GetCypherLetter(key);
            return cypherLetter;
        }

        private char GetCypherLetter(char key)
        {
            char mappedLetter = Plugboard.GetMappedLetter(key);
            mappedLetter = FastRotor.GetMappedLetter(mappedLetter);
            mappedLetter = MiddleRotor.GetMappedLetter(mappedLetter);
            mappedLetter = SlowRotor.GetMappedLetter(mappedLetter);
            mappedLetter = Reflector.GetMappedLetter(mappedLetter);
            mappedLetter = SlowRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = MiddleRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = FastRotor.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);
            mappedLetter = Plugboard.GetMappedLetter(mappedLetter, LetterMapper.MappingDirection.LeftToRight);

            return mappedLetter;
        }

        private void MoveRotors()
        {
            bool slowRotorIncremented = false;
            if (MiddleRotor.IsSocketInNotchPosition)
            {
                slowRotorIncremented = true;
                SlowRotor.Advance();
            }
            if (FastRotor.IsSocketInNotchPosition || slowRotorIncremented)
                MiddleRotor.Advance();
            
            FastRotor.Advance();
        }

        public void Reset()
        {
            FastRotor.Reset();
            MiddleRotor.Reset();
            SlowRotor.Reset();
        }
    }
}
