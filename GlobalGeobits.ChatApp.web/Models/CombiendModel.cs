using System.Collections.Generic;


namespace GlobalGeobits.ChatApp.web.Models
{
    public class CombiendModel
    {
        public List<Users> Users { get; set; }

        public List<Messages> Conversation { get; set; }
    }
}