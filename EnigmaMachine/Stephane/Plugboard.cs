namespace EnigmaMachine.Stephane
{
    public sealed class Plugboard : LetterMapper
    {
        private const string DefaultMappings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public Plugboard(string mappings = DefaultMappings) : base(mappings)
        {
        }
    }
}
