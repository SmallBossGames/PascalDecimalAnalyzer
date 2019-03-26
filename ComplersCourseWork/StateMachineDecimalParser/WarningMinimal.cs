namespace ComplersCourseWork.StateMachineDecimalParser
{
    class WarningMinimal : Warning
    {
        public WarningMinimal(string text, char character, int position, WarningType warningType) 
            : base(text, character, position, warningType)
        {
        }

        public override string ToString()
        {
            return $"{WarningType}: Position {Position};\nInfo: {Text};";
        }
    }
}
