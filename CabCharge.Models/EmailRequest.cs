using System;

namespace CabCharge.Models
{
    public class EmailRequest
    {
        public string From { get; set; }

        public string Tos { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
