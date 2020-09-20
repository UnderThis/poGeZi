using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace mvcProject.Models
{ 
    
    public class mpost
    { 
        
        public int P_ID { get; set; }
        public int P_BOARDID { get; set; }
        public string P_POSTER { get; set; }
        public string P_PICTURE { get; set; }
 
        public string P_Title { get; set; }
        public string P_CONTENTS { get; set; }
        public string p_EMOJI { get; set; }
        public Nullable<System.DateTime> P_DATETIME { get; set; }
        public int P_CLICK { get; set; }
        public string P_BOARDNAME { get; set; }
        
    }
}
