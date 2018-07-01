using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer.DTO
{
    public class PollDto : BaseDto
    {
        public int IdPollQuestion { get; set; }
        public string PollQuestion { get; set; }

        public PollAnswersDto[] answers { get; set; }

        public int IdAnswer { get; set; }
        public string IpAddress { get; set; }
        public int IdQuestion { get; set; }
        public int? IdUserLastChange { get; set; }
        public DateTime? LastTimeChanged { get; set; }

    }
}