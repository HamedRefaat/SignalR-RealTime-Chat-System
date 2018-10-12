# SignalR-RealTime-Chat-System

* ChatApp that have:
* User Registration page.
* user Login page.
* User Home page (Chat DashBoard).
* start a real time chat among users.
* send a private message to user even if user is offline
* Save All Conversations among users


## Used Technologies
* [ASP MVC 5](https://docs.microsoft.com/en-us/previous-versions/aspnet/dd381412(v=vs.108))
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
* [SQL database](https://docs.microsoft.com/en-us/sql/sql-server/sql-server-technical-documentation?view=sql-server-2017)
* [SignalR 2](http://signalr.net/)
* [Entity framework 6 CodeFirst](https://msdn.microsoft.com/en-us/magazine/dn519921.aspx)

### Prerequisites

* .Net FrameWork 4.2
* VisualStudio 2017

## Models Classes

Business Models Clases that hold all requried classes and its proberties

## User Model class
user class that hold all User proberites and validation on each proberity

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalGeobits.ChatApp.web.Models
{

    public enum Gender { 
    Mael, Femael
    }

    public class Users
    {
        [Key]
        public int UserID { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
          ErrorMessageResourceName = "Validation_reqired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_maxlength")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_fn")]
        public string UserFristName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_maxlength")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_ln")]
        public string UserLastName { get; set; }


        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_dn")]
        [RegularExpression(@"^[a-zA-Z0-9_]{1,3}[a-zA-Z0-9 @#$-]*$",ErrorMessage ="Writ a name or keep blank")]
        public string UserDisplayName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
          ErrorMessageResourceName = "Validation_reqired")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_gender")]
        public Gender UserGender { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_dob")]
        [DataType(DataType.Date)]
        [Plus18(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_birthdate")]
        public DateTime UserDateOfBirth { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_reqired")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_invalid_mail")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_mail")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_reqired")]
        [RegularExpression(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})$", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_invalid_pass")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_pass")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "Validation_reqired")]
        [Compare("UserPassword", ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Validation_Confirmpass")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "lbl_confirm_pass")]
        [DataType(DataType.Password)]
        public string UserConfirmPassword { get; set; }

        public int Status { get; set; }

        // the list of all sent messages by a spasific account
        public virtual ICollection<Messages> SentMessages { get; set; }

        // the list of all received messages by a spasific account
        public virtual ICollection<Messages> ReceivedMessages { get; set; }


    }

    /// <summary>
    /// a custom validation attribute that validate that account age is plus 18 years
    /// </summary>
 
    public  class Plus18 : ValidationAttribute
    {

        /// <summary>
        /// overrided IsValid method that check the validation 
        /// So, if account age is plus 18 so it valid and return true
        /// else return false is no Date of Birhth is enterd or age is less than 18
        /// </summary>
        /// <param name="value"> Value is the Date of Birth of the account </param>
        /// <returns>
        /// the Isvalid method return True is account age is equal or plus 18 Years,
        /// return false otherwise
        /// 
        /// </returns>
        public override bool IsValid(object value)
        {

            var DOB = value as DateTime?;

            if (DOB == null) return false;
            if (DOB != null && DOB.HasValue) {

                int ageYears = DateTime.Now.Year - DOB.Value.Year;

                if (ageYears >= 18)
                    return true;
            
            }
            return false;
        }
    }
}


```
## Messages Model Class
Once we use the EntityFrameWork 6 (CodeFirst Approch), we must declare the messages tables 
```csharp
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
```
## DB Context class
 A DbContext instance represents a combination of the Unit Of Work and Repository patterns such that it can be used to query from a database and group together changes that will then be written back to the store as a unit. DbContext is conceptually similar to ObjectContext.
 ```csharp
using System.Data.Entity;

namespace GlobalGeobits.ChatApp.web.Models
{
    public class ChatAppDbContext : DbContext
    {

        public ChatAppDbContext(): base("GlobalGeobitsDB")
        {

        }
        //map the users model class to SQL Database table
        public DbSet<Users> Users { get; set; }

        //map the users model class to SQL Database table
        public DbSet<Messages> Messages { get; set; }
    }
}
```
## DB Combiend Model class
The Combiend Model Class is just a class that holdes info about users and messages together, to pass it to the view
```csharp
using System.Collections.Generic;


namespace GlobalGeobits.ChatApp.web.Models
{
    public class CombiendModel
    {
        public List<Users> Users { get; set; }

        public List<Messages> Conversation { get; set; }
    }
}
```

### GlobalResource
A resource file is an XML file that can contain strings and other resources, such as image file paths. Resource files are typically used to store user interface strings that must be translated into other languages. This is because you can create a separate resource file for each language into which you want to translate a Web page.

Global resource files are available to any page or component in your Web site. Local resource files are associated with a single Web page, user control, or master page, and contain the resources for only that page. For more information, [see ASP.NET Web Page Resources Overview](https://msdn.microsoft.com/en-us/library/ms227427.aspx).

In Visual Web Developer, you can use the managed resource editor to create global or local resource files. For local resource files, you can also generate a culturally neutral base resource file directly from a Web page in the designer.

## Adding Resource File 
* right clik on project solution
* Add -> ASP Folder
* App_GlobalResources
* Right clik on App_GlobalResources
* Add-> resource file

## Resource.designer.cs
```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Resource", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm Password.
        /// </summary>
        public static string lbl_confirm_pass {
            get {
                return ResourceManager.GetString("lbl_confirm_pass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Display Name.
        /// </summary>
        public static string lbl_dn
        {
            get {
                return ResourceManager.GetString("lbl_dn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Birth Date.
        /// </summary>
        public static string lbl_dob
        {
            get {
                return ResourceManager.GetString("lbl_dob", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Frist Name.
        /// </summary>
        public static string lbl_fn
        {
            get {
                return ResourceManager.GetString("lbl_fn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gender.
        /// </summary>
        public static string lbl_gender
        {
            get {
                return ResourceManager.GetString("lbl_gender", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last Name.
        /// </summary>
        public static string lbl_ln
        {
            get {
                return ResourceManager.GetString("lbl_ln", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email Adress.
        /// </summary>
        public static string lbl_mail
        {
            get {
                return ResourceManager.GetString("lbl_mail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PassWord.
        /// </summary>
        public static string lbl_pass
        {
            get {
                return ResourceManager.GetString("lbl_pass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your Age Must be equal or over 18 years.
        /// </summary>
        public static string Validation_birthdate
        {
            get {
                return ResourceManager.GetString("Validation_birthdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password Are Not Matched.
        /// </summary>
        public static string Validation_Confirmpass
        {
            get {
                return ResourceManager.GetString("Validation_Confirmpass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, Enter a valid mail &apos;yourName@Example.com&apos;.
        /// </summary>
        public static string Validation_invalid_mail
        {
            get {
                return ResourceManager.GetString("Validation_invalid_mail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The PassWord must contains at least One digit, One lowerCase character, One UpperCase character, one special symbols in the list &quot;@#$%&quot; and must be from 6 to 20 characters.
        /// </summary>
        public static string Validation_invalid_pass
        {
            get {
                return ResourceManager.GetString("Validation_invalid_pass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The max length of this filed is 100.
        /// </summary>
        public static string Validation_maxlength
        {
            get {
                return ResourceManager.GetString("Validation_maxlength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Field is Requried.
        /// </summary>
        public static string Validation_reqired
        {
            get {
                return ResourceManager.GetString("Validation_reqired", resourceCulture);
            }
        }
    }
}
```
### Data Base Helper Classes
DBManager is a class that holdes all comunications with databse, inserting, Deleting and updating
```csharp
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
```

 ### Controlers
 In this section, you will learn about the Controller in ASP.NET MVC.

The Controller in MVC architecture handles any incoming URL request. Controller is a class, derived from the base class System.Web.Mvc.Controller. Controller class contains public methods called Action methods. Controller and its action method handles incoming browser requests, retrieves necessary model data and returns appropriate responses.

In ASP.NET MVC, every controller class name must end with a word "Controller". For example, controller for home page must be HomeController and controller for student must be StudentController. Also, every controller class must be located in Controller folder of MVC folder structure.
 
 ## AccountController
  AccountCotroler is used to handel the user account ie Registration and Login
  ```csharp

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

  ```
  
  ## HomeControler
  Home Controler will used to handel the message exchanging and saving
  ```csharp
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
```

  ### Views
  View is the UI that prsent the data in a way that user can deal with
  
  ## Shared Views
  Views that shared and can be used in any view (Like Master Page in ASP.NET WebForms)
  * LayOut View
  ```html
  <!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Global Geobits Chat App</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
           
        </div>
    </div>
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - Hamed Refaat</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required: false)
    
</body>
</html>


  ```
  ## Account Views
  * Login View
  ```html
@model GlobalGeobits.ChatApp.web.Models.Users

@{
    ViewBag.Title = "Login";
}

<h2>Login</h2>


@using (Html.BeginForm()) 
{
    @Html.AntiForgeryToken()

<div id="loginform" class="form-horizontal">
   

    <h4>Login To Chat App</h4>
    <hr />

    @if (ViewBag.error == "ieop")
    {

        <h2 style="color:red">Invalid Email Or Password</h2>
    }

    <div class="form-group">
        @Html.LabelFor(model => model.UserEmail, htmlAttributes: new { @class = "control-label col-md-2" })
        <div class="col-md-10">
            @Html.EditorFor(model => model.UserEmail, new { htmlAttributes = new { @class = "form-control" } })
            @Html.ValidationMessageFor(model => model.UserEmail, "", new { @class = "text-danger" })
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(model => model.UserPassword, htmlAttributes: new { @class = "control-label col-md-2" })
        <div class="col-md-10">
            @Html.EditorFor(model => model.UserPassword, new { htmlAttributes = new { @class = "form-control" } })
        </div>
    </div>

    <div class="form-group">
        <label class="control-label col-md-2"> Remeber Me</label>
        <div class="col-md-10">
            <input type="checkbox" id="remberme" name="remberme" />
        </div>
    </div>

    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <input  type="submit" value="Login" class="btn btn-default" />
            <h3>Or @Html.ActionLink("Register here", "Register") for Free if you don't have an account</h3>
        </div>
    </div>
</div>
}


@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
  ```
  * Register View
  ```html
@model GlobalGeobits.ChatApp.web.Models.Users

@{
    ViewBag.Title = "Register New User";
}

<h2>Regester New User</h2>
@if (ViewBag.error == "eex")
{
    <h3 style="color: red">Email already exists</h3>
    <h4>Please, @Html.ActionLink("Login here", "login") </h4>
}
@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()

    <div class="form-horizontal">
        <h4>Regidter New User</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.UserFristName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserFristName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserFristName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserLastName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserLastName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserLastName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserDisplayName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserDisplayName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserDisplayName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserGender, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EnumDropDownListFor(model => model.UserGender, htmlAttributes: new { @class = "form-control" })
                @Html.ValidationMessageFor(model => model.UserGender, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserDateOfBirth, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserDateOfBirth, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserDateOfBirth, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserEmail, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserEmail, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserEmail, "", new { @class = "text-danger", @id = "valmal" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserPassword, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserPassword, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserPassword, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserConfirmPassword, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserConfirmPassword, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserConfirmPassword, "", new { @class = "text-danger" })
            </div>
        </div>



        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Register" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Already has an Account?", "login")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")


    <script src="~/Scripts/Registrationjs.js"></script>
}
  ```
  * Thanks after registration View
  ```html

@{
    ViewBag.Title = "Thanks";
}

<h1>Welcome @ViewBag.userName Thanks for Registration. Please, @Html.ActionLink("Login here","login")</h1>

```
#Edit View
```html
@model GlobalGeobits.ChatApp.web.Models.Users

@{
    ViewBag.Title = "Edit";
}

<h2>Edit</h2>


@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()
    
    <div class="form-horizontal">
        <h4>Users</h4>
        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        @Html.HiddenFor(model => model.UserID)

        <div class="form-group">
            @Html.LabelFor(model => model.UserFristName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserFristName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserFristName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserLastName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserLastName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserLastName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserDisplayName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserDisplayName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserDisplayName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserGender, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EnumDropDownListFor(model => model.UserGender, htmlAttributes: new { @class = "form-control" })
                @Html.ValidationMessageFor(model => model.UserGender, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserDateOfBirth, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserDateOfBirth, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserDateOfBirth, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserEmail, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserEmail, new { htmlAttributes = new { @class = "form-control" , @disabled = "disabled" } })
              
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserPassword, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserPassword, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserPassword, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.UserConfirmPassword, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.UserConfirmPassword, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.UserConfirmPassword, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.Status, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Status, new { htmlAttributes = new { @class = "form-control", @disabled = "disabled" } })
                @Html.ValidationMessageFor(model => model.Status, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Save" class="btn btn-default" />
            </div>
        </div>
    </div>
}

<div>
    @Html.ActionLink("Chat Room", "Index","Chat")
</div>

@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
}
```

## Home Views

* Index view
```html
@model  GlobalGeobits.ChatApp.web.Models.CombiendModel
@{
    ViewBag.Title = "Index";
}

<h2 id="chatpage">Chat Room</h2>
<h2>
    Wellcom @ViewBag.username to Chat App.
</h2>

<style>
    div.panel-body {
        overflow: scroll;
        background: url('http://subtlepatterns.com/patterns/geometry2.png');
    }
</style>

<div class="row">

    <input type="hidden" id="currentUserId" value=@ViewBag.currntuser_id />
    <input type="hidden" id="currentUserDisplayName" value=@ViewBag.currentUserDisplayName />
    <input type="hidden" id="currentUserGender" value=@ViewBag.currntuser_gender />

    <div class=" col-md-3">
        <div class="chat-panel panel panel-default">
            <div class="panel-heading">
                <h3>Friends</h3> 
                <i class="fa fa-paper-plane fa-fw"></i>
            </div>
            <div class="panel-body" style="height:380px;">
                <ul id= "userlist">
                    @if (Model.Users != null)
                    {
                        foreach (var user in Model.Users)
                        {
                            int uid = -1;

                            if (user != null)
                            {
                                uid = user.UserID;

                            }

                            <li id="@user.UserID-1" style="margin-left:-44px;">

                                <h3 id="userdisname" style="display:inline-block;">
                                    <a href="/chat/index/@user.UserID#chat">@user.UserDisplayName</a>
                                    

                                </h3>

                                    <span id="@user.UserID" style="color: gray; font-size : x-large">•</span>
  
                            </li>

                        }
                    }
                </ul>
            </div>
        </div>
    </div>

    <div class=" col-md-9">
        <div class="chat-panel panel panel-default">
            <div class="panel-heading">
                @if (ViewBag.ChatWithName != null)
                {

                    <input type="hidden" id="inChatuser" value=@ViewBag.userid />
                    <input type="hidden" id="inChatuserGender" value=@ViewBag.chatWithGender />

                    <h3 id="chattab"> @ViewBag.ChatWithName  is @ViewBag.Status  </h3>
                }



                <i class="fa fa-paper-plane fa-fw"></i>
            </div>
            <!-- /.panel-heading -->
            <div class="panel-body" id="chat" style="height:300px;">
                <input type="hidden" id="displayname" />





                @if (@ViewBag.ChatWithName != null && Model.Conversation != null)
    {
        int myfrindid = int.Parse(ViewBag.userid.ToString());

        foreach (var conv in Model.Conversation)
        {

            if (conv.MessageSenderID == myfrindid)
            {
                
                    <p id="friend" style="color:green; text-align:left;">
                        <div class="row">
                            <div class="col-4" style="min-width: 140px; max-width :140px;">

                                <img src="~/Content/Images/@ViewBag.chatWithGender-1.png" title="@ViewBag.ChatWithName" />




                            </div>
                            <div class="col-8">
                                <h3 style="margin-left: 20px;"> @ViewBag.ChatWithName </h3>

                                @{
                        string DateFormat = "";
                        if (conv.MessageSentDateTime.Day == DateTime.Now.Day)
                        {
                            DateFormat = "Today at " + String.Format("{0:t}", conv.MessageSentDateTime);
                        }
                        else if (conv.MessageSentDateTime.Day == DateTime.Now.Day - 1)
                        {
                            DateFormat = "YesterDay at " + String.Format("{0:t}", conv.MessageSentDateTime);
                        }
                        else
                        {

                            DateFormat = String.Format("{0:f}", conv.MessageSentDateTime);
                        }
                                }

                                <strong style="margin-left: 10px;">

                                    @DateFormat

                                </strong>

                            </div>

                        </div>
                        <div class="row">
                            <div class="col-12" style="margin-left: 20px;">
                                <strong style="color:green; ">

                                    @conv.MessageContent;

                                </strong>
                            </div>
                        </div>

                    </p>
                
}
else
{


    // my messages
           
                <p id="me" style="color:blue; text-align:left;">
                    <div class="row">
                        <div class="col-4" style="min-width: 140px; max-width :140px;">

                            <img src="~/Content/Images/@ViewBag.currntuser_gender-1.png" title="@ViewBag.username" />




                        </div>
                        <div class="col-8">
                            <h3 style="margin-left: 20px;"> @ViewBag.username </h3>

                            @{
                    string DateFormat = "";
                    if (conv.MessageSentDateTime.Day == DateTime.Now.Day)
                    {
                        DateFormat = "Today at " + String.Format("{0:t}", conv.MessageSentDateTime);
                    }
                    else if (conv.MessageSentDateTime.Day == DateTime.Now.Day - 1)
                    {
                        DateFormat = "YesterDay at " + String.Format("{0:t}", conv.MessageSentDateTime);
                    }
                    else
                    {

                        DateFormat = String.Format("{0:f}", conv.MessageSentDateTime);
                    }
                            }

                            <strong style="margin-left: 10px;">

                                @DateFormat

                            </strong>

                        </div>

                    </div>
                    <div class="row">
                        <div class="col-12" style="margin-left: 20px;">
                            <strong style="color:blue; ">

                                @conv.MessageContent;

                            </strong>
                        </div>
                    </div>

                </p>
          





}


}






}
else if (ViewBag.ChatWithName != null)
{

string text = "";

if (ViewBag.chatWithGender == "Mael")
{
text = "Him";
}
else
{
text = "Her";
}


            <h1 id="noconvers" style="font-family:Arial;  color: sandybrown; align-content:center;"> No Conversation between you and @ViewBag.ChatWithName Start Texting with @text Now :)  </h1>

}



            </div>
            <!-- /.panel-body -->



            <div class="panel-footer">
                @if (@ViewBag.ChatWithName != null)
                {
                    <div class="input-group">
                        <input id="message" type="text" name="message" class="form-control input-sm" placeholder="Type your message here..." />

                        <span class="input-group-btn">

                            <input type="button" class="btn btn-warning btn-sm" id="sendmessage" value="Send">

                        </span>
                    </div>
                }

            </div>
        </div>
    </div>
</div>

<audio id="notificationsound">
    <source src="~/Content/Soundes/plucky.mp3" type="audio/mpeg" />
    <source src="~/Content/Soundes/plucky.ogg" type="audio/ogg" />

</audio>



@section scripts {
    <!--Script references. -->
    <!--The jQuery library is required and is referenced by default in _Layout.cshtml. -->
    <!--Reference the SignalR library. -->

    <script src="~/Scripts/jquery-3.3.1.js"></script>
    <script src="//code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="~/Scripts/jquery.signalR-2.3.0.min.js"></script>
    <script src="/signalr/hubs"></script>

    <script src="~/Scripts/ChatHubManager.js"></script>
  

   

    <script>


$("#sendmessage").click(function () {




    var message = $("#message").val();
    var touser = $("#inChatuser").val();
    var gender = $("#inChatuserGender").val();
    $("#message").val("");
    if (message.replace(/\s/g, '').length) {

        chatHub.server.sendPrivateMessage(touser, message, gender);


        $.ajax(
            {
                url: '/Chat/SaveMessage',
                type: 'POST',
                data: { MessageContent: message, MessageReceiverID: @ViewBag.userid
    }
}).done(function (response) {


    $('#chat').append('<p id="me" style="color:blue; text-align:left;"><div class="row"><div class="col-4" style="min-width: 140px; max-width :140px;"><img src="/Content/Images/@ViewBag.currntuser_gender-1.png" title="@ViewBag.username" /></div><div class="col-8"><h3 style="margin-left: 20px;">@ViewBag.username </h3><strong style="margin-left: 10px;">' + response + '</strong></div></div><div class="row"><div class="col-12" style="margin-left: 20px;"><strong style="color:blue;  ">'
        + message + '</strong></div></div></p>');

    $('#chat').scrollTop($('#chat')[0].scrollHeight);
    $('#noconvers').hide();

    toastr.success("Message send");

}).fail(function (response) {

    toastr.error("Messege not Sent " + response);
});

            }
            else {
    toastr.error("Empety Message And Spaces are Not Allowed");
}


        });


    </script>
}
  ```
  ### SignalR
  ## hubs Class
  To create a Hub, create a class that derives from [Microsoft.Aspnet.Signalr.Hub](https://msdn.microsoft.com/library/microsoft.aspnet.signalr.hub(v=vs.111).aspx). The following example shows a simple Hub class for a chat application.
  ```csharp
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

  ```
 User Details for hub class
 ```csharp
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
 ```
 
 ## Connect To SignalR
 # Hub Manager on client
 a js file that register on the declared methods in the hub class
 
 ```js
// declare tha chathb 
var chatHub
$(document).ready(function () {
    //assign the chat hub to our chathub class
    chatHub = $.connection.chatHub;


    //subscripe on the onNewUserConnected event so when a user connect the hub the assigend function will fire
    // that's the use of the SignalR js proxy
    chatHub.client.onNewUserConnected = function (userid, Name) {
        // what to do when a new user connect the chat app ?
        //ofcourse notifiy all user that the use is online now so thy can start a real time chat with him
        toastr.success(Name + " Is online Now");

        //what happend if the recent connected user is a new user just registerd now. So he is not in the user list

        if ($('#' + userid + '-1').length)         // is user in the user list
        {
            // just change his status to make him online
          
            $("#" + userid + "").css("color", "green").css("font-size", "xx-large");
            $("#chattab").text(Name + " is Online Now");

        }
        else {
            // a new user register while other users are online so we have to append the user name to the ul
            toastr.success("Say Hello to " + Name + "who just registerd Now");

            var item = ' <li id="' + userid + '-1" style="margin-left:-44px;"> <h3 id = "' + Name + '"  style = "display:inline-block;" > <a href="/chat/index/' + userid + '#chat">' + Name + '</a></h3> <span id="' + userid + '" style="color: green; font-size : xx-large">•</span></li >';

            $("#userlist").append(item);

        }


    }
    // if user connect to the chat while there are othe user online, so when he connect
    // he received a callback message from server tell'm who is online now
    chatHub.client.sendonlinuser = function (userid) {
        $("#" + userid + "").css("color", "green").css("font-size", "xx-large");

        
    }
    // when user logout or close the browser
    // the server send a message to all connected to tell that a user is offline

    chatHub.client.onUserDisconnected = function (connectedid, userId, userDisplayName) {

        var st = "offline";
        toastr.warning(userDisplayName + " Is " + st);
        $("#" + userId + "").css("color", "gray").css("font-size", "xx-large");

        $("#chattab").text(userDisplayName + " is Online Offline");

    }

    // register on the sendprivatemessage method so when some one send a message to other one
    //the recevier knows that he got an new message from the sender
    chatHub.client.sendPrivateMessage = function (Name, userId, gender, message) {
        // we have 2 senarioes here
        //1- the recevier user already open the chat windwo the the sender so in this case we append the message in the reveiver chat window
        //2- the revever dosent open the sender user chat windw so just notify him that he got a private message


        var inchatuser = $("#inChatuser").val();

        var d = new Date();
        var UserInChat = $("#inChatuser").val();
       // if the receiver open the sender chat windwo, hust append the message
        if (UserInChat == userId) {
            var tt = "Am";
            var hr = d.getHours();
            if (hr > 12) { hr = hr - 12; tt = "PM" }
            var hrr = hr + "";
            
            if (hr < 10) {
                hrr = "0" + hr;
            }

            
            var min = d.getMinutes();
            var minn = min + "";
            if (min < 10) { minn = "0" + min }

            var currntdate = "Today at " + hrr + ":" + minn + " " + tt;

            var toappend = '<p id="friend" style="color:green; text-align:left;"><div class="row"><div class="col-4" style="min-width: 140px; max-width :140px;"><img src="/Content/Images/' + gender + '-1.png" title="' + Name + '"</div><div class="col-8"><h3 style="margin-left: 20px;"> ' + Name + ' </h3><strong style="margin-left: 10px;">' + currntdate + '</strong</div></div><div class="row"><div class="col-12" style="margin-left: 20px;"><strong style="color:green; ">' + message + '</strong></div></div></p>';
            $('#chat').append(toappend);
            $('#chat').scrollTop($('#chat')[0].scrollHeight);
            $('#noconvers').hide();

        }
        else {
            // the receiver open othe chat window or dosn't open any, so just notify him
            // that he got a private message from sender and play a notification sound
            // to let him know if he minimize the browser windw
            toastr.success("You received a new message from your friend " + Name);
            $("#" + userId + "").css("color", "red").css("font-size", "xx-large");

            $("#" + userId + "-1").effect("shake", { direction: "right", times: 10, distance: 5 }, 1000);

            $('#notificationsound')[0].play();
        }
    }


    // start the hub to enable clients to register on events
    $.connection.hub.start().done(function () {

        var userid = $("#currentUserId").val();
        //   alert("started");
        var userDisplayName = $("#currentUserDisplayName").val();
        var userGender = $("#currentUserGender").val();
        chatHub.server.connect(userid, userDisplayName, userGender);
    });

});
 ```
 
 ### DataBase Update
 I update the Users Model to add a status code that tells if the user is online or not 
 Here How we detect if user is online or not
 
 ## Editing Database in codeFrist Approch after database is created
 
To enable this we must add the follwing code int the Application class

```csharp

Database.SetInitializer<ChatAppDbContext>(new DropCreateDatabaseIfModelChanges<ChatAppDbContext>());

```
## AppStart method 
```csharp
using GlobalGeobits.ChatApp.web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GlobalGeobits.ChatApp.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<ChatAppDbContext>(new DropCreateDatabaseIfModelChanges<ChatAppDbContext>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}


```
## Errors and exceptions Log

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalGeobits.ChatApp.web.ErrorsLog
{
    public class  LogFilterExceptions : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null) return;

            File.AppendAllText(actionExecutedContext.HttpContext.Server.MapPath("~/Errors/logfilter.text"), "<EXP>" + Environment.NewLine + actionExecutedContext.Exception.ToString() + Environment.NewLine + "</EXP>" + Environment.NewLine);
        }
    }


}
```
## Aplication Configrations
```xml
<?xml version="1.0" encoding="utf-8"?>
<!--
  For more information on how to configure your ASP.NET application, please visit
  https://go.microsoft.com/fwlink/?LinkId=301880
  -->
<configuration>
 
  <configSections>
   
    
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    
  <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
   <connectionStrings>
   <add name="GlobalGeobitsDB" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|GlobalGeobitsDB.mdf;Integrated Security=True" providerName="System.Data.SqlClient" />  
  
  
    </connectionStrings>
    
    
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>
  <system.web>
    <compilation debug="true" targetFramework="4.6.1" />
    <httpRuntime targetFramework="4.6.1" />
    <httpModules>
      <add name="ApplicationInsightsWebTracking" type="Microsoft.ApplicationInsights.Web.ApplicationInsightsHttpModule, Microsoft.AI.Web" />
      <add name="TelemetryCorrelationHttpModule" type="Microsoft.AspNet.TelemetryCorrelation.TelemetryCorrelationHttpModule, Microsoft.AspNet.TelemetryCorrelation" />
    </httpModules>

   
    
  </system.web>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" culture="neutral" publicKeyToken="30ad4fe6b2a6aeed" />
        <bindingRedirect oldVersion="0.0.0.0-11.0.0.0" newVersion="11.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Optimization" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-1.1.0.0" newVersion="1.1.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="WebGrease" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="0.0.0.0-1.6.5135.21930" newVersion="1.6.5135.21930" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Helpers" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-3.0.0.0" newVersion="3.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.WebPages" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-3.0.0.0" newVersion="3.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" />
        <bindingRedirect oldVersion="1.0.0.0-5.2.6.0" newVersion="5.2.6.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Diagnostics.DiagnosticSource" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.3.1" newVersion="4.0.3.1" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Antlr3.Runtime" publicKeyToken="eb42632606e9261f" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-3.5.0.2" newVersion="3.5.0.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.AspNet.TelemetryCorrelation" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.0.4.0" newVersion="1.0.4.0" />
      
     
      </dependentAssembly>

      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.1.0.0" newVersion="2.1.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin.Security" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.1.0.0" newVersion="2.1.0.0" />
      </dependentAssembly>
      
      
    </assemblyBinding>
  </runtime>
  <system.webServer>
   
     
    <validation validateIntegratedModeConfiguration="false" />
    <modules runAllManagedModulesForAllRequests="true">
      
      
      <remove name="ApplicationInsightsWebTracking" />
      <add name="ApplicationInsightsWebTracking" type="Microsoft.ApplicationInsights.Web.ApplicationInsightsHttpModule, Microsoft.AI.Web" preCondition="managedHandler" />
      <remove name="TelemetryCorrelationHttpModule" />
      <add name="TelemetryCorrelationHttpModule" type="Microsoft.AspNet.TelemetryCorrelation.TelemetryCorrelationHttpModule, Microsoft.AspNet.TelemetryCorrelation" preCondition="managedHandler" />
    </modules>
  </system.webServer>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" />
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
    </providers>
  </entityFramework>
  <system.codedom>
    <compilers>
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:default /nowarn:1659;1699;1701" />
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:default /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+" />
    </compilers>
  </system.codedom>
</configuration>
```

## Authors

* **Hamed Refaat** - *Initial work* - [Hamed Refaat](https://gist.github.com/hamed1m54)



## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/hamed1m54/GlobalGeobits.ChatApp/blob/master/License.md) file for details

