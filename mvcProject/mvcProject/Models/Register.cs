using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
using System.Web.Mvc;

namespace mvcProject.Models
{
    
        public class RegisterModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
            [Display(Name = "账号")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            
            [Display(Name = "邮箱")]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,4}",ErrorMessage="格式错误" )]
            public string E_mail { get; set; }


            [Display(Name = "用户昵称")]
            public string Nickname { get; set; }

            [Display(Name = "性别")]
            public Nullable<int> Sex { get; set; }

            public Nullable<System.DateTime> RegisterDate { get; set; }
            [Display(Name = "头像")]
            public string UserLogo { get; set; }
            //[DataType(DataType.Password)]
            //[Display(Name = "确认密码")]
            //[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
            //public string ConfirmPassword { get; set; }
        }
     
}