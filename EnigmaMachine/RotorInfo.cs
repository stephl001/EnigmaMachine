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

        public string Type { get; private set; }
        public char RingSettingOffset { get; private set; }
        public char StartingOffset { get; private set; }
    }
}
