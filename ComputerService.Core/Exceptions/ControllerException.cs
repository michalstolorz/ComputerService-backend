using System;

namespace ComputerService.Core.Exceptions
{
    public class ControllerException : CustomException
    {
        protected ControllerException()
        {
        }

        public ControllerException(string code) : base(code)
        {
        }

        public ControllerException(string message, params object[] args)
            : base(string.Empty, message, args)
        {
        }

        public ControllerException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }

        public ControllerException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public ControllerException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
        }
    }
}
