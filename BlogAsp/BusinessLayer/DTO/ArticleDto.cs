﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BlogAsp.DataLayer;
using BlogAsp.Models;

namespace BlogAsp.BusinessLayer.DTO
{
    public class ArticleDto : BaseDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(400)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public string ImagePath { get; set; }

        public DateTime DatePost { get; set; }

        public virtual AspNetUser Author { get; set; }


        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

        public string PreviewText(string text)
        {
            if (text.Length <= 300 || text == null)
            {
                return text;
            }

            return text.Substring(0, 300) + " ...";
        }


    }
}