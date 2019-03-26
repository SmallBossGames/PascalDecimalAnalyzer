using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplersCourseWork.StateMachineDecimalParser
{
    readonly struct DecimalParserStatus
    {
        public DecimalParserStatus(bool success, IReadOnlyCollection<Warning> warnings, string result) : this()
        {
            Success = success;
            Warnings = warnings;
            Result = result;
        }

        public bool Success { get; }
        public IReadOnlyCollection<Warning> Warnings { get; }
        public string Result { get; }
    }
}
