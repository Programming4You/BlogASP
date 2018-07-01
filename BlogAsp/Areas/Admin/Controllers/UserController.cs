using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogAsp.BusinessLayer;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.BusinessLayer.Operations;
using BlogAsp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;


namespace BlogAsp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index(int? page, string searchString)
        {
            OpUserSelect op = new OpUserSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            int pageSize = 4;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                var search = result.Items.Cast<UserDto>().Where(u => u.FullName.Contains(searchString));
                return View(search.ToPagedList(pageNumber, pageSize));
            }
            
            List<UserDto> users = result.Items.Cast<UserDto>().ToList();
    
            return View(users.ToPagedList(pageNumber, pageSize));
            
        }



        // GET: Admin/User/Details/5
        public ActionResult Details(string id)
        {
            OpUserSelect op = new OpUserSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var user = result.Items.Cast<UserDto>().Where(u => u.Uuid == id).FirstOrDefault();

            var model = new UserDto();
            model.Uuid = user.Uuid;
            model.FullName = user.FullName;
            model.UserImage = user.UserImage;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.UserName = user.UserName;

            return View(user);
        }



        // GET: Admin/User/Create
        public ActionResult Create()
        {
            ViewBag.roles = OperationManager.Singleton.ExecuteOperation(new OpRoleSelect()).Items as RoleDto[];

            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(UserDto user, HttpPostedFileBase image, string RoleName)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            // Upload image. Check allowed types.
            if (image != null)
                {
                    var allowedContentTypes = new[] {"image/jpeg", "image/jpg", "image/png", "image/gif", "image/tif"};

                    if (allowedContentTypes.Contains(image.ContentType))
                    {
                        var imagesPath = "/Content/UserImages/";
                        var filename = Guid.NewGuid().ToString() + image.FileName;
                        var uploadPath = imagesPath + filename;
                        var physicalPath = Server.MapPath(uploadPath);
                        image.SaveAs(physicalPath);
                        user.UserImage = uploadPath;
                    }
                }
            else
            {
                return RedirectToAction("Create");
            }


            OpUserInsert op = new OpUserInsert();
            op.UserDto = user;
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);


            if (result.Status)
            {       //added User role 
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var u = UserManager.FindByName(user.UserName);
                    var userId = u.Id;                                  
                    UserManager.AddToRole(userId, RoleName);   //this add RoleName
                    db.SaveChanges();

                    TempData["Success"] = "Added Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create");
                }

        }




        // GET: Admin/User/Edit/5
        public ActionResult Edit(string id)
        {
            OpUserSelect op = new OpUserSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var user = result.Items.Cast<UserDto>().Where(u => u.Uuid == id).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UserDto();
            model.Uuid = user.Uuid;
            model.FullName = user.FullName;
            model.UserImage = user.UserImage;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.UserName = user.UserName;

            return View(model);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserDto user, HttpPostedFileBase image)
        {

            if (image != null)
            {
                var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/tif" };

                if (allowedContentTypes.Contains(image.ContentType))
                {
                    var imagesPath = "/Content/UserImages/";
                    var filename = Guid.NewGuid().ToString() + image.FileName;
                    var uploadPath = imagesPath + filename;
                    var physicalPath = Server.MapPath(uploadPath);
                    image.SaveAs(physicalPath);
                    user.UserImage = uploadPath;
                }
            }
            else
            {
                return RedirectToAction("Edit");
            }

            OpUserUpdate op = new OpUserUpdate();
            op.UserDto = user;
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            TempData["Success"] = "Updated Successfully!";
            return RedirectToAction("Index");
        }




        // GET: Admin/User/Delete/5
        public ActionResult Delete(string id)
        {

            OpUserSelect op = new OpUserSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            var user = result.Items.Cast<UserDto>().Where(u => u.Uuid == id).FirstOrDefault();

            var model = new UserDto();
            model.Uuid = user.Uuid;
            model.FullName = user.FullName;
            model.UserImage = user.UserImage;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.UserName = user.UserName;

            return View(model);
        }



        // POST: Admin/User/Delete/5
        [HttpPost]
        public ActionResult DeleteUser(UserDto dto)
        {
            OpUserDelete op = new OpUserDelete();                       
            op.Uuid = dto.Uuid;         

            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);

            TempData["Success"] = "Deleted Successfully!";
            return RedirectToAction("Index");


        }


        
        public ActionResult Report(int? page, string searchString, string sortDates)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            OpUserSelect op = new OpUserSelect();
            OperationResult result = OperationManager.Singleton.ExecuteOperation(op);


            if (!String.IsNullOrEmpty(searchString))
            {
                var search = result.Items.Cast<UserDto>().Where(u => u.FullName.Contains(searchString));
                return View(search.ToPagedList(pageNumber, pageSize));
            }


            ViewBag.DateSortParm = sortDates == "Date" ? "date_desc" : "Date";
                var sortUsers = from u in result.Items.Cast<UserDto>().ToList()
                               select u;
                switch (sortDates)
                {
                    case "Date":
                        sortUsers = sortUsers.OrderBy(u => u.Logged);
                        break;
                    case "date_desc":
                        sortUsers = sortUsers.OrderByDescending(u => u.Logged);
                        break;
                    default:
                        sortUsers = sortUsers.OrderBy(u => u.Logged);
                        break;
                }

                return View(sortUsers.ToPagedList(pageNumber, pageSize));
                 
        }



    }
}
