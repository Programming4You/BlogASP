using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;
using BlogAsp.Models;

namespace BlogAsp.Controllers
{
    public class PollController : Controller
    {


        public ActionResult Poll()
        {
            OpPollSelect op = new OpPollSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            PollDto[] poll = result.Items.Cast<PollDto>().ToArray();


            PollViewModel vm = new PollViewModel
            {
                Poll = poll[0] as PollDto 
            };

            return View(vm);
        }



        public ActionResult AjaxPollVote()
        {
    
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            string[] items = json.Split('&');
            int id = Convert.ToInt32(items[0].Split('=')[1]);
            int qid = Convert.ToInt32(items[1].Split('=')[1]);

            OperationResult res = OperationManager.Singleton.ExecuteOperation(new OpPollVoteInsert() { Vote = new PollDto() { IdAnswer = id, IpAddress = Request.UserHostAddress, IdQuestion = qid } });
            if (res.Status)
                Response.Write("Success");
            else
                Response.Write("Error");


            return View(res);
        }


    }
}