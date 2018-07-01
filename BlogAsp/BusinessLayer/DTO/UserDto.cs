
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BlogAsp.BusinessLayer.DTO
{
    public class UserDto : BaseDto
    {

        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select file!")]
        public string UserImage { get; set; }

        public string Photo { get { return UserImage == null ? "/Content/UserImages/NoImage.png" : UserImage; } }

        [Required]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [Required]
        public string UserName { get; set; }


        public DateTime? Logged { get; set; }
        public DateTime? Logout { get; set; }

        [DisplayName("Role")]
        public RoleDto RoleName { get; set; }

    }
}