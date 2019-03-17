using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeorForm_lab1.Lexer;

namespace TeorForm_lab1.RecursiveDescent
{
    class DecimalParser
    {
        private DecimalParseMode mode;
        private List<Warning> warnings;
        private TextData textData;

        public bool ParseDecimalConst(TextData data, out List<Warning> warningsCollection)
        {
            mode = DecimalParseMode.DecimalConst;
            warnings = new List<Warning>();
            textData = data;

            while (true)
            {
                switch (mode)
                {
                    case DecimalParseMode.DecimalConst:
                        ParseDecimal();
                        break;
                    case DecimalParseMode.UnsignedDecimalConst:
                        ParseUnsignedDecimal();
                        break;
                    case DecimalParseMode.DecimalConstWithNull:
                        ParseUnsignedDecimalWithNull();
                        break;
                    case DecimalParseMode.UnsignedInteger:
                        ParseUnsignedInteger();
                        break;
                    case DecimalParseMode.NullStartDecimal:
                        ParseNullStartDecimal();
                        break;
                    case DecimalParseMode.Ending:
                        warningsCollection = warnings;
                        return warnings.All(x => x.WarningType != WarningType.Error);
                    default:
                        warningsCollection = warnings;
                        return false;
                }
            }
        }

        void ParseDecimal()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case ' ':
                    case '\t':
                    case '\n':
                        //Here we ignore whitespace
                        textData.AdvanceChar();
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        mode = DecimalParseMode.DecimalConstWithNull;
                        textData.AdvanceChar();
                        return;
                    case '0':
                        mode = DecimalParseMode.NullStartDecimal;
                        textData.AdvanceChar();
                        return;
                    case '+':
                    case '-':
                        mode = DecimalParseMode.UnsignedDecimalConst;
                        textData.AdvanceChar();
                        return;
                    case '\0':
                        MakeWarning(
                            "Unknown character! There can only be digit or sign.",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit or sign.", 
                            textData.PeekChar(), 
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseUnsignedDecimal()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        mode = DecimalParseMode.DecimalConstWithNull;
                        textData.AdvanceChar();
                        return;
                    case '0':
                        mode = DecimalParseMode.NullStartDecimal;
                        textData.AdvanceChar();
                        return;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        MakeWarning(
                            "Unknown character! There can only be digit from 1 to 9",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 1 to 9",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseUnsignedDecimalWithNull()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        textData.AdvanceChar();
                        break;
                    case '.':
                        mode = DecimalParseMode.UnsignedInteger;
                        textData.AdvanceChar();
                        return;
                    case ',':
                        mode = DecimalParseMode.UnsignedInteger;
                        MakeWarning(
                            "There can only be digit from 0 to 9 or '.' character",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        break;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9 or '.' character",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseUnsignedInteger()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        textData.AdvanceChar();
                        break;
                    case '\0':
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseNullStartDecimal()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        mode = DecimalParseMode.DecimalConstWithNull;
                        MakeWarning(
                            "Null at the beginning is excess",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case '.':
                        mode = DecimalParseMode.UnsignedInteger;
                        textData.AdvanceChar();
                        return;
                    case ',':
                        mode = DecimalParseMode.UnsignedInteger;
                        MakeWarning(
                            "There can only be digit from 0 to 9 or '.' character",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        break;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void MakeWarning(string text, char character, int position, WarningType warningType)
        {
            warnings.Add(new Warning(text, character, position, warningType));
        }
    }

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
            return $"{WarningType}: Chartacter '{Character}' at position {Position};\n Info: {Text};";
        }
    }

    enum DecimalParseMode:byte
    {
        DecimalConst,
        UnsignedDecimalConst,
        DecimalConstWithNull,
        NullStartDecimal,
        UnsignedInteger,
        Ending,
    }

    enum WarningType:byte
    {
        Error,
        Warning,
    }
}
