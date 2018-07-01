using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.DataLayer;

namespace BlogAsp.BusinessLayer.Operations
{

    public class ArticleCriteria : SelectCriteria
    {

    }

    public class OpArticleBase : Operation
    {
        private ArticleDto article;

        public ArticleDto Article
        {
            get { return article; }
            set { article = value; }
        }

        public ArticleCriteria Criteria { get; set; }

        public override OperationResult Execute(BlogEntities entities)
        {
             
            IEnumerable<ArticleDto> ieArticles = from article in entities.Articles
                         join user in entities.AspNetUsers on article.AuthorId equals user.Id 
                select new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    ImagePath = article.ImagePath,
                    DatePost = article.DatePost,
                    Author = article.AspNetUser,
                    AuthorId = article.AspNetUser.Id
                };

            if (Criteria != null && Criteria.Uuid != null)
            {
                ieArticles = ieArticles.Where(a => a.Uuid == Criteria.Uuid);
            }

            if (Criteria != null && Criteria.Id != null)
            {
                ieArticles = ieArticles.Where(a => a.Id == Criteria.Id);
            }

            OperationResult result = new OperationResult();
            result.Items = ieArticles.ToArray();
            result.Status = true;
            return result;
        }
    }


    public class OpArticleSelect : OpArticleBase
    {

    }




    public class OpArticleInsert : OpArticleBase
    {
     
        public override OperationResult Execute(BlogEntities entities)
        {
            Article article = new Article();
            article.Title = this.Article.Title;
            article.Content = this.Article.Content; 
            article.ImagePath = this.Article.ImagePath;
            article.AuthorId = this.Article.AuthorId;
            article.DatePost = this.Article.DatePost;

            entities.Articles.Add(article);
            entities.SaveChanges();
            return base.Execute(entities);

        }
    }



    public class OpArticleUpdate : OpArticleBase
    {
        public override OperationResult Execute(BlogEntities entities)
        {
            Article article = entities.Articles.Where(a => a.Id == Article.Id).FirstOrDefault();

            if (article != null)
            {
                article.Id = this.Article.Id;
                article.Title = this.Article.Title;
                article.Content = this.Article.Content;
                article.ImagePath = this.Article.ImagePath;

                entities.SaveChanges();
                return base.Execute(entities);
            }

            OperationResult result = new OperationResult();
            result.Status = false;
            result.Message = "Article does not exist";
            return result;
        }
    }


    public class OpArticleDelete : OpArticleBase
    {

        public override OperationResult Execute(BlogEntities entities)
        {

            Article article = entities.Articles.Where(a => a.Id == Article.Id).FirstOrDefault();
         
                entities.Articles.Remove(article);
                entities.SaveChanges();

                return base.Execute(entities);
         
        }
    } 


}