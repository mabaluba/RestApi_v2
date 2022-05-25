using System;
using System.Threading.Tasks;
using BusinessLogic.BusinessLogicExceptions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using UniversityDomain.EntityInterfaces;

namespace BusinessLogic.NotivicationServices
{
    internal class SendEmailService : ISendEmailService
    {
        private readonly EducationMailContacts _options;

        public SendEmailService(IOptionsMonitor<EducationMailContacts> options)
        {
            _options = options.CurrentValue ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendEmailAsync(IAverageGrade studentInfo, ITeacher teacher, int attendanceCount)
        {
            _ = studentInfo ?? throw new ArgumentNullException(nameof(studentInfo));
            _ = teacher ?? throw new ArgumentNullException(nameof(studentInfo));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.DisplayName, _options.Mail));
            message.To.Add(new MailboxAddress($"Dear {studentInfo.FirstName} {studentInfo.LastName}", studentInfo.Email));
            message.To.Add(new MailboxAddress($"Dear {teacher.FirstName} {teacher.LastName}", teacher.Email));
            message.Subject = "Warning - Course attendance problem!";

            message.Body = new TextPart("plain")
            {
                Text = $"Teacher {teacher.FirstName} {teacher.LastName}. Student {studentInfo.FirstName} {studentInfo.LastName} missed {attendanceCount} lections!"
            };

            // sending e-mails
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.friends.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync("joey", "password");
                await client.SendAsync(message);
            }
            catch (Exception exception)
            {
                throw new MailServiceException("Unable to use ASYNC mail service.", exception);
            }
            finally
            {
                 await client.DisconnectAsync(true);
            }
        }
    }
}