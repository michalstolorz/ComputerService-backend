using System;

namespace ComputerService.Core.Exceptions
{
    public abstract class CustomException : Exception
    {
        public string Code { get; }

        protected CustomException()
        {
        }

        public CustomException(string code)
        {
            Code = code;
        }

        public CustomException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public CustomException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public CustomException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public CustomException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
