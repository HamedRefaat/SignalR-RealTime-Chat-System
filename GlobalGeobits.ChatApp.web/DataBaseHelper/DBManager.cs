using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GlobalGeobits.ChatApp.web.Models
{
    
    public class DBManager :IDisposable
    {
        private ChatAppDbContext db;

        public DBManager()
        {
            db = new ChatAppDbContext();
        }

         ~DBManager() {

            Dispose();
        }

        #region User Methods
        public Users GetUserBy(int? id) {
            try
            {
                var user = db.Users.Find(id);
                return user;
            }
            catch (Exception Ex) {
                throw new Exception("Ops somthing got Wrong With DataBase", Ex);
               
            }

            
        }

        public Users GetUserBy(string Emaile)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.UserEmail == Emaile);

                return user;
            }
            catch (Exception Ex)
            {
                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }

        }

        public Users IsUser(string Email, string Password) {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.UserEmail == Email && u.UserPassword == Password);
                return user;
            }
            catch (Exception Ex)
            {
                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }

        }


        public bool AddNewUser(Users user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception) {
                return false;
            }
            return true;  
        }

        public bool SetUserStatus(int UserId, int status) {
            var user = GetUserBy(UserId);
            if (user == null) return false;


            user.Status = status;

            try
            {
                db.SaveChanges();
            }
            catch (Exception Ex)
            {
                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }


            return true;
        }

        public bool UpdateUser(Users user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception Ex) {

                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }

        }

        public List<Users> GetUsersExcept(int user_id) {

            try
            {
                var _allusers = db.Users.Where(u => u.UserID != user_id).ToList();
                return _allusers;
            }
            catch (Exception Ex)
            {

                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }

        }

        #endregion

        #region Messages Methodes

        public bool AddNewMessage(Messages message)
        {
            try
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {

                throw new Exception("Ops somthing got Wrong With DataBase", Ex);

            }

        }

        public List<Messages> GetConversationBtween(int currentid, int? frindid) {

            List<Messages> fromMeToFriend = db.Messages.Where(m => m.MessageSenderID == currentid && m.MessageReceiverID == frindid).ToList();
            List<Messages> fromFriendToMe = db.Messages.Where(m => m.MessageSenderID == frindid && m.MessageReceiverID == currentid).ToList();

            List<Messages> all = new List<Messages>();
            if (fromMeToFriend != null)
                all.AddRange(fromMeToFriend);
            if (fromFriendToMe != null)
                all.AddRange(fromFriendToMe);

            if (all.Count == 0)
                return null;


        return all.OrderBy(m => m.MessageSentDateTime).ToList();



        }

        #endregion

        public void Dispose() {
            db.Dispose();
        }
    }
}