using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogAsp.Models
{
    public class UserRoleViewModel
    {
        public string Uuid { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select file!")]
        public string UserImage { get; set; }

        public string Photo { get { return UserImage == null ? "/Content/UserImages/NoImage.png" : UserImage; } }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }


        [Required]
        public string UserName { get; set; }



        [DisplayName("Role")]
        public string RoleName { get; set; }
    }
}