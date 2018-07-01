using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;
using BlogAsp.Models;
using PagedList;

namespace BlogAsp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TextController : Controller
    {
        // GET: Admin/Text
        public ActionResult Index(int? page, string searchString)
        {
            OpArticleSelect op = new OpArticleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                var search = result.Items.Cast<ArticleDto>().Where(a => a.Content.Contains(searchString));
                return View(search.ToPagedList(pageNumber, pageSize));
            }

            List<ArticleDto> articles = result.Items.Cast<ArticleDto>().ToList();

            return View(articles.ToPagedList(pageNumber, pageSize));
        }



        // GET: Admin/Text/Details/5
        public ActionResult Details(int id)
        {
            OpArticleSelect op = new OpArticleSelect();
            op.Criteria = new ArticleCriteria() { Id = id };
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var article = result.Items.Cast<ArticleDto>().Where(a => a.Id == id).First();
 
            // Check if article exists
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }




        // GET: Admin/Text/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Text/Create
        [HttpPost]
        public ActionResult Create(ArticleDto article, HttpPostedFileBase image)
        {
            // Author is admin
            article.AuthorId = "cb646481-f6b9-4874-a7f6-b90f329cabb1";

            article.DatePost = DateTime.Now;

            // Upload image. Check allowed types.
            if (image != null)
            {
                var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/tif" };

                if (allowedContentTypes.Contains(image.ContentType))
                {
                    var imagesPath = "/Content/Images/";
                    var filename = Guid.NewGuid().ToString() + image.FileName;
                    var uploadPath = imagesPath + filename;
                    var physicalPath = Server.MapPath(uploadPath);
                    image.SaveAs(physicalPath);
                    article.ImagePath = uploadPath;
                }
            }
            else
            {
                return RedirectToAction("Create");
            }


            OpArticleInsert op = new OpArticleInsert();
            op.Article = article;
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);


            if (result.Status)
            {
                TempData["Success"] = "Added Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }




        // GET: Admin/Text/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            OpArticleSelect op = new OpArticleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var article = result.Items.Cast<ArticleDto>().Where(a => a.Id == id).First();


            // Check if article exists
            if (article == null)
            {
                return HttpNotFound();
            }

            // Create the view model
            var model = new ArticleDto();
            model.Id = article.Id;
            model.Title = article.Title;
            model.Content = article.Content;
            model.ImagePath = article.ImagePath;

            return View(model);
        }


        // POST: Admin/Text/Edit/5
        [HttpPost]
        public ActionResult Edit(ArticleDto user, HttpPostedFileBase image)
        {

            if (image != null)
            {
                var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/tif" };

                if (allowedContentTypes.Contains(image.ContentType))
                {
                    var imagesPath = "/Content/Images/";
                    var filename = Guid.NewGuid().ToString() + image.FileName;
                    var uploadPath = imagesPath + filename;
                    var physicalPath = Server.MapPath(uploadPath);
                    image.SaveAs(physicalPath);
                    user.ImagePath = uploadPath;
                }
            }
            else
            {
                return RedirectToAction("Edit");
            }


                OpArticleUpdate op = new OpArticleUpdate();
                op.Article = user;
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Index");

        }





        // GET: Admin/Text/Delete/5
        public ActionResult Delete(int id)
        {
            OpArticleSelect op = new OpArticleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var article = result.Items.Cast<ArticleDto>().Where(u => u.Id == id).FirstOrDefault();

            var model = new ArticleDto();
            model.Id = article.Id;
            model.Title = article.Title;
            model.Content = article.Content;
            model.ImagePath = article.ImagePath;
            model.DatePost = article.DatePost;

            return View(model);
        }



        // POST: Admin/Text/Delete/5
        [HttpPost]
        public ActionResult DeleteArticle(int id)
        {
            OperationManager.Singleton.ExecuteOperation(new OpArticleDelete() { Article = new ArticleDto() { Id = id } }).Items.Cast<ArticleDto>().ToArray();

            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
