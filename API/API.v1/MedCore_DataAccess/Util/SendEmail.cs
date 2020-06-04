using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Util
{
    public class SendEmail
    {
        String Host;
        Int32 Port;
        String Login;
        String Password;
        Boolean EnableSsl;
        String mailFrom;
        List<String> mailTo;
        String mailSubject;
        String mailBody;
        Boolean IsBodyHtml;
        List<Attachment> attachments;

        public SendEmail(List<String> mailTo, String mailSubject, String mailBody, String mailFrom = "", String nomeMailFrom = "")
        {
            var ctx = new DesenvContext();

            var smtpconfigs = (from s in ctx.tblSmtpConfig where s.smtpActive select s).ToList();

            var smtpconfig = smtpconfigs.Any(x => x.smtpFrom == mailFrom) ? smtpconfigs.FirstOrDefault(x => x.smtpFrom == mailFrom) : smtpconfigs.FirstOrDefault();

            Host = smtpconfig.smtpHost;
            Port = smtpconfig.smtpPort;

            Login = smtpconfig.smtpUsername;
            Password = smtpconfig.smtpPassword;

            EnableSsl = (Port == 587);

            mailFrom = (mailFrom == "" ? smtpconfig.smtpFrom : mailFrom);

            var toAddress = new MailAddress(mailFrom, nomeMailFrom);

            this.mailFrom = mailFrom == "" ? smtpconfig.smtpFrom : nomeMailFrom == "" ? mailFrom : toAddress.ToString();
            this.mailTo = mailTo;
            this.mailSubject = mailSubject;
            this.mailBody = mailBody;

            this.IsBodyHtml = true;
        }

        public String Send()
        {
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = this.Host;
            smtp.Port = this.Port;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            if (!string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.Password))
            {
                smtp.Credentials = new System.Net.NetworkCredential(this.Login, this.Password);
            }
            smtp.EnableSsl = Convert.ToBoolean(this.EnableSsl);


            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
            mailMsg.From = new System.Net.Mail.MailAddress(this.mailFrom);
            foreach (String email in this.mailTo)
                mailMsg.To.Add(email);

            mailMsg.Subject = this.mailSubject;
            mailMsg.Body = this.mailBody;

            mailMsg.Priority = System.Net.Mail.MailPriority.Normal;
            mailMsg.IsBodyHtml = this.IsBodyHtml;
            mailMsg.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            if(this.attachments != null && this.attachments.Any())
            {
                this.attachments.ForEach(a => mailMsg.Attachments.Add(a));
            }

            try
            {
                smtp.Send(mailMsg);
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                mailMsg.Dispose();
            }

            return "";
        }

        public void AddAttachment(Attachment item)
        {
            if(this.attachments == null)
            {
                this.attachments = new List<Attachment>();
            }
            this.attachments.Add(item);
        }
    }
}