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
        public static bool ParseDecimalConst(string data, out List<Warning> warningsCollection)
        {
            var textData = new TextData(data);
            var parser = new DecimalParser();
            return parser.ParseDecimalConst(textData, out warningsCollection);
        }
    }
}
