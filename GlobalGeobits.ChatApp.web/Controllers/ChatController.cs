using GlobalGeobits.ChatApp.web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GlobalGeobits.ChatApp.web.Controllers
{
    public class ChatController : Controller
    {
        #region Data
        // GET: Chat
        private DBManager DBManager = new DBManager();

        #endregion

        #region Index Action
        public ActionResult Index( int? id)
        {
            

            int userid = 0;
            if (Session["user_id"] == null)
            {
                return RedirectToAction("login", "Account");
            }
            else
            {
                CombiendModel myModel = new CombiendModel();
                myModel.Users = null;
                myModel.Conversation = null;
                try
                {
                    userid = int.Parse(Session["user_id"].ToString());
                    var user = DBManager.GetUserBy(userid);
                    if (user == null)
                        return RedirectToAction("login", "Account");
                    else
                    {
                        FillViewBage(user);
                        List <Users> allUsersExceptCurrent = new List<Users>();
                        allUsersExceptCurrent = DBManager.GetUsersExcept(userid);
                        myModel.Users = allUsersExceptCurrent;
                        if (id != null)
                        {

                            var chatwithuser = DBManager.GetUserBy(id);
                            if (chatwithuser != null)
                            {
                                FillFrindViewBage(chatwithuser);
                                List<Messages> conv = Conversation(id);
                                myModel.Conversation = conv;
                            }
                        }
                        else
                        {
                            myModel.Conversation = null;
                        }

                        
                        return View(myModel);
                    }

                }
                catch (Exception) {
                 
                }

            }
            
            return View();
        }
        #endregion

        #region Methodes Via Ajax

        [HttpPost]
        public string SaveMessage(Messages Message)
        {
            if (Session["user_id"] == null)
                return "no current user";
            if (ModelState.IsValid)
            {
                int currentUserId = int.Parse(Session["user_id"].ToString());
                Message.MessageSenderID = currentUserId;
                Message.MessageSentDateTime = DateTime.Now;
                DBManager.AddNewMessage(Message);
                DateTime date = Message.MessageSentDateTime;
                   // ViewBag.SentMessageDate = "Today at "+ String.Format("{0:t}", date);
                return "Today at " + String.Format("{0:t}", date);
            }
            else

            return "not valid";
        }
        #endregion

        #region Help Methods

        private List<Messages> Conversation(int? frindid)
        {
            if (frindid == null) return null;
            List<Messages> conv = new List<Messages>();
            if (Session["user_id"] == null)
                return null;
            else
            {
                int currentid = int.Parse(Session["user_id"].ToString());

                conv = DBManager.GetConversationBtween(currentid, frindid);
            }
                return conv;
        }
        public  int checkUserAvaliablity(string userId) {

            int userid = int.Parse(userId);
            var user = DBManager.GetUserBy(userid);
            if (user == null) return 3;
            return user.Status;
        }

        private void FillFrindViewBage(Users chatwithuser)
        {
            ViewBag.ChatWithName = chatwithuser.UserDisplayName;
            ViewBag.userid = chatwithuser.UserID;
            ViewBag.chatWithGender = chatwithuser.UserGender.ToString();
            ViewBag.Status = (chatwithuser.Status == 1) ? "Online" : "Ofline";

        }

        private void FillViewBage(Users user)
        {
            ViewBag.username = user.UserFristName;
            ViewBag.currntuser_id = user.UserID;
            ViewBag.currentUserDisplayName = user.UserDisplayName;
            ViewBag.currntuser_gender = user.UserGender.ToString();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DBManager.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}