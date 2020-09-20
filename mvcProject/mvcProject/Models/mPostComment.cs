using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcProject.Models
{
    public class mPostComment
    {
        public int PC_ID { get; set; }
        public Nullable<int> PC_BOARDID { get; set; }
        public Nullable<int> PC_POSTID { get; set; }
        public string PC_COMMENTER { get; set; }
        public string PC_PICTURE { get; set; }
        public string PC_CONTENTS { get; set; }
        public string PC_EMOJI { get; set; }
        public Nullable<int> PC_GOOD { get; set; }
        public Nullable<int> PC_BAD { get; set; }
        public Nullable<System.DateTime> PC_DATETIME { get; set; }
    }
}