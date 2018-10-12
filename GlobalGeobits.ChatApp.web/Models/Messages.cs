using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GlobalGeobits.ChatApp.web.Models
{
    public class Messages
    {

        public int ID { get; set; }

        public string MessageContent { get; set; }

        public int MessageSenderID { get; set; }

        public int MessageReceiverID { get; set; }

        public DateTime MessageSentDateTime { get; set; }


    }
}