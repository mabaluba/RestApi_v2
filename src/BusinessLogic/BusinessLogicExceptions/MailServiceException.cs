using System;

namespace BusinessLogic.BusinessLogicExceptions
{
    public class MailServiceException : Exception
    {
        public MailServiceException()
        {
        }

        public MailServiceException(string message) : base(message)
        {
        }

        public MailServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}