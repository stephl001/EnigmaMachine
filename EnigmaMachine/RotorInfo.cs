namespace EnigmaMachine
{
    public sealed class RotorInfo
    {
        public RotorInfo(string type, char startingOffset = 'A', char ringSettingOffset = 'A')
        {
            Type = type;
            RingSettingOffset = ringSettingOffset;
            StartingOffset = startingOffset;
        }

        public string Type { get; }
        public char RingSettingOffset { get; }
        public char StartingOffset { get; }
    }
}
