using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace StandardUtilities
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
   public class EmailSender
    {
        /// <summary>
        /// Send Email.
        /// </summary>
        /// <returns></returns>
       public static bool SendEmail(string smtpServer, int smtpPort, string mailFrom, string mailToListSeparatedByComma, string mailCCListSeparatedByComma, string mailSubject, string mailBody, string attachment, bool enableSsl)
       {
           bool isMailSentStatus = false;
           Attachment mailattachment = null;
           SmtpClient smtpClient = null;
           MailMessage mail = null;
           try
           {
               smtpClient = new SmtpClient(smtpServer);               
               mail = new MailMessage();
               mail.From = new MailAddress(mailFrom);
               mail.To.Add(mailToListSeparatedByComma);
               if (mailCCListSeparatedByComma != null && !mailCCListSeparatedByComma.Trim().Equals("", StringComparison.OrdinalIgnoreCase))
               {
                   mail.CC.Add(mailCCListSeparatedByComma);
               }
               
               mail.Subject = mailSubject;
               mail.Body = mailBody;
               mail.IsBodyHtml = true;               
               if(attachment != null)
               {
                   mailattachment = new Attachment(attachment);
                   mail.Attachments.Add(mailattachment);
               }

               smtpClient.Port = smtpPort;
               smtpClient.EnableSsl = enableSsl;              
               smtpClient.Credentials = new NetworkCredential("debasish.gallop@gmail.com", "temp@1234");           
               smtpClient.Send(mail);
               isMailSentStatus = true;

           }
           catch (SmtpException)
           {
               try
               {
                   mail.Attachments.Remove(mailattachment);
                   mail.Body = mail.Body + "<br>MailAttachment Was Removed  Because Of Exceeeding Max Size Allowed By SMTP</br>";
                   smtpClient.Send(mail);
                   isMailSentStatus = true;
               }
               catch (Exception ex1)
               {

                   isMailSentStatus = false;
                   throw new Exception(String.Format("Exception Encountered while sending mail: Detailed Message - {0}", ex1.Message));

               }
           }
           catch (Exception ex)
           {

               isMailSentStatus = false;
               throw new Exception(String.Format("Exception Encountered while sending mail: Detailed Message - {0}", ex.Message));

           }
           finally
           {
               if (mailattachment != null)
               {
                   mailattachment.Dispose();
               }
           }
           return isMailSentStatus;

       }

    }
}
