using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeorForm_lab1.Lexer;

namespace ComplersCourseWork.StateMachineDecimalParser
{
    class DecimalParser
    {
        private DecimalParseMode mode;
        private LinkedList<Warning> warnings;
        private TextData textData;
        private StringBuilder resultString;

        public DecimalParserStatus ParseDecimalConst(TextData data)
        {
            mode = DecimalParseMode.DecimalConst;
            warnings = new LinkedList<Warning>();
            textData = data;
            resultString = new StringBuilder();

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
                    case DecimalParseMode.UnsignedIntegerWithExponent:
                        ParseUnsignedIntegerWithExponent();
                        break;
                    case DecimalParseMode.NullStartDecimal:
                        ParseNullStartDecimal();
                        break;
                    case DecimalParseMode.Ending:
                        return new DecimalParserStatus(
                            warnings.All(x => x.WarningType != WarningType.Error),
                            warnings,
                            resultString.ToString());
                    case DecimalParseMode.UnsignedInteger:
                        ParseUnsignedInteger();
                        break;
                    case DecimalParseMode.NullStartInteger:
                        ParseNullStartInteger();
                        break;
                    case DecimalParseMode.SignedInteger:
                        ParseSignedInteger();
                        break;
                    case DecimalParseMode.UnsignedIntegerWithNull:
                        ParseUnsignedIntegerWithNull();
                        break;
                    default:
                        throw new NotImplementedException();
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
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case '0':
                        mode = DecimalParseMode.NullStartDecimal;
                        textData.AdvanceChar();
                        return;
                    case '+':
                    case '-':
                        mode = DecimalParseMode.UnsignedDecimalConst;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case '\0':
                        MakeWarningMinimal(
                            "Value cannot be empty",
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
                        SaveCharacter();
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
                        MakeWarningMinimal(
                            "Value cannot be empty",
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
                        SaveCharacter();
                        textData.AdvanceChar();
                        break;
                    case '.':
                        mode = DecimalParseMode.UnsignedIntegerWithExponent;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case ',':
                        mode = DecimalParseMode.UnsignedIntegerWithExponent;
                        SaveCharacter('.');
                        MakeWarning(
                            "There can only be digit from 0 to 9 or '.' character",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case 'E':
                    case 'e':
                        mode = DecimalParseMode.SignedInteger;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
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

        void ParseUnsignedIntegerWithExponent()
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
                        SaveCharacter();
                        textData.AdvanceChar();
                        break;
                    case 'E':
                    case 'e':
                        mode = DecimalParseMode.SignedInteger;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
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

        void ParseNullStartDecimal()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '0':
                        MakeWarning(
                            "Null at the beginning is excess",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
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
                        SaveCharacter();
                        MakeWarning(
                            "Null at the beginning is excess",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case 'E':
                    case 'e':
                        mode = DecimalParseMode.SignedInteger;
                        SaveCharacter();
                        MakeWarning(
                            "Null with exponent equal null",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case '.':
                        mode = DecimalParseMode.UnsignedIntegerWithExponent;
                        SaveCharacter('0');
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case ',':
                        mode = DecimalParseMode.UnsignedIntegerWithExponent;
                        SaveCharacter('0');
                        SaveCharacter('.');
                        MakeWarning(
                            "There can only be digit from 0 to 9 or '.' character",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        SaveCharacter('0');
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


        void ParseSignedInteger()
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
                        mode = DecimalParseMode.UnsignedIntegerWithNull;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case '0':
                        mode = DecimalParseMode.NullStartInteger;
                        textData.AdvanceChar();
                        return;
                    case '+': case '-':
                        mode = DecimalParseMode.UnsignedInteger;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        MakeWarningMinimal(
                            "Exponent cannot be empty",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9 or sign",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseNullStartInteger()
        {
            while (true)
            {
                switch (textData.PeekChar())
                {
                    case '0':
                        MakeWarning(
                            "Null at the beginning is excess",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
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
                        mode = DecimalParseMode.UnsignedIntegerWithNull;
                        SaveCharacter();
                        MakeWarning(
                            "Null at the beginning is excess",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Warning);
                        textData.AdvanceChar();
                        return;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        SaveCharacter('0');
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9 or sign",
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
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        mode = DecimalParseMode.UnsignedIntegerWithNull;
                        SaveCharacter();
                        textData.AdvanceChar();
                        return;
                    case '0':
                        mode = DecimalParseMode.NullStartInteger;
                        textData.AdvanceChar();
                        return;
                    case '\0':
                    case ' ':
                    case '\t':
                    case '\n':
                        MakeWarningMinimal(
                           "Value cannot be empty",
                           textData.PeekChar(),
                           textData.Position,
                           WarningType.Error);
                        mode = DecimalParseMode.Ending;
                        return;
                    default:
                        MakeWarning(
                            "Unknown character! There can only be digit from 0 to 9 or sign",
                            textData.PeekChar(),
                            textData.Position,
                            WarningType.Error);
                        textData.AdvanceChar();
                        break;
                }
            }
        }

        void ParseUnsignedIntegerWithNull()
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
                        SaveCharacter();
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

        void SaveCharacter(char value)
        {
            resultString.Append(value);
        }

        void SaveCharacter() => SaveCharacter(textData.PeekChar());

        void MakeWarning(string text, char character, int position, WarningType warningType)
        {
            warnings.AddLast(new Warning(text, character, position, warningType));
        }

        void MakeWarningMinimal(string text, char character, int position, WarningType warningType)
        {
            warnings.AddLast(new WarningMinimal(text, character, position, warningType));
        }
    }

    enum DecimalParseMode:byte
    {
        DecimalConst,
        UnsignedDecimalConst,
        DecimalConstWithNull,
        NullStartDecimal,
        UnsignedIntegerWithExponent,
        UnsignedIntegerWithNull,
        UnsignedInteger,
        NullStartInteger,
        SignedInteger,
        Ending,
    }
}
