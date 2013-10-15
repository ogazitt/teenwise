namespace TeenWise.WebSite.Controllers
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Web.Mvc;
    using System.Web.Security;
    using TeenWise.WebSite.Models;
    using TeenWise.WebSite.Models.Storage;

    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string email, string password, string returnUrl)
        {
            RegisteredUser validUser = null;
            string error = null; 
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (Membership.ValidateUser(email, password))
                    {
                        validUser = new RegisteredUser() { EmailAddress = email };
                        FormsAuthentication.SetAuthCookie(email, true);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        try
                        {   // register user in database
                            Storage.Users.RegisterUser(email);
                            validUser = new RegisteredUser() { EmailAddress = email };
                            FormsAuthentication.SetAuthCookie(email, true);
                            SendEmailNotification("New user registered at TeenWise.com", email);
                        }
                        catch (Exception)
                        {
                            error = string.Format("Unable to register '{0}' at this time. Please try again later.", email);
                        }
                    }
                }
                catch (Exception)
                {
                    error = string.Format("Unable to register or verify '{0}' at this time. Please try again later.", email);
                }
            }
            else
            {
                error = string.Format("'{0}' is not a valid email address.", email);
            }

            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("summary", error);
            } 

            return View("LogOn", validUser);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public static bool SendEmailNotification(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("TeenWisesoftware@gmail.com");
            message.To.Add(new MailAddress("ogazitt@TeenWise.com"));
            message.To.Add(new MailAddress("smillet@TeenWise.com"));
            message.Subject = subject;
            message.Body = body;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(message.From.Address, "zrc022..");
            smtp.Host = "smtp.gmail.com";

            try
            {
                smtp.Send(message);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
