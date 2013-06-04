using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class DiscussionModel
    {
        public string DiscussionKey { get; set;  }
        public string DiscussionPostKey { get; set; }
        public int MaxContentLength { get; set; }
        public string DiscussionKeyFilter { get; set; }
        public int MaxNumberToRetrieve { get; set; }
        public int MaxSubjectLength { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CrossPostKey { get; set; }
        public string SearchString { get; set; }
        public string PostedSince { get; set; }
        public string PostedBefore { get; set; }
        public string Author { get; set; }

    }
}