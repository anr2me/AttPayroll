using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Core.Interface.Service;
using Data.Repository;
using log4net;
using Service.Service;
using Validation.Validation;

namespace WebView.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("AuthenticationController");
        private IUserAccountService _userAccountService;

        public AuthenticationController()
        {
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
        }

        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Account/

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to requested URL, or homepage if no previous page requested
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                LoginModel model = new LoginModel();
                //model.ListCompany = (from b in _companyService.GetCompanies("", "", "")
                //                     where b.Deleted == false
                //                     select new SelectListItem
                //                     {
                //                         Text = b.CompanyName,
                //                         Value = b.Id.ToString()
                //                     }).ToList();
                //model.ListCompany.Insert(0, new SelectListItem { Value = "0", Text = "-- Please Select Company --" });

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: validate user password, save to session, etc
                    string password = user.Password.Trim();
                    string username = user.UserName.Trim();
                    //password = StringEncryptor.Encrypt(password);


                    if (ModelState.IsValid)
                    {
                        var objUser = _userAccountService.IsLoginValid(username, password);
                        
                        if (objUser == null)
                        {
                            ModelState.AddModelError("", "Invalid credential");
                        }
                        else if (objUser.IsDeleted)
                        {
                            ModelState.AddModelError("", "User has been removed");
                        }
                        else
                        {
                            int SessionTime = 120;
                            string strSessionTime = System.Configuration.ConfigurationManager.AppSettings["SessionTime"];
                            if (!String.IsNullOrEmpty(strSessionTime))
                            {
                                if (!int.TryParse(strSessionTime, out SessionTime))
                                    SessionTime = 120;
                            }

                            FormsAuthenticationTicket tkt;
                            string cookiestr;
                            HttpCookie ck;
                            tkt = new FormsAuthenticationTicket(1,
                                objUser.Id.ToString(),
                                DateTime.Now,
                                DateTime.Now.AddMinutes(SessionTime),
                                user.RememberMe,
                                objUser.Username + "#" + objUser.Name);
                            cookiestr = FormsAuthentication.Encrypt(tkt);
                            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                            if (user.RememberMe)
                                ck.Expires = tkt.Expiration;
                            ck.Path = FormsAuthentication.FormsCookiePath;
                            Response.Cookies.Add(ck);

                            // Log
                            LOG.Info("Login Success, UserCode: " + username);

                            // Redirect to requested URL, or homepage if no previous page requested
                            string returnUrl = Request.QueryString["ReturnUrl"];
                            if (!String.IsNullOrEmpty(returnUrl))
                                return Redirect(returnUrl);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //  LOG.Error("Login Failed, username:" + user.UserName, ex);
                    ModelState.AddModelError("", "Login Failed, Please try Again or Contact Your Administrator.");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Log
            LOG.Info("Logout Success, UserCode: ");

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Authentication");
        }
    }
}
