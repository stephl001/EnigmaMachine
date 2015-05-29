namespace EnigmaMachine.Stephane
{
    public class Plugboard : LetterMapper
    {
        private const string DefaultMappings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public Plugboard(string mappings = DefaultMappings) : base(mappings)
        {
        }
    }
}
