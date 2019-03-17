namespace TeorForm_lab1.Lexer
{
    class TextData
    {
        public int Position { get; private set; }

        public string SourceCode { get; }

        public bool IsEndOfData => Position == SourceCode.Length;

        public TextData(string source)
        {
            SourceCode = source;
        }

        public char PeekChar()
        {
            if (Position == SourceCode.Length)
                return '\0';

            return SourceCode[Position];
        }

        public char PeekChar(int offset)
        {
            if (Position + offset == SourceCode.Length)
                return '\0';

            return SourceCode[Position + offset];
        }

        public void AdvanceChar() 
            => Position += 1;

        public void AdvanceChar(int offset)
            => Position += offset;
    }
}
