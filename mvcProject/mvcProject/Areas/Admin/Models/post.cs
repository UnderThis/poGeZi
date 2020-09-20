using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace mvcProject.Areas.Admin.Models
{
    public class aWrite
    {
        Wang rp = new Wang();
        [Required]
        [StringLength(10, ErrorMessage = " 至少包含 {2} 个字,不能多于{1}。", MinimumLength = 1)]
        public string N_Title { get; set; }


         

        [Required]
        [StringLength(500, ErrorMessage = "至少包含 {2} 个字,不能多于{1}。", MinimumLength = 15)]
        [DataType(DataType.MultilineText)]
        public string N_CONTENTS { get; set; }

        public string picture { get; set; }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            var selectList = rp.BOARD.Select(a => new SelectListItem
            {
                Text = a.B_NAME,
                Value = a.B_ID.ToString(),
            });
            return selectList;
        }
    }
}