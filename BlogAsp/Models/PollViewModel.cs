using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogAsp.BusinessLayer.DTO;

namespace BlogAsp.Models
{
    public class PollViewModel
    {
        public PollDto Poll { get; set; }
        public PollAnswersDto[] Answers { get; set; }
    }
}