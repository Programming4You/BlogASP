using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.DataLayer;
using BlogAsp.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogAsp.BusinessLayer.Operations
{

    public class UserCriteria : SelectCriteria
    {

    }

    public class OpUserBase : Operation
    {

        public UserCriteria Criteria { get; set; }

        public UserDto UserDto { get; set; }


        public override OperationResult Execute(BlogEntities entities)
        {

            IEnumerable<UserDto> ieUsers = from user in entities.AspNetUsers

                                           select new UserDto
                {
                    Uuid = user.Id,
                    FullName = user.FullName,
                    UserImage = user.UserImage,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,

                      RoleName = (from role in user.AspNetRoles
                                 select new RoleDto {
                                     Name = role.Name
                                 }).ToList().FirstOrDefault(),

                    Logged = user.Logged,
                    Logout = user.Logout
                };

           
            OperationResult result = new OperationResult();
            result.Items = ieUsers.ToArray();
            result.Status = true;
            return result;
        }
    }


    public class OpUserSelect : OpUserBase
    {

    }


    public class OpUserInsert : OpUserBase
    {

        private UserDto userDto;

        public UserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; }
        }

        ApplicationDbContext db = new ApplicationDbContext();

        public override OperationResult Execute(BlogEntities entities)
        {
    
            AspNetUser userDto = new AspNetUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = this.userDto.FullName,
                UserImage = this.userDto.UserImage,
                Email = this.userDto.Email,
                PasswordHash = "ACSljbNQuXi2ibOu690+vN5o4NrbVWLV03fD2r81QVS2VD2NYAZUaSKVG11LA7uJjQ==",   //default password
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = false,
                PhoneNumber = this.userDto.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = this.userDto.UserName
            };

            entities.AspNetUsers.Add(userDto);
            
            entities.SaveChanges();

            return base.Execute(entities);
        }
    }





    public class OpUserUpdate : OpUserBase
    {
        public override OperationResult Execute(BlogEntities entities)
        {
            AspNetUser user = entities.AspNetUsers.Where(u => u.Id == UserDto.Uuid).FirstOrDefault();

            if (user != null)
            {
                user.Id = this.UserDto.Uuid;
                user.FullName = this.UserDto.FullName;
                user.UserImage = this.UserDto.UserImage;
                user.Email = this.UserDto.Email;
                user.EmailConfirmed = false;
                user.PhoneNumber = this.UserDto.PhoneNumber;
                user.PhoneNumberConfirmed = false;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = true;
                user.AccessFailedCount = 0;
                user.UserName = this.UserDto.UserName;
                entities.SaveChanges();

                return base.Execute(entities);
            }

            OperationResult result = new OperationResult();
            result.Status = false;
            result.Message = "User does not exist";
            return result;
        }
    }





    public class OpUserDelete : OpUserBase
    {
        public string Uuid { get; set; }


        public override OperationResult Execute(BlogEntities entities)
        {

            AspNetUser user = entities.AspNetUsers.Where(u => u.Id == Uuid).FirstOrDefault();


            if (user != null)
            {
                entities.AspNetUsers.Remove(user);
                entities.SaveChanges();
                return base.Execute(entities);
            }

            OperationResult result = new OperationResult();
            result.Status = false;
            result.Message = "The user does not exist";
            return result;


        }
    }



}