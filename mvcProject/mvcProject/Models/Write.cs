using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace mvcProject.Models
{
    public class Write
    {
        Wang rp = new Wang();
        [Required]
        [StringLength(10, ErrorMessage = " 至少包含 {2} 个字,不能多于{1}。", MinimumLength = 1)]
        public string P_Title { get; set; }

        
        
        public string B_NAME { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "至少包含 {2} 个字,不能多于{1}。", MinimumLength = 15)]
        [DataType(DataType.MultilineText)]
        public string P_CONTENTS { get; set; }

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