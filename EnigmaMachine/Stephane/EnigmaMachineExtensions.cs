using System.Linq;

namespace EnigmaMachine.Stephane
{
    public static class EnigmaMachineExtensions
    {
        public static string Encrypt(this EnigmaMachine machine, string text)
        {
            return new string(text.ToCharArray().Select(machine.EncryptLetter).ToArray());
        }

        private static char EncryptLetter(this EnigmaMachine machine, char letter)
        {
            if (!char.IsLetter(letter))
                return letter;

            bool isLower = char.IsLower(letter);
            letter = machine.PressKey(char.ToUpperInvariant(letter));
            letter = isLower ? char.ToLowerInvariant(letter) : letter;

            return letter;
        }
    }
}
