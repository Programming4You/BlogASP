using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogAsp.BusinessLayer.DTO;
using BlogAsp.DataLayer;
using Microsoft.Ajax.Utilities;

namespace BlogAsp.BusinessLayer.Operations
{

    public class RoleCriteria : SelectCriteria
    {
        public string Name { get; set; }
    }



    public class OpRoleBase : Operation
    {
        public RoleCriteria Criteria { get; set; }

        private RoleDto roleDto;
        public RoleDto RoleDto
        {
            get { return roleDto; }
            set { roleDto = value; }
        }


        public override OperationResult Execute(BlogEntities entities)
        {

            IEnumerable<RoleDto> ieRoles = from role in entities.AspNetRoles
                select new RoleDto
                {
                    Uuid = role.Id,
                    Name = role.Name
                };

            if (Criteria != null && Criteria.Uuid != null)
            {
                ieRoles = ieRoles.Where(r => r.Uuid == Criteria.Uuid);
            }


            if (Criteria != null && Criteria.Name != null)
            {
                ieRoles = ieRoles.Where(r => r.Name == Criteria.Name);
            }

            OperationResult result = new OperationResult();
            result.Items = ieRoles.ToArray();
            result.Status = true;
            return result;
        }
    }


    public class OpRoleSelect : OpRoleBase
    {

    }



    public class OpRoleInsert : OpRoleBase
    {
        public override OperationResult Execute(BlogEntities entities)
        {
            AspNetRole role = new AspNetRole();
            role.Id = Guid.NewGuid().ToString();
            role.Name = this.RoleDto.Name;

            entities.AspNetRoles.Add(role);
            entities.SaveChanges();
            return base.Execute(entities);
        }
    }



    public class OpRoleUpdate : OpRoleBase
    {
        public override OperationResult Execute(BlogEntities entities)
        {
            AspNetRole role = entities.AspNetRoles.Where(r => r.Id == RoleDto.Uuid).FirstOrDefault();
            if (role != null)
            {
                role.Name = this.RoleDto.Name;

                entities.SaveChanges();
                return base.Execute(entities);
            }

            OperationResult result = new OperationResult();
            result.Status = false;
            result.Message = "The role does not exist!";
            return result;
        }
    }





    public class OpRoleDelete : OpRoleBase
    {

        public string Uuid { get; set; }


        public override OperationResult Execute(BlogEntities entities)
        {

            AspNetRole role = entities.AspNetRoles.Where(r => r.Id == Uuid && r.AspNetUsers.Count() == 0).FirstOrDefault();
            if (role != null)
            {
                entities.AspNetRoles.Remove(role);
                entities.SaveChanges();
                return base.Execute(entities);
            }

            OperationResult result = new OperationResult();
            result.Status = false;
            result.Message = "The role does not exist or contains users";
            return result;
        }
    }



}




