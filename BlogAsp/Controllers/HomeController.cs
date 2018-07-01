using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace BlogAsp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Article");
        }


        public ActionResult Author()
        {
            return View();
        }




        public async Task<ActionResult> email(FormCollection form)
        {
            var name = form["sname"];
            var email = form["semail"];
            var messages = form["smessage"];
            var phone = form["sphone"];
            var x = await SendEmail(name, email, messages, phone);
            if (x == "sent")
                ViewData["esent"] = "Your Message has been sent";
            return RedirectToAction("Author");
        }


        //Must be set Less Secure Apps for Gmail

        private async Task<string> SendEmail(string name, string email, string messages, string phone)
        {                                                                                                     
            var message = new MailMessage();
            message.To.Add(new MailAddress("milan.cuckovic.302.15@ict.edu.rs")); // replace with receiver's email
            message.From = new MailAddress("youremail@gmail.com"); // replace with sender's email   
            message.Subject = "Message From" + email;
            message.Body = "Name: " + name + "\nFrom: " + email + "\nPhone: " + phone + "\n" + messages;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "youremail@gmail.com", // replace with sender's email
                    Password = "******" // replace with password   
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return "sent";
            }
        }



    }
}