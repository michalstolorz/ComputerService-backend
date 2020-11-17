using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Exceptions
{
    public class ValidatorException : CustomException
    {
        protected ValidatorException()
        {
        }

        public ValidatorException(string code) : base(code)
        {
        }

        public ValidatorException(string message, params object[] args)
            : base(string.Empty, message, args)
        {
        }

        public ValidatorException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }

        public ValidatorException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public ValidatorException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
        }
    }
}
