namespace ComplersCourseWork.StateMachineDecimalParser
{
    class Warning
    {
        public Warning(string text, char character, int position, WarningType warningType)
        {
            Text = text;
            Position = position;
            WarningType = warningType;
            Character = character;
        }

        public string Text { get; }
        public int Position { get; }
        public WarningType WarningType { get; }
        public char Character { get; }

        public override string ToString()
        {
            return $"{WarningType}: Chartacter '{Character}' at position {Position};\nInfo: {Text};";
        }
    }
}
