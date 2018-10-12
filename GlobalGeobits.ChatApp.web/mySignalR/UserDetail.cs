namespace GlobalGeobits.ChatApp.web.SignalR
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string Gender { get; internal set; }
    }
}