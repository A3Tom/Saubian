using System;
using MimeKit;

namespace Saubian.Domain.ViewModels
{
    public class MessageDetail
    {
        public MessageDetail(MimeMessage mimeMessage)
        {
            Subject = mimeMessage.Subject;
            Sender = mimeMessage?.Sender?.Address;
            Body = CleanseStringOfFormatting(mimeMessage.TextBody);
            SentAt = mimeMessage.Date;
        }

        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public DateTimeOffset SentAt { get; private set; }
        public bool Cleansed { get; set; }
        public bool SanityChecked { get; set; }

        public string CleanseStringOfFormatting(string targetString)
        {
            Cleansed = true;

            return !string.IsNullOrEmpty(targetString) ? 
                targetString.Replace(Environment.NewLine, string.Empty) :
                null;
        }
    }
}
