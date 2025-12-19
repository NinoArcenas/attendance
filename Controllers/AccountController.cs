using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using AttendanceManagementSystem.Filters;
using AttendanceManagementSystem.Models;
using WebMatrix.WebData;
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes;

namespace AttendanceManagementSystem.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private UsersContext db = new UsersContext();
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //WebSecurity.ChangePassword("00001777", "123456", "123");
        //Get Fname Display sa Tible
        [HttpPost]
        public string changepass(string idno, string oldpassword, string newpassword)
        {
            UsersContext db = new UsersContext();
            var c = (from emp in db.Users
                     where emp.username == idno
                     select emp).Count();
            if (c > 0) //meaning naay employee naka exist.. pwedena ma change pass
            {
                WebSecurity.ChangePassword(idno, oldpassword, newpassword);
                return "na change na";
            }
            else //wala dili ma change pass
            {
                return "wala ma change ang password";
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //Create User Registration
        [AllowAnonymous]
        public ActionResult CreateUserRegistration()
        {
            Employee_list();

            var client = from ab in db.Users
                         select ab;
            ViewBag.Users = client;
            return View();
        }

        public ActionResult GetEmployeeList()
        {
            using (var db = new UsersContext())
            {
                var positionHolders = db.Employees.ToList();
                return Json(positionHolders, JsonRequestBehavior.AllowGet);
            }
        }

        //Save Position Holder
        [HttpPost]
        public JsonResult Save_CreateUserRegistration(string lifenumber, string fname, string mname, string lname,
                                              string company, string gender, string position,
                                              string department, string email)
        {
            string message = "";
            bool success = false;

            int isexist = db.Employees.Count(a => a.fname == fname);
            if (isexist > 0)
            {
                message = "This Member already exists.";
            }
            else
            {
                try
                {
                    Employee d = new Employee
                    {
                        idno = lifenumber,
                        fname = fname,
                        company = company,
                        gender = gender,
                        date_hired = DateTime.Now,
                        emp_status = position,
                        dept = department,
                        email = email,
                        designation = department,
                        created_by = User.Identity.Name,
                        date_created = DateTime.Now
                    };

                    db.Employees.Add(d);
                    db.SaveChanges();

                    success = true;
                    message = "User registration successfully saved.";
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    message = "Validation Errors: ";
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            message += $"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}; ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = "Error saving the position holder: " + ex.Message;
                }
            }
            return Json(new { success = success, message = message });
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterEmployee_list();

            var client = from ab in db.Users select ab;
            ViewBag.Users = client;
            return View();
        }

        [HttpGet]
        public JsonResult EmpDetails()
        {
            Employee_list();

            var client = from ab in db.Users
                         where ab.status == 1
                         orderby ab.datecreated descending
                         select new { ab.id,ab.username,ab.fname,ab.company,ab.email,ab.department,ab.accesstype };
            return Json(client, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmpDetails_View(int id)
        {
            var client = from ab in db.Users
                         where ab.status == 1 && ab.id == id
                         orderby ab.datecreated descending
                         select new { ab.id, ab.username, ab.fname, ab.company, ab.department, ab.email, ab.accesstype };
            return Json(client, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmpEmail_View(int id)
        {
            var client = from ab in db.Users
                         where ab.status == 1 && ab.id == id
                         select new { ab.id, ab.email };
            return Json(client, JsonRequestBehavior.AllowGet);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password);

                var test = db.Employees.FirstOrDefault(ab => ab.idno == model.UserName);
                if (test != null)
                {
                    User d = new User
                    {
                        username = model.UserName,
                        fname = test.fname,
                        company = test.company,
                        department = test.dept,
                        email = model.Email,
                        accesstype = model.AccessType,
                        datecreated = DateTime.Now,
                        createdby = User.Identity.Name,
                        dateupdated = DateTime.Now,
                        updatedby = User.Identity.Name,
                        status = 1
                    };

                    db.Users.Add(d);
                    db.SaveChanges();
                }

                return RedirectToAction("Register", "Account");
            }

            // ❗ Refill dropdown before returning the view to avoid null ViewBag.register
            RegisterEmployee_list();

            // Optional: repopulate users list too if needed in your view
            var client = from ab in db.Users select ab;
            ViewBag.Users = client;

            return View(model);
        }

        //Update Access Type
        [HttpPost]
        public string UpdateAccessType(int acc, int id)
        {
            User d = db.Users.Find(id);
            d.accesstype = acc;
            d.dateupdated = DateTime.Now;
            d.updatedby = User.Identity.Name;

            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();
            return "updateaccesstype";
        }

        //Update Email
        [HttpPost]
        public string UpdateEmails(int id, string eml)
        {
            User d = db.Users.Find(id);
            d.email = eml;
            d.dateupdated = DateTime.Now;
            d.updatedby = User.Identity.Name;

            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();
            return "updateemail";
        }

        //Update Information
        [HttpPost]
        public string UpdateInformation(int id, string eml, int acc)
        {
            User d = db.Users.Find(id);
            d.email = eml;
            d.accesstype = acc;
            d.dateupdated = DateTime.Now;
            d.updatedby = User.Identity.Name;

            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();
            return "updateinfo";
        }

        //List of Account name
        private void Employee_list()
        {
            var acc = db.ZionP0sitionHolders
                .Where(x=> !db.Users.Any(y => y.username == x.life_number))
         .Select(x => new
         {
             Value = x.life_number + "*" + x.position + "*" + x.full_name,
             Text = x.full_name
         }).ToList();
            ViewBag.employee = acc;
        }

        private void RegisterEmployee_list()
        {
            var acc = db.Employees
                .Where(x => !db.Users.Any(y => y.username == x.idno))
         .Select(x => new
         {
             Value = x.idno + "*" + x.email,
             Text = x.fname
         }).ToList();
            ViewBag.register = acc;
        }

        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(string UserName)
        {
            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            //    : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
            //    : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(UserName));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }        

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult Profile(string username)
        {
            var user = from ab in db.Users                       
                       where ab.username == username
                       select ab;
            ViewBag.users = user;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profileImage(HttpPostedFileBase file, DirectoryPicture directoryimage)
        {
            //get id using username from table users.. used this to update data 
            int ids = (from u in db.Users
                       where u.username == User.Identity.Name && u.status == 1
                       select u).FirstOrDefault().id;

            User um = db.Users.Find(ids);
            if (!String.IsNullOrEmpty(User.Identity.Name))
            {
                var proImg = directoryimage.imageforpreview;

                if (proImg.ContentLength > 0)
                {
                    byte[] PicProfile = null;
                    using (var binReader = new BinaryReader(proImg.InputStream))
                    {
                        PicProfile = binReader.ReadBytes(proImg.ContentLength);
                    }
                    um.photopicture = PicProfile;
                    db.Entry(um).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.success = "okay";
                }
                else
                {
                    ViewBag.err = "no picture";

                }
            }
            return RedirectToAction("Index", "Home");
        }

        //get the images in user profile
        public static byte[] DisplayImageProfilePicture(string username)
        {
            UsersContext db = new UsersContext();

            var pic = (from photo in db.Users
                       where photo.username == username && photo.status == 1
                       select photo).FirstOrDefault();
            if (pic != null)
            {
                return pic.photopicture;
            }
            return null;
        }

        public string name(string name)
        {
            var user = from ab in db.Users
                       where ab.username == name
                       select ab;
            return user.FirstOrDefault().fname;
        }


        public ActionResult UserProfile(string username)
        {
            
            var test = from ab in db.Users
                       where ab.fname == username
                       select ab;
            ViewBag.test = test;

            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
        
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
