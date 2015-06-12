using System.Collections.Generic;
using System.Linq;

namespace EnigmaMachine
{
    public static class EnigmaMachineExtensions
    {
        public static string Encrypt(this IEnigmaMachine machine, string text)
        {
            return new string(text.ToCharArray().Select(machine.EncryptLetter).ToArray());
        }

        private static char EncryptLetter(this IEnigmaMachine machine, char letter)
        {
            if (!char.IsLetter(letter))
                return letter;

            bool isLower = char.IsLower(letter);
            letter = machine.PressKey(char.ToUpperInvariant(letter));
            letter = isLower ? char.ToLowerInvariant(letter) : letter;

            return letter;
        }

        public static void SetupPlugboard(this IEnigmaMachine machine, IEnumerable<char> mappings)
        {
            machine.SetupPlugboard(new string(mappings.ToArray()));
        }
    }
}
