using System;

namespace ComputerService.Core.Exceptions
{
    public class RepositoryException : CustomException
    {
        protected RepositoryException()
        {
        }

        public RepositoryException(string code) : base(code)
        {
        }

        public RepositoryException(string message, params object[] args)
            : base(string.Empty, message, args)
        {
        }

        public RepositoryException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }

        public RepositoryException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public RepositoryException(Exception innerException, string code, string message, params object[] args)
            : base(code, string.Format(message, args), innerException)
        {
        }
    }
}
