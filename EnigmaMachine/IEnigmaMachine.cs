namespace EnigmaMachine
{
    public interface IEnigmaMachine
    {
        char PressKey(char letter);
        void SetupPlugboard(string mappings);
        void SetupRotors(RotorInfo[] rotorInfos);
        void SetupReflector(string type);
        char[] GetCurrentRotorRingLetters();
        void SetStartupRotorRingLetters(char[] letters);
        void ResetRotors();
    }
}
