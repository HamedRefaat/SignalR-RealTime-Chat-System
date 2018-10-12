using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlobalGeobits.ChatApp.web.Models;

namespace GlobalGeobits.ChatApp.web.Controllers
{
    public class AccountController : Controller
    {
        #region Data

        private DBManager DBManager = new DBManager();

        #endregion

        #region Registration

        // GET: Users/Register
        public ActionResult Register()
        {
            if (Session["user_id"] != null) {
                //already login
                return RedirectToAction("index", "chat");
            }
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserID,UserFristName,UserLastName,UserDisplayName,UserGender,UserDateOfBirth,UserEmail,UserPassword,UserConfirmPassword,Status")] Users Account)
        {
            try
            {
                if (DBManager.GetUserBy(Account.UserEmail)!= null)
                {
                    ViewBag.error = "Email Already Existes";
                    return View();
                }

                if (ModelState.IsValid)
                {

                    if (Account.UserDisplayName == null || Account.UserDisplayName.Trim().Length<1)
                        Account.UserDisplayName = Account.UserFristName + " " + Account.UserLastName;
                    DBManager.AddNewUser(Account);
                    return RedirectToAction("Thanks", new { id = Account.UserDisplayName});
                }
                else
                {
                    ModelState.AddModelError("validationerror", "invalid information");
                }
            }
            catch (Exception ex) {
                ModelState.AddModelError("exp", ex);
            }
            return View(Account);
        }

        #endregion

        #region User Profile
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = DBManager.GetUserBy(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.currntuser_gender = users.UserGender.ToString();
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserFristName,UserLastName,UserDisplayName,UserGender,UserDateOfBirth,UserEmail,UserPassword,UserConfirmPassword,Status")] Users users)
        {
            if (ModelState.IsValid)
            {
                DBManager.UpdateUser(users);
                return RedirectToAction("Index");
            }
            ViewBag.currntuser_gender = users.UserGender.ToString();
            return View(users);
        }

        #endregion

        #region User Authentication
        public ActionResult Login()
        {
            if (Session["user_id"] != null)
            {
                
                    //already login
               return RedirectToAction("index", "chat");
            }
            var cookie = Request.Cookies.Get("userinfo");
            if (cookie != null)
            {
                string mail = cookie["user_mail"].ToString();
                string pass = cookie["user_pass"].ToString();
                var user = DBManager.IsUser(mail, pass);
                if (user != null)
                {
                    CreateSesstion(user);
                    return RedirectToAction("index", "chat");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserEmail,UserPassword")] Users Account)
        {
            
            string mail = Account.UserEmail;
            string pass = Account.UserPassword;

            var user = DBManager.IsUser(mail, pass);
            
            if (user == null)
            {
                ViewBag.error = "ieop";
                return View();
            }
            else {

                CreateSesstion(user);
                var rember = Request.Form["remberme"];
                if (rember == "on") {

                    CreateCookie(user.UserEmail, user.UserPassword);
                }
                return RedirectToAction("index", "Chat");

            }

        }

        #endregion

        #region Help Messages

        private void CreateSesstion(Users user)
        {
            Session["user_id"] = user.UserID.ToString();
            Session["user_displyname"] = user.UserDisplayName;
            DBManager.SetUserStatus(user.UserID, 1);
        }

        private void CreateCookie(string mail, string pass)
        {
            HttpCookie cookie = new HttpCookie("userinfo");
            cookie["user_mail"] = mail;
            cookie["user_pass"] = pass;
            cookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(cookie);
        }

        public ActionResult LogOut()
        {
            if (Session["user_id"] != null)
            {
                int userid = int.Parse(Session["user_id"].ToString());
                var user = DBManager.GetUserBy(userid);
                if (user != null)
                {
                    //user is offline
                    DBManager.SetUserStatus(userid, 0);
                    DestroySesstionAndcooki();
                }
            }

            return RedirectToAction("login", "Account");
        }

        private void DestroySesstionAndcooki()
        {
            Session.Abandon();
            Session["user_id"] = null;
            Session["user_displyname"] = null;
            var cookie = Request.Cookies.Get("userinfo");
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Response.Cookies.Add(cookie);

            }
        }

        public ActionResult Thanks(string id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public string AlredyExiestes(string Email) {

            var user = DBManager.GetUserBy(Email);

            if (user == null)
                return "";
            return "This Email Already Exists Please Go To Login Page ";


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
