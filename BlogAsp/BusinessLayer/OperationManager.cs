using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogAsp.DataLayer;


namespace BlogAsp.BusinessLayer
{
    [Authorize]
    public class OperationManager
    {
 
        private BlogEntities _entiteti = new BlogEntities();


        private OperationManager() { }


        private static volatile OperationManager singleton;
        private static object syncRoot = new object();

        public static OperationManager Singleton
        {
            get
            {
                if (OperationManager.singleton == null)
                {
                    lock (OperationManager.syncRoot)
                    {
                        if(OperationManager.singleton == null)
                            OperationManager.singleton = new OperationManager();
                    }
                   
                }

                return OperationManager.singleton;
            }
        }




        public OperationResult ExecuteOperation(Operation o)
        {

            OperationResult result;

            try
            {
                result = o.Execute(_entiteti);
            }
            catch (Exception e)
            {
                result = new OperationResult();
                result.Message = e.Message;
                result.Status = false;
            }

            return result;
        }


    }

}