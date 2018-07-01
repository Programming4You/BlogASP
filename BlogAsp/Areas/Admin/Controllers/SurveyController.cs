using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;
using BlogAsp.DataLayer;
using BlogAsp.Models;


namespace BlogAsp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveyController : Controller
    {
        // GET: Admin/Survey
        public ActionResult Index()
        {
            var result = OperationManager.Singleton.ExecuteOperation(new OpPollQuestionSelect()).Items.Cast<PollDto>().ToArray();
            return View(result);
        }




        // POST: Admin/Survey/Delete/5
        [HttpGet]
        public ActionResult DeleteQuestion(int id)
        {
        
            OperationManager.Singleton.ExecuteOperation(new OpPollDeleteQuestion() { PollQuestion = new PollDto() { IdPollQuestion = id } }).Items.Cast<PollDto>().ToArray();

            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
