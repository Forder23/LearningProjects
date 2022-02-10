using Mailjet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Utilities
{
    public interface IEmailSenderService
    {
        Task<MailjetResponse> SendMail(string email,string htmlCode,string subject=null);
    }
}
