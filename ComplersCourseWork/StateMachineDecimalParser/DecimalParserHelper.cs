using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeorForm_lab1.Lexer;

namespace ComplersCourseWork.StateMachineDecimalParser
{
    static class DecimalParserHelper
    {
        public static DecimalParserStatus ParseDecimalConst(string source)
        {
            var textData = new TextData(source);
            var parser = new DecimalParser();
            return parser.ParseDecimalConst(textData);
        }
    }
}
