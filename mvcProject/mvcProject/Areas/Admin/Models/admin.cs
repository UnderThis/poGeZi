using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcProject.Areas.Admin.Models
{
    public class admin
    {
        [Required]
        public string userName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
    public class adminAddModel
    {
        [Required]
        [StringLength(5, ErrorMessage = "至少包含 {2} 个字,不能多于{1}。", MinimumLength = 1)]
        public string Account { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "至少包含 {2} 个字,不能多于{1}。", MinimumLength = 3)]
        public string Password { get; set; }

         
    }
}