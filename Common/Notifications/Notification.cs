/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace QuantConnect.Notifications
{
    /// <summary>
    /// Local/desktop implementation of messaging system for Lean Engine.
    /// </summary>
    public abstract class Notification
    {
        public string From = "francis.gauthier.3@ens.etsmtl.ca";
        /// <summary>
        /// Method for sending implementations of notification object types.
        /// </summary>
        /// <remarks>SMS, Email and Web are all handled by the QC Messaging Handler. To implement your own notification type implement it here.</remarks>
        public abstract void Send();
    }

    /// <summary>
    /// Web Notification Class
    /// </summary>
    public class NotificationWeb : Notification
    {
        /// <summary>
        /// Send a notification message to this web address
        /// </summary>
        public string Address;

        /// <summary>
        /// Object data to send.
        /// </summary>
        public object Data;

        /// <summary>
        /// Constructor for sending a notification SMS to a specified phone number
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data"></param>
        public NotificationWeb(string address, object data = null)
        {
            Address = address;
            Data = data;
        }

        public override void Send()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Sms Notification Class
    /// </summary>
    public class NotificationSms : Notification
    {
        /// <summary>
        /// Send a notification message to this phone number
        /// </summary>
        public string PhoneNumber;

        /// <summary>
        /// Message to send. Limited to 160 characters
        /// </summary>
        public string Message;

        /// <summary>
        /// Constructor for sending a notification SMS to a specified phone number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        public NotificationSms(string number, string message)
        {
            PhoneNumber = number;
            Message = message;
        }

        public override void Send()
        {
            
            MailMessage mail = new MailMessage(From, String.Concat(this.PhoneNumber, "@pcs.rogers.com"));
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "etscourriel.etsmtl.ca";
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ah96720@ens.etsmtl.ca", "");
            mail.Body = this.Message;
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            client.Send(mail);

        }
    }


    /// <summary>
    /// Email notification data.
    /// </summary>
    public class NotificationEmail : Notification
    {
        /// <summary>
        /// Send to address:
        /// </summary>
        public string Address;

        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject;

        /// <summary>
        /// Message to send.
        /// </summary>
        public string Message;

        /// <summary>
        /// Email Data
        /// </summary>
        public string Data;

        /// <summary>
        /// Default constructor for sending an email notification
        /// </summary>
        /// <param name="address">Address to send to</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="message">Message body of the email</param>
        /// <param name="data">Data to attach to the email</param>
        public NotificationEmail(string address, string subject, string message, string data)
        {
            Message = message;
            Data = data;
            Subject = subject;
            Address = address;
        }

        public override void Send()
        {
            MailMessage mail = new MailMessage(From, this.Address);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "etscourriel.etsmtl.ca";
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ah96720@ens.etsmtl.ca", "");
            mail.Subject = this.Subject;
            mail.Body = this.Message;
            ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };
            client.Send(mail);
        }
    }
}
