using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer
{
    public abstract class SelectCriteria
    {
        public string Uuid { get; set; }
        public int Id { get; set; }
    }
}