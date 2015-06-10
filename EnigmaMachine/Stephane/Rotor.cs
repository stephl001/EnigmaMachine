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

    public class Rotor : LetterMapper
    {
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
            {"Reflector A", new RotorDefinition("EJMZALYXVBWFCRQUONTSPIKHGD", new char[] {})},
            {"Reflector B", new RotorDefinition("YRUHQSLDPXNGOKMIEBFZCWVJAT", new char[] {})},
            {"Reflector C", new RotorDefinition("FVPJIAOYEDRZXWGCTKUQSBNMHL", new char[] {})},
            {"ETW", new RotorDefinition("ABCDEFGHIJKLMNOPQRSTUVWXYZ", new char[] {})}
        };

        private readonly char[] _notches;
        private readonly int _offset;
        
        protected Rotor(RotorDefinition def, char offsetLetter)
            : base(def.Mappings)
        {
            _notches = def.Notches;
            _offset = offsetLetter - 'A';
        }

        public override char GetMappedLetter(char letter, MappingDirection dir = MappingDirection.RightToLeft)
        {
            if (dir == MappingDirection.RightToLeft)
            {
                char innerMapping = Mapping[letter.AddOffset(-_offset) - 'A'];
                return innerMapping.AddOffset(_offset);
            }

            char innerInput = letter.AddOffset(-_offset);
            return (char)(((Mapping.IndexOf(innerInput) + _offset) % AlphabetLength) + 'A');
        }

        public static Rotor Create(string type, char offset = 'A')
        {
            return new Rotor(RotorDefinitions[type], offset);
        }

        public bool IsNotch(char letter)
        {
            return _notches.Contains(letter);
        }
    }
}
