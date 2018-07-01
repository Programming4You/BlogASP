using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer.DTO
{
    public class PollAnswersDto : BaseDto
    {
        public int IdPollAnswer { get; set; }
        public string PollAnswer { get; set; }
        public int? IdUserLastChange { get; set; }
        public DateTime? LastTimeChanged { get; set; }
    }
}