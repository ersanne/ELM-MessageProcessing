using System;

namespace ELMPrototype.exceptions
{
    /// <summary>
    ///     Custom exceptions for invalid inputs
    /// </summary>
    public class InputException : Exception
    {
        public InputException(string message) : base(message)
        {
        }
    }
}