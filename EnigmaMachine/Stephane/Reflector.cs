using System.Collections.Generic;

namespace EnigmaMachine.Stephane
{
    public sealed class Reflector : Rotor
    {
        private Reflector(RotorDefinition def)
            : base(def, 'A')
        {
        }

        private static readonly IDictionary<string, RotorDefinition> RotorDefinitions = new Dictionary<string, RotorDefinition>
        {
            {"Reflector A", new RotorDefinition("EJMZALYXVBWFCRQUONTSPIKHGD", new char[] {})},
            {"Reflector B", new RotorDefinition("YRUHQSLDPXNGOKMIEBFZCWVJAT", new char[] {})},
            {"Reflector C", new RotorDefinition("FVPJIAOYEDRZXWGCTKUQSBNMHL", new char[] {})}
        };

        public static Reflector Create(string type)
        {
            return new Reflector(RotorDefinitions[type]);
        }
    }
}
