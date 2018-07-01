using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
    }
}