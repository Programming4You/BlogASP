using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogAsp.BusinessLayer.DTO
{
    public class RoleDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
    }
}