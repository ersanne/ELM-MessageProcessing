﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELMPrototype.exceptions
{
    /// <summary>
    /// Custom exceptions for invalid inputs
    /// </summary>
    public class InputException : Exception
    {
        public InputException()
        {
        }

        public InputException(string message) : base(message)
        {
        }

        public InputException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}