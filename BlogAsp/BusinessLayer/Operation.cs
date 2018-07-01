using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogAsp.DataLayer;

namespace BlogAsp.BusinessLayer
{
    public abstract class Operation
    {
        public abstract OperationResult Execute(BlogEntities entities);
    }


}