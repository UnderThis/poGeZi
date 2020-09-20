using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mvcProject.Models
{
    public partial class Lu_REntities1 : DbContext
    {
        public Lu_REntities1()
            : base("name=Lu_REntities1")
        {
        }

        
    }
    
        public class LoginModel
        {
            //[Required]
            //[Display(Name = "用户名")]
            public string Accounts { get; set; }

            //[Required]
            //[DataType(DataType.Password)]
            //[Display(Name = "密码")]
            public string Password { get; set; }

            
        }

   
}