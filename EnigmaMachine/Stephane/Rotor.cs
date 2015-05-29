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

    public sealed class Rotor : LetterMapper
    {
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
        
        private Rotor(RotorDefinition def, char offset)
            : base(def.Mappings, offset)
        {
            _notches = def.Notches;
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
