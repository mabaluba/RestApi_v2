using System.Threading.Tasks;
using BusinessLogic.BusinessLogicExceptions;

namespace BusinessLogic.NotivicationServices
{
    internal class SendSmsService : ISendSmsService
    {
        public Task SendSmsAsync(string phoneNumber)
        {
            // send sms to the phoneNumber
            throw new SmsServiceException("SMS ASYNC service`s not implemented.");
        }
    }
}