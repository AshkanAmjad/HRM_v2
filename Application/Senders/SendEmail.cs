using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Application.Senders
{
    //تابع ارسال ایمیل برای فراموشی رمز عبور
    public class SendEmail
    {
        public static bool Send(string to, string subject, string body, out string message)
        {
            string checkMessage = "عملیات ارسال با شکست مواجه شد.";
            bool result = false;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("noreply.hrmsystemv2@gmail.com", "سامانه مدیریت منابع انسانی");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                //System.Net.Mail.Attachment attachment;
                // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
                // mail.Attachments.Add(attachment);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply.hrmsystemv2@gmail.com", "bpgvrbwnkbwhmkrk");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                result = true;
                checkMessage = "کد تایید، با موفقیت ارسال شد.";
            }
            catch (Exception ex)
            {
                result = false;
            }

            message = checkMessage;
            return result;
        }
    }
}