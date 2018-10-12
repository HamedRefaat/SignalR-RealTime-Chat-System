using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Mvc;

namespace GlobalGeobits.ChatApp.web.SignalR.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {

        

        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
      

     

        public void Connect(string userid, string userDisplayName, string Gender)
        {
            var id = Context.ConnectionId;
            var user = ConnectedUsers.FirstOrDefault(u => u.UserId == userid);

            
                if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
                {
                    ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserId = userid, UserDisplayName= userDisplayName, Gender = Gender });
                    

                    // send to all except caller client
                    Clients.AllExcept(id).onNewUserConnected(userid, userDisplayName);
                // send connected now to caller
                foreach (var u in ConnectedUsers) {
                    if(u.ConnectionId != Context.ConnectionId)
                    Clients.Caller.sendonlinuser(u.UserId);
                        
                        }
                   
            }
            

        }

        public void SendMessageToAll(int userid, string message)
        {
            // store last 100 messages in cache
         //   AddMessageinCache(userid, message);

            // Broad cast message
            Clients.All.messageReceived(userid, message);
        }

        public void SendPrivateMessage(string toUserId, string message, string gender)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.UserId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUser.ConnectionId).sendPrivateMessage(fromUser.UserDisplayName, fromUser.UserId,fromUser.Gender, message);

                // send to caller user
              //  Clients.Caller.sendPrivateMessage(toUserId, toUser.UserId,toUser.Gender, message);
            }

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled )
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            
                if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserId, item.UserDisplayName);

            }

            return base.OnDisconnected(stopCalled);
        }


    }


}
