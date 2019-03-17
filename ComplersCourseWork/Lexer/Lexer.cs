using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeorForm_lab1.Lexer
{
    class Lexer
    {
        private Lexer(TextData data)
        {
            _textData = data;
            _builder = new StringBuilder();
        }

        internal struct TokenInfo
        {
            internal SyntaxKind kind;
            internal SpecialType specialType;
            internal string Text;
            internal int position;

            internal string StringValue;
            internal char CharValue;
            internal double DoubleValue;
            internal int Int32Value;
        }

        private readonly TextData _textData;
        private readonly StringBuilder _builder;
        //private int counter;

        internal static List<ISyntaxToken> GetTokens(TextData data)
        {
            TokenInfo tokenInfo = new TokenInfo
            {
                kind = SyntaxKind.None,
                specialType = SpecialType.None,
            };

            var lexer = new Lexer(data);

            var tempTokens = new List<ISyntaxToken>();

            while (!data.IsEndOfData)
            {
                switch (data.PeekChar())
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\0':
                        //Here we skip whitespace
                        data.AdvanceChar();
                        tokenInfo.kind = SyntaxKind.None;
                        break;

                    case '+':
                        tokenInfo.position = data.Position;

                        if (data.PeekChar(1) == '+')
                        {
                            tokenInfo.kind = SyntaxKind.PlusPlusToken;
                            data.AdvanceChar(2);
                        }  
                        else if (data.PeekChar(1) == '=')
                        {
                            tokenInfo.kind = SyntaxKind.PlusEqualToken;
                            data.AdvanceChar(2);
                        }
                        else
                        {
                            tokenInfo.kind = SyntaxKind.PlusToken;
                            data.AdvanceChar();
                        }
                        break;

                    case '-':
                        tokenInfo.position = data.Position;

                        if (data.PeekChar(1) == '-')
                        {
                            tokenInfo.kind = SyntaxKind.MinusMinusToken;
                            data.AdvanceChar(2);
                        }
                        else if (data.PeekChar(1) == '=')
                        {
                            tokenInfo.kind = SyntaxKind.MinusEqualToken;
                            data.AdvanceChar(2);
                        }
                        else
                        {
                            tokenInfo.kind = SyntaxKind.MinusToken;
                            data.AdvanceChar();
                        }
                        break;

                    case '*':
                        tokenInfo.position = data.Position;

                        if (data.PeekChar(1) == '=')
                        {
                            tokenInfo.kind = SyntaxKind.MultEqualToken;
                            data.AdvanceChar(2);
                        }
                        else
                        {
                            tokenInfo.kind = SyntaxKind.MultToken;
                            data.AdvanceChar();
                        }
                        
                        break;

                    case '/':
                        tokenInfo.position = data.Position;

                        if (data.PeekChar(1) == '=')
                        {
                            tokenInfo.kind = SyntaxKind.DivEqualToken;
                            data.AdvanceChar(2);
                        }
                        else
                        {
                            tokenInfo.kind = SyntaxKind.DivEqualToken;
                            data.AdvanceChar();
                        }

                        break;

                    case '=':
                        tokenInfo.position = data.Position;

                        if (data.PeekChar(1) == '=')
                        {
                            tokenInfo.kind = SyntaxKind.EqualEqualToken;
                            data.AdvanceChar(2);
                        }
                        else
                        {
                            tokenInfo.kind = SyntaxKind.EqualToken;
                            data.AdvanceChar();
                        }

                        break;

                    case '(':
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.OpeningBracket;
                        data.AdvanceChar();
                        break;

                    case '{':
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.OpeningBrace;
                        data.AdvanceChar();
                        break;

                    case ')':
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.ClosingBracket;
                        data.AdvanceChar();
                        break;

                    case '}':
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.ClosingBrace;
                        data.AdvanceChar();
                        break;

                    case '\'':
                        lexer.ScanCharacter(ref tokenInfo);
                        break;

                    case ';':
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.SemicolonToken;
                        data.AdvanceChar();
                        break;

                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        lexer.ScanIdentifierOrKeyword(ref tokenInfo);
                        break;

                    case '"':
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
                        lexer.ScanNumericOrLiteralValue(ref tokenInfo);
                        break;
                    default:
                        tokenInfo.position = data.Position;
                        tokenInfo.kind = SyntaxKind.Unknown;
                        tokenInfo.Text = data.PeekChar().ToString();
                        data.AdvanceChar();
                        break;
                }

                if (tokenInfo.kind!= SyntaxKind.None)
                {
                    tempTokens.Add(GetToken(ref tokenInfo));
                }
            }



            return tempTokens;
        }

        internal static ISyntaxToken GetToken(ref TokenInfo tokenInfo)
        {
            switch (tokenInfo.kind)
            {
                case SyntaxKind.None:
                    throw new NotImplementedException();
                    break;
                case SyntaxKind.StringKeyword:
                    throw new NotImplementedException();
                    break;
                case SyntaxKind.IntKeyword:
                    throw new NotImplementedException();
                    break;
                case SyntaxKind.IdentifierToken:
                    return new SyntaxIdentifierToken(SyntaxKind.IdentifierToken, tokenInfo.Text, tokenInfo.position);
                case SyntaxKind.StringLiteralToken:
                    if (tokenInfo.specialType == SpecialType.System_String)
                        return new SyntaxValueToken<string>(tokenInfo.StringValue, tokenInfo.kind, tokenInfo.position);
                    else
                        throw new FormatException();

                case SyntaxKind.DoubleLiteralToken:
                    if (tokenInfo.specialType == SpecialType.System_Double)
                        return new SyntaxValueToken<double>(tokenInfo.DoubleValue, tokenInfo.kind, tokenInfo.position);
                    else
                        throw new FormatException();

                case SyntaxKind.IntLiteralToken:
                    if (tokenInfo.specialType == SpecialType.System_Int32)
                        return new SyntaxValueToken<int>(tokenInfo.Int32Value, tokenInfo.kind, tokenInfo.position);
                    else
                        throw new FormatException();

                case SyntaxKind.CharLiteralToken:
                    if (tokenInfo.specialType == SpecialType.System_Char)
                        return new SyntaxValueToken<char>(tokenInfo.CharValue, tokenInfo.kind, tokenInfo.position);
                    else
                        throw new FormatException();

                case SyntaxKind.EqualToken:
                case SyntaxKind.EqualEqualToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.PlusPlusToken:
                case SyntaxKind.PlusEqualToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.MinusMinusToken:
                case SyntaxKind.MinusEqualToken:
                case SyntaxKind.MultToken:
                case SyntaxKind.MultEqualToken:
                case SyntaxKind.DivToken:
                case SyntaxKind.DivEqualToken:
                case SyntaxKind.DotToken:
                case SyntaxKind.OpeningBracket:
                case SyntaxKind.ClosingBracket:
                case SyntaxKind.OpeningBrace:
                case SyntaxKind.ClosingBrace:
                case SyntaxKind.ForKeyword:
                case SyntaxKind.WhileKeyword:
                case SyntaxKind.IfKeyword:
                case SyntaxKind.SemicolonToken:
                    return new SyntaxTriviaToken(tokenInfo.kind, tokenInfo.position);
                case SyntaxKind.Unknown:
                    return new SyntaxUnknownToken(tokenInfo.position, tokenInfo.Text);
                default:
                    throw new NotImplementedException();
            }
        }

        private void ScanIdentifierOrKeyword(ref TokenInfo tokenInfo)
        {
            var startOffset = _textData.Position;
            var currentOffset = startOffset;
            var chars = _textData.SourceCode;

            while (chars.Length > currentOffset)
            {
                switch (chars[currentOffset])
                {
                    case 'a': case 'b':
                    case 'c': case 'd': case 'e': case 'f': case 'g':
                    case 'h': case 'i': case 'j': case 'k': case 'l':
                    case 'm': case 'n': case 'o': case 'p': case 'q':
                    case 'r': case 's': case 't': case 'u': case 'v':
                    case 'w': case 'x': case 'y': case 'z': case 'A':
                    case 'B': case 'C': case 'D': case 'E': case 'F':
                    case 'G': case 'H': case 'I': case 'J': case 'K':
                    case 'L': case 'M': case 'N': case 'O': case 'P':
                    case 'Q': case 'R': case 'S': case 'T': case 'U':
                    case 'V': case 'W': case 'X': case 'Y': case 'Z':
                        currentOffset++;
                        break;

                    case '0': case '1': case '2': case '3': case '4':
                    case '5': case '6': case '7': case '8': case '9':
                        if(currentOffset==startOffset)
                        {
                            throw new FormatException("Первым символом идентификатора не может быть число");
                        }
                        currentOffset++;
                        break;

                    default:
                        goto EndOfLoop;
                }
            }

        EndOfLoop:

            var length = currentOffset - startOffset;
            tokenInfo.Text = chars.Substring(startOffset, length);

            if(TryGetKeyword(tokenInfo.Text, out var kind))
            {
                tokenInfo.kind = kind;
            }
            else
            {
                tokenInfo.kind = SyntaxKind.IdentifierToken;
            }

            _textData.AdvanceChar(length);
        }

        private bool TryGetKeyword(string keyword, out SyntaxKind kind)
        {
            switch (keyword)
            {
                case "for":
                    kind = SyntaxKind.ForKeyword;
                    return true;
                case "if":
                    kind = SyntaxKind.IfKeyword;
                    return true;
                case "while":
                    kind = SyntaxKind.WhileKeyword;
                    return true;
                default:
                    kind = SyntaxKind.None;
                    return false;
            }
        }

        private void ScanNumericOrLiteralValue(ref TokenInfo tokenInfo)
        {
            if(_textData.PeekChar() == '"')
            {
                var startOffset = _textData.Position + 1;
                var currentOffset = startOffset;
                var chars = _textData.SourceCode;

                while (chars[currentOffset++] != '"')
                {
                    if(currentOffset >= chars.Length)
                    {
                        throw new FormatException("Кавычки не закрыты");
                    }
                }

                var length = currentOffset - startOffset - 1;

                if(length <=0)
                {
                    throw new FormatException();
                }

                tokenInfo.StringValue = chars.Substring(startOffset, length);
                tokenInfo.kind = SyntaxKind.StringLiteralToken;
                tokenInfo.specialType = SpecialType.System_String;
                tokenInfo.position = startOffset;
                _textData.AdvanceChar(length + 2);
            }
            else
            {
                var startOffset = _textData.Position;
                var currentOffset = startOffset;
                var intValueOffset = 0;
                var chars = _textData.SourceCode;
                var isDouble = false;
                var isExponentExist = false;

                while (chars.Length>currentOffset)
                {
                    switch(chars[currentOffset])
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
                            currentOffset++;
                            if (isDouble)
                                isExponentExist = true;

                            break;
                        case '.':
                            intValueOffset = currentOffset - 1;

                            if (isDouble)
                                goto default;

                            if (currentOffset == startOffset)
                                throw new FormatException();

                            isDouble = true;
                            currentOffset++;
                            break;
                        default:
                            goto EndOfLoop;
                    }
                }

            EndOfLoop:

                if(isDouble && isExponentExist)
                {
                    var length = currentOffset - startOffset;

                    if (length <= 0)
                    {
                        throw new FormatException();
                    }

                    tokenInfo.Text = chars.Substring(startOffset, length);
                    tokenInfo.DoubleValue = double.Parse(tokenInfo.Text, CultureInfo.InvariantCulture);
                    tokenInfo.kind = SyntaxKind.DoubleLiteralToken;
                    tokenInfo.specialType = SpecialType.System_Double;
                    tokenInfo.position = startOffset;
                    _textData.AdvanceChar(length);
                }
                else
                {
                    if (isDouble && !isExponentExist)
                        currentOffset = intValueOffset;

                    var length = currentOffset - startOffset;

                    if (length <= 0)
                    {
                        throw new FormatException();
                    }

                    tokenInfo.Text = chars.Substring(startOffset, length);
                    tokenInfo.Int32Value = int.Parse(tokenInfo.Text);
                    tokenInfo.kind = SyntaxKind.IntLiteralToken;
                    tokenInfo.specialType = SpecialType.System_Int32;
                    tokenInfo.position = startOffset;
                    _textData.AdvanceChar(length);
                }
            }
        }

        private void ScanCharacter(ref TokenInfo tokenInfo)
        {
            var startOffset = _textData.Position + 1;
            var currentOffset = startOffset;
            var chars = _textData.SourceCode;

            if (chars[startOffset - 1] != '\'')
            {
                throw new FormatException();
            }

            while (chars[currentOffset++] != '\'')
            {
                if (currentOffset >= chars.Length)
                {
                    throw new FormatException("Кавычки не закрыты");
                }
            }

            var length = currentOffset - startOffset - 1;

            if (length <= 0)
            {
                throw new FormatException();
            }

            tokenInfo.Text = chars.Substring(startOffset, length);
            tokenInfo.CharValue = char.Parse(tokenInfo.Text);
            tokenInfo.kind = SyntaxKind.CharLiteralToken;
            tokenInfo.specialType = SpecialType.System_Char;
            tokenInfo.position = startOffset;
            _textData.AdvanceChar(length + 2);
        }
    }

    class SyntaxValueToken<T> : ISyntaxToken
    {
        public SyntaxValueToken(T value, SyntaxKind syntaxKind, int position)
        {
            Value = value;
            SyntaxKind = syntaxKind;
            SourceTextPosition = position;
        }

        public SyntaxKind SyntaxKind { get; }
        public T Value { get; }
        public int SourceTextPosition { get; }
    }

    class SyntaxIdentifierToken : ISyntaxToken
    {
        public SyntaxIdentifierToken(SyntaxKind syntaxKind, string identifierName, int position)
        {
            IdentifierName = identifierName;
            SourceTextPosition = position;
        }

        public SyntaxKind SyntaxKind => SyntaxKind.IdentifierToken;
        public string IdentifierName { get; }
        public int SourceTextPosition { get; }
    }

    class SyntaxTriviaToken : ISyntaxToken
    {
        public SyntaxTriviaToken(SyntaxKind syntaxKind, int position)
        {
            SyntaxKind = syntaxKind;
            SourceTextPosition = position;
        }

        public SyntaxKind SyntaxKind { get; }
        public int SourceTextPosition { get; }
    }

    class SyntaxUnknownToken : ISyntaxToken
    {
        public SyntaxUnknownToken(int sourceTextPosition, string text)
        {
            SourceTextPosition = sourceTextPosition;
            Text = text;
        }

        public SyntaxKind SyntaxKind => SyntaxKind.Unknown;

        public int SourceTextPosition { get; }

        public string Text { get; }
    }

    enum SyntaxKind:ushort
    {
        None,
        Unknown,
        StringKeyword,
        IntKeyword,
        IdentifierToken,
        StringLiteralToken,
        DoubleLiteralToken,
        IntLiteralToken,
        CharLiteralToken,
        SemicolonToken,

        //Arithmetic operators
        EqualToken,
        EqualEqualToken,
        PlusToken,
        PlusPlusToken,
        PlusEqualToken,
        MinusToken,
        MinusMinusToken,
        MinusEqualToken,
        MultToken,
        MultEqualToken,
        DivToken,
        DivEqualToken,
        DotToken,

        //Brackets
        OpeningBracket,
        ClosingBracket,
        OpeningBrace,
        ClosingBrace,

        //Keywords
        ForKeyword,
        WhileKeyword,
        IfKeyword,
    };

    enum SpecialType
    {
        None,
        System_Int32,
        System_String,
        System_Double,
        System_Char,
    };
}
