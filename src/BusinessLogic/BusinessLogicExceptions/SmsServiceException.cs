using System;

namespace BusinessLogic.BusinessLogicExceptions
{
    public class SmsServiceException : Exception
    {
        public SmsServiceException()
        {
        }

        public SmsServiceException(string message) : base(message)
        {
        }

        public SmsServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}