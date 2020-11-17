using System;

namespace ComputerService.Core.Exceptions
{
    public class HelperException : CustomException
    {
        protected HelperException()
        {
        }

        public HelperException(string code) : base(code)
        {
        }

        public HelperException(string message, params object[] args)
            : base(string.Empty, message, args)
        {
        }

        public HelperException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }

        public HelperException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public HelperException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
        }
    }
}
