using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;

namespace BlogAsp.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: Admin/Role
        public ActionResult Index(string searchString)
        {
            OpRoleSelect op = new OpRoleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            
            if (!String.IsNullOrEmpty(searchString))
            {
                var search = result.Items.Cast<RoleDto>().Where(s => s.Name.Contains(searchString));
                return View(search.ToList());
            }

            return View(result.Items as RoleDto[]);

        }



        // GET: Admin/Role/Details/5
        public ActionResult Details(string id)
        {
            OpRoleSelect op = new OpRoleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);
            var role = result.Items.Cast<RoleDto>().Where(r => r.Uuid == id).FirstOrDefault();

            var model = new RoleDto();
            model.Uuid = role.Uuid;
            model.Name = role.Name;

            return View(model);
        }



        // GET: Admin/Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        [HttpPost]
        public ActionResult CreateRole(RoleDto dto)
        {
           
                OpRoleInsert op = new OpRoleInsert();
                op.RoleDto = dto;
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                if (result.Status)
                {
                    TempData["Success"] = "Added Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create");
                }

          
        }



        // GET: Admin/Role/Edit/5
        public ActionResult Edit(string id)
        {
            OpRoleSelect op = new OpRoleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var role = result.Items.Cast<RoleDto>().Where(r => r.Uuid == id).FirstOrDefault();

            if (role == null)
            {
                return HttpNotFound();
            }

            // Create the view model
            var model = new RoleDto();
            model.Uuid = role.Uuid;
            model.Name = role.Name;
   
            return View(model);
        }

        // POST: Admin/Role/Edit/5
        [HttpPost]
        public ActionResult EditRole(RoleDto dto)
        {
            //if (ModelState.IsValid)
            //{
                OpRoleUpdate op = new OpRoleUpdate();
                op.RoleDto = dto;
                OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Index");
          
            //}

        }



        // GET: Admin/Role/Delete/5
        public ActionResult Delete(string id)
        {
            OpRoleSelect op = new OpRoleSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var role = result.Items.Cast<RoleDto>().Where(r => r.Uuid == id).FirstOrDefault();

            if (role == null)
            {
                return HttpNotFound();
            }

            // Create the view model
            var model = new RoleDto();
            model.Uuid = role.Uuid;
            model.Name = role.Name;

            return View(model);
        }

        // POST: Admin/Role/Delete/5
        [HttpPost]
        public ActionResult Delete(RoleDto dto)
        {
            OpRoleDelete op = new OpRoleDelete();
            op.Uuid = dto.Uuid;
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
