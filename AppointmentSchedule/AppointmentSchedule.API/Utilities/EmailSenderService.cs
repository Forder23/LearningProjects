using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentSchedule.API.Utilities;

namespace AppointmentSchedule.API.Utilities
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _Configuration;

        public EmailSenderService(IConfiguration config)
        {
            _Configuration = config;
        }
        public async Task<MailjetResponse> SendMail(string email,string htmlCode, string subject = null)
        {
            MailjetClient client = new(
                _Configuration.GetValue<string>("MailJetConfig:API_PUBLIC_KEY"), 
                _Configuration.GetValue<string>("MailJetConfig:API_SECRET_KEY"));

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, _Configuration.GetValue<string>("MailJetConfig:SenderEmail"))
               .Property(Send.FromName, _Configuration.GetValue<string>("MailJetConfig:SenderName"))
               .Property(Send.Subject, _Configuration.GetValue<string>("MailJetConfig:Subject"))
               .Property(Send.HtmlPart, htmlCode)
               .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
                   });
            MailjetResponse response = await client.PostAsync(request);

            return response;
        }
    }
}
