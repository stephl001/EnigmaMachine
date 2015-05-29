using System.Collections.Generic;
using System.Linq;

namespace EnigmaMachine.Stephane
{
    public sealed class RotorDefinition
    {
        internal RotorDefinition(string mappings, char[] notches)
        {
            Mappings = mappings;
            Notches = notches;
        }

        public string Mappings { get; private set; }
        public char[] Notches { get; private set; }
    }

    public sealed class Rotor
    {
        public enum MappingDirection
        {
            RightToLeft,
            LeftToRight
        };

        private const int AlphabetLength = 26;

        private static readonly IDictionary<string, RotorDefinition> RotorDefinitions = new Dictionary<string, RotorDefinition>
        {
            {"I", new RotorDefinition("EKMFLGDQVZNTOWYHXUSPAIBRCJ", new [] {'Q'})},
            {"II", new RotorDefinition("AJDKSIRUXBLHWTMCQGZNPYFVOE", new [] {'E'})},
            {"III", new RotorDefinition("BDFHJLCPRTXVZNYEIWGAKMUSQO", new [] {'V'})},
            {"IV", new RotorDefinition("ESOVPZJAYQUIRHXLNFTGKDCMWB", new [] {'J'})},
            {"V", new RotorDefinition("VZBRGITYUPSDNHLXAWMJQOFECK", new [] {'Z'})},
            {"VI", new RotorDefinition("JPGVOUMFYQBENHZRDKASXLICTW", new [] {'M','Z'})},
            {"VII", new RotorDefinition("NZJHGRCXMYSWBOUFAIVLPEKQDT", new [] {'M','Z'})},
            {"VIII", new RotorDefinition("FKQHTLXOCBJSPDZRAMEWNIUYGV", new [] {'M','Z'})},
            {"ETW", new RotorDefinition("ABCDEFGHIJKLMNOPQRSTUVWXYZ", new char[] {})}
        };

        private readonly string _mapping;
        private readonly char[] _notches;
        private readonly int _offset;

        private Rotor(RotorDefinition def, char offset)
        {
            _mapping = def.Mappings;
            _notches = def.Notches;
            _offset = offset - 'A';
        }

        public static Rotor Create(string type, char offset = 'A')
        {
            return new Rotor(RotorDefinitions[type], offset);
        }

        public char GetMappedLetter(char letter, MappingDirection dir = MappingDirection.RightToLeft)
        {
            if (dir == MappingDirection.RightToLeft)
                return _mapping[(letter - 'A' + _offset) % AlphabetLength];

            return (char)('A' + ((_mapping.IndexOf(letter) - _offset + AlphabetLength) % AlphabetLength));
        }

        public bool IsNotch(char letter)
        {
            return _notches.Contains(letter);
        }
    }
}
