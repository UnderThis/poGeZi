//using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using System.Web.Mvc;
using mvcProject.Models;
using System.Web.Security;
using WebMatrix.Data;
using WebMatrix.WebData;
using mvcProject.DAL;
using System.Collections;
using System.Data;
using System.IO;

namespace mvcProject.Controllers
{
    
    public class HomeController : Controller 
    {
        private Wang conn = new Wang();
        private mpost mp = new mpost();
        public int Pid { get; set; }
         

         
        
        //
        // GET: /Test/  
        
        //
        // GET: /Home/


        public ActionResult Index(string type)
        {
            
            var userid = Session["userAccount"];

            var board = conn.BOARD.ToList();
            ViewBag.board = board;
            #region 也是查询
            //System.Collections.Generic.List<POST> aa = conn.POST.ToList();    
            #endregion

           
            var aa =  conn.POST.OrderByDescending(r=>r.P_DATETIME).ToList();
            if (type == null||type=="all"){
                return View(aa);
            }
            else if (type == "click")
            {
                var dt = DateTime.Now.AddDays(-14);
                var bb = (from a in conn.POST
                         where a.P_DATETIME > dt && a.P_CLICK > 100
                         orderby a.P_CLICK
                         select a).ToList();
                return View(bb);
            }
            else
            {
                int bid=System.Convert.ToInt32(type) ;
                var bb = from a in conn.POST
                         where a.P_BOARDID == bid
                         select a;
                return View(bb);

            }
            
        }
         
        [HttpPost]  
        public ActionResult Index()
        {  

            var search=Request["Text1"];
            var prg = conn.POST
                .Where(c => c.P_Title.Contains(search) || c.P_CONTENTS.Contains(search))
                .OrderByDescending(a => a.P_CLICK)
                .ToList();
            
           
            return View(prg);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

   
     
        public ActionResult Post(int postId)
        {

            
            //var all=new Actio();
            //var post = new newPost();

            mpost mp = new mpost();
            mp.P_ID = postId;
            conn.Configuration.AutoDetectChangesEnabled = false;
           

            //all.postInfo = post.GetListByPost();

            var pro = conn.POST.FirstOrDefault(r => r.P_ID == postId);

            var proc = conn.POSTCOMMENT.Where(r => r.PC_POSTID == postId).OrderByDescending(r => r.PC_ID);


            //POSTCOMMENT post1 = conn.POSTCOMMENT.Find(postId);  也是一种查询

              
            var list = new mpost()
            {
              P_ID = pro.P_ID,
              P_DATETIME=pro.P_DATETIME,
              P_BOARDID = (int)pro.P_BOARDID,
              P_BOARDNAME = pro.P_BOARDNAME,
              P_CLICK = (int)pro.P_CLICK,
              P_CONTENTS = pro.P_CONTENTS,
              p_EMOJI = pro.p_EMOJI,
              P_PICTURE = pro.P_PICTURE,
              P_POSTER = pro.P_POSTER,
              P_Title = pro.P_Title,
              
            };



            POST p = new POST();
            int click = (int)pro.P_CLICK + 1;
            #region 也是一种增加
            //conn.Set(pro).Attach(POST);
            //p.P_CLICK = click;
            //p.P_ID = postId;
            //conn.Entry<POST>(p).State = System.Data.EntityState.Modified;  
            //conn.SaveChanges();    
            #endregion

            ViewBag.post = list;

            //新增 由于被ef跟踪
            using (var db = new Wang())
            {
                var pidd = db.POST.Where(aa => aa.P_ID == postId).FirstOrDefault();

                pidd.P_CLICK = click;
                db.SaveChanges();
            }


            HttpCookie cook = new HttpCookie("b_id", pro.P_BOARDID.ToString());
            HttpCookie cook1 = new HttpCookie("id", pro.P_ID.ToString());
            Response.Cookies.Add(cook);
            Response.Cookies.Add(cook1);
           
             




            return View(proc);

             
             
        }


       
        [HttpPost]
        public ActionResult postcomment()
        {

            var id = Session["userID"];
            string comment = Request["comment"];
            var a = Convert.ToInt32(Request.Cookies["id"].Value);
            var b = Convert.ToInt32(Request.Cookies["b_id"].Value);
            var name = Session["userName"].ToString();
            string logo=Session["userLogo"].ToString();
            POSTCOMMENT comm = new POSTCOMMENT();

            comm.PC_COMMENTERID = (int)id;
            comm.PC_COMMENTER = name; 
            comm.PC_PICTURE = logo;
            comm.PC_POSTID = a;
            comm.PC_BOARDID = (int)b;
            comm.PC_CONTENTS = comment;
            comm.PC_DATETIME = DateTime.Parse(DateTime.Now.ToString());
            comm.PC_BAD = 0;
            comm.PC_GOOD = 0;   
            conn.POSTCOMMENT.Add(comm);
            conn.SaveChanges();


            return Content("<script>history.go(-1)</script>");
        }

      
        public ActionResult Write()
        {
            Write write = new Write();
            GenerateReadyTimeViewData();
            return View(write);
        }
        private void GenerateReadyTimeViewData()
        {
            ViewData["boardId"] = GetTimeHourList();
        }
        private List<SelectListItem> GetTimeHourList()
        {
            List<SelectListItem> hourList = new List<SelectListItem>();
            System.Collections.Generic.List<BOARD> aa = conn.BOARD.ToList();
            for (int i = 0; i < aa.Count; i++)
            {
                hourList.Add(new SelectListItem { Text = aa[i].B_NAME, Value = aa[i].B_ID.ToString()});
            }

            return hourList;
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Write(Write model)
        {
            var a = Request["count"];
            var id = Session["userID"];
            string ss;
            string pa = "~/unload/postPicture/";
            ss ="p"+ System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);
            var pp = Path.GetExtension(Request.Files["file"].FileName);
            Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + ss);

            if (ModelState.IsValid)
            {
                int na = System.Convert.ToInt32(model.B_NAME);
                var bn = conn.BOARD.FirstOrDefault(c => c.B_ID == na);
                POST post = new POST();
                post.P_BOARDID = na;
                post.P_Title = model.P_Title;
                post.P_PICTURE = pp == "" ? "" : ss;
                post.P_CONTENTS = model.P_CONTENTS;
                post.P_POSTERID =(int)id;
                post.P_POSTER = Session["userName"].ToString();
                post.P_CLICK = 0;
                post.P_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                post.P_BOARDNAME = bn.B_NAME;
                conn.POST.Add(post);
                conn.SaveChanges();
                return Content("<script>alert('你已发帖成功');window.location.href='/Home/Write'</script>");
            }

            return View(model);
        }

    }
}
