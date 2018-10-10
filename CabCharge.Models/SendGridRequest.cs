namespace CabCharge.Models
{
    public class SendGridRequest
    {
        public Personalization[] Personalizations { get; set; }

        public EmailPro From { get; set; }

        public string Subject { get; set; }

        public Content[] Content { get; set; }
    }

    public class Personalization
    {
        public EmailPro[] To { get; set; }
    }

    public class EmailPro
    {
        public string Email { get; set; }
    }

    public class Content
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
