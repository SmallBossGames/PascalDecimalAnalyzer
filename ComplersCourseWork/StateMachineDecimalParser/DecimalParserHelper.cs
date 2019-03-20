using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeorForm_lab1.Lexer;
using TeorForm_lab1.RecursiveDescent;

namespace ComplersCourseWork.StateMachineDecimalParser
{
    static class DecimalParserHelper
    {
        public static bool ParseDecimalConst(string source, out List<Warning> warningsCollection, out string result)
        {
            var textData = new TextData(source);
            var parser = new DecimalParser();
            return parser.ParseDecimalConst(textData, out warningsCollection, out result);
        }
    }
}
