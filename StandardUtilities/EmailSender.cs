using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using TCMFactory;

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

       public static string CreateHtmlBodyForMail(IList<TestCase> listTestCases)
       {
           string mailbody = null;
           try
           {
               //string totalExecutionTimeInMins = Convert.ToString(reporter.GetTestExecutionTimeInMins());
               int passedTestcases = TestExecutionManagement.GetTotalPassTestCases(listTestCases);
               int failedTestcases = TestExecutionManagement.GetTotalFailedTestCases(listTestCases);
               int executedTestcases = passedTestcases + failedTestcases;
               
               //to get all failed test cases details
               IList<TestCase> listFailedTestCases = TestExecutionManagement.GetFailedTestCases(listTestCases);

               StringBuilder myBuilder = new StringBuilder();

               //Table For Failed Test Case Details
               myBuilder.AppendFormat("<table border='1'; style='color: Black;width ='1000'; bgcolor='#b3cccc'><col width=\"70%\"><col width=\"30%\"> <tr><th bgcolor=\"#75a3a3\" colspan=\"2\">" + "Automation Execution Result" + "</th></tr><tr><td>Total Test Cases Executed : </td><td style=\"text-align:center\">" + executedTestcases + "</td></tr><tr><td>Total Test Cases Passed : </td><td style=\"text-align:center\">" + Convert.ToString(passedTestcases) + "</td></tr><tr><td>Total Test Cases Failed : </td><td style=\"text-align:center\">" + Convert.ToString(failedTestcases) + "</td></tr></table>");
               if (listFailedTestCases.Count > 0)
               {

                   myBuilder.AppendFormat("<br />");
                   myBuilder.AppendFormat("<br />");


                   /*Table for failed Test Cases Summary*/
                   myBuilder.Append("<table border='1'; style='color: Black;width ='2000'; bgcolor='#b3cccc'>");
                   myBuilder.Append("<col width=\"15%\"><col width=\"15%\"><col width=\"15%\"><col width=\"15%\"><col width=\"40%\">");
                   /*Table header*/
                   myBuilder.Append("<tr><th bgcolor=\"#75a3a3\" colspan=\"5\">Failed Test Cases Summary</th></tr>");

                   /*Table Column*/
                   myBuilder.Append("<tr><th>Sr.No</th><th>Test Case Name</th><th>Test Case Description </th><th>Status</th><th>Status Description</th></tr>");

                   /*Row for failed Test case*/
                   int counter = 0;
                   foreach (TestCase testcase in listFailedTestCases)
                   {
                      
                       myBuilder.Append("<tr>");
                                                                   
                       /*Sr.No*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(String.Format("{0}", ++ counter));
                       myBuilder.Append("</td>");

                       /*TestCase Name*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestCaseName);
                       myBuilder.Append("</td>");

                       /*TestCase Description*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestCaseDescription);
                       myBuilder.Append("</td>");

                       /*TestCase Status*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.convertTestExecutionStatusToString());
                       myBuilder.Append("</td>");

                       /*TestCase Status Description */
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestExecutionResultMsg);
                       myBuilder.Append("</td>");

                       myBuilder.Append("</tr>");
                       /*rows ends for failed test case*/
                   }

                   myBuilder.Append("</table>");
               }

               //Table For Passed Test Case Details
               //get all passed test case
               IList<TestCase> listPassedTestCases = TestExecutionManagement.GetPassedTestCases(listTestCases);
               if (listPassedTestCases.Count > 0)
               {

                   myBuilder.AppendFormat("<br />");
                  
                   /*Table for failed Test Cases Summary*/
                   myBuilder.Append("<table border='1'; style='color: Black;width ='2000'; bgcolor='#b3cccc'>");
                   myBuilder.Append("<col width=\"15%\"><col width=\"15%\"><col width=\"15%\"><col width=\"15%\"><col width=\"40%\">");
                   /*Table header*/
                   myBuilder.Append("<tr><th bgcolor=\"#75a3a3\" colspan=\"5\">Passed Test Cases Summary</th></tr>");

                   /*Table Column*/
                   myBuilder.Append("<tr><th>Sr.No</th><th>Test Case Name</th><th>Test Case Description </th><th>Status</th><th>Status Description</th></tr>");

                   /*Row for failed Test case*/
                   int counter = 0;
                   foreach (TestCase testcase in listPassedTestCases)
                   {

                       myBuilder.Append("<tr>");

                       /*Sr.No*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(String.Format("{0}", ++counter));
                       myBuilder.Append("</td>");

                       /*TestCase Name*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestCaseName);
                       myBuilder.Append("</td>");

                       /*TestCase Description*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestCaseDescription);
                       myBuilder.Append("</td>");

                       /*TestCase Status*/
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.convertTestExecutionStatusToString());
                       myBuilder.Append("</td>");

                       /*TestCase Status Description */
                       myBuilder.Append("<td style=\"text-align:center\">");
                       myBuilder.Append(testcase.TestExecutionResultMsg);
                       myBuilder.Append("</td>");

                       myBuilder.Append("</tr>");
                       /*rows ends for failed test case*/
                   }

                   myBuilder.Append("</table>");
               }

               //form mailBody from myBuilder
               mailbody = myBuilder.ToString();
           }
           catch (Exception ex)
           {
              Console.WriteLine(String.Format("Mail Body Could Not Be Formed Due To Error - ", ex.Message));
           }

           return mailbody.ToString();
       }

    }
}
