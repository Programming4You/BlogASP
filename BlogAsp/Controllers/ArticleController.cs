using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;
using BlogAsp.DataLayer;
using BlogAsp.Models;
using PagedList;


namespace BlogAsp.Controllers
{
    
    public class ArticleController : Controller
    {
        // GET: Article
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // Get: Article/List
        [HttpGet]
        public ActionResult List(int? page)
        {
       
            OpArticleSelect op = new OpArticleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            List<ArticleDto> articles = result.Items.Cast<ArticleDto>().OrderByDescending(x => x.DatePost).ToList();

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
            
        }

        //
        // Get: Article/AllArticles
        [HttpGet]
        public ActionResult AllArticles()
        {
                OpArticleSelect op = new OpArticleSelect();
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                var articles = result.Items.Cast<ArticleDto>().Where(u => u.Author.UserName == this.User.Identity.Name).ToList();
 
                return View(articles);
        }

        //
        // Get: Article/Random
        [HttpGet]
        public ActionResult Random()
        {
            using (var database = new BlogEntities())
            {
                // Get count articles from database
                var countArticles = database.Articles.Count();


                // Get random article from article list
                Random random = new Random();
                int randomNum = random.Next(1, countArticles - 2);

                OpArticleSelect op = new OpArticleSelect();
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);
                var articles = result.Items.Cast<ArticleDto>().OrderBy(a => a.Id).Skip(randomNum).Take(4).ToList();

                return View(articles);
            }
        }

        //
        // GET: Article/Details
        [NoDirectAccess]
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OpArticleSelect op = new OpArticleSelect();
            op.Criteria = new ArticleCriteria() { Id = id};
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var article = result.Items.Cast<ArticleDto>().ToArray().Where(a => a.Id == id).First();

            // Check if article exists
            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
           
        }

        //
        // GET: Article/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: Article/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(ArticleDto article, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Insert article in database
                using (var database = new BlogEntities())
                {
       
                var authorId = database.AspNetUsers.Where(u => u.UserName == this.User.Identity.Name).First().Id;

                // Set articlea author
                article.AuthorId = authorId;

                    article.DatePost = DateTime.Now;

                    // Upload image. Check allowed types.
                    if (image != null)
                    {
                        var allowedContentTypes = new[] {"image/jpeg", "image/jpg", "image/png", "image/gif", "image/tif"};

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
            }

            return View(article);
        }

    


        //
        // POST: Article/Delete
        [Authorize]
        //[HttpPost]
        [HttpGet]
        //[ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OperationManager.Singleton.ExecuteOperation(new OpArticleDelete() { Article = new ArticleDto() { Id = id } }).Items.Cast<ArticleDto>().ToArray();

            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
    
        }

        //
        // GET: Article/Edit
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

                OpArticleSelect op = new OpArticleSelect();
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                var article = result.Items.Cast<ArticleDto>().Where(a => a.Id == id).First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // Check if article exists
                if (article == null)
                {
                    return HttpNotFound();
                }

                // Create the view model
                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;

                return View(model);          
        }

        //
        // POST: Article/Edit
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ArticleDto dto)
        {
            // Check if model state is valid
            if (ModelState.IsValid)
            {
   
                OpArticleUpdate op = new OpArticleUpdate();
                op.Article = dto;
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Index");
                
            }

            return View("Edit");
        }


        private bool IsUserAuthorizedToEdit(ArticleDto article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }





    }
}