namespace BusinessLogic.NotivicationServices
{
    using System.Threading.Tasks;

    internal interface ISendSmsService
    {
        Task SendSmsAsync(string phoneNumber);
    }
}