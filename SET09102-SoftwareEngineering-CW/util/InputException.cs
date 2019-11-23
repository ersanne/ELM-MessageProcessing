﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET09102_SoftwareEngineering_CW.exceptions
{
    class InputException : Exception
    {
        public InputException()
        {
        }

        public InputException(string message)
            : base(message)
        {
        }

        public InputException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}