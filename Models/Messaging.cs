using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class MessagingModel
    {
        public int FirstRecord { get; set; }
        public int MaxRecords { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MessageKey { get; set; }
    }
}