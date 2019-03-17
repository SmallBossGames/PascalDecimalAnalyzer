using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeorForm_lab1.Lexer
{
    interface ISyntaxToken
    {
        SyntaxKind SyntaxKind { get; }
        int SourceTextPosition { get; }
    }
}
