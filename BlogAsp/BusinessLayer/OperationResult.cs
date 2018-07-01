using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer
{
    public class OperationResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public BaseDto[] Items { get; set; }
    }
}