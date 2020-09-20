using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace mvcProject.Areas.Admin.Models
{
    public class boardManager
    {
        public int B_ID { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = " 至少包含 {2} 个字,不能多于{1}。", MinimumLength = 1)]
        public string B_NAME { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = " 至少包含 {2} 个字,不能多于{1}。", MinimumLength = 1)]
        public string B_CONTENTS { get; set; }

        public Nullable<int> B_NUMBER { get; set; }
    }
}