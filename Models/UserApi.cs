using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class UserApiModel
    {
        public AuthenticationModel Authentication { get; set; }
        public ContactModel Contacts { get; set; }
        public DiscussionModel Discussions { get; set; }
        public FriendModel Friends { get; set; }
        public MessagingModel Messaging { get; set; }
    }
}