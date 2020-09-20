using mvcProject.Areas.Admin.Models;
using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace mvcProject.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        Wang conn = new Wang();
        //
        // GET: /Admin/Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult notice( )
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult notice(aWrite model)
        {
            string aName=Session["adminName"].ToString();
            int aID=(int)Session["adminID"];
            string ss;
            string pa = "~/unload/Notice/";
            ss ="N"+ System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);
            var pp = Path.GetExtension(Request.Files["file"].FileName);
            Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + ss);
            if (ModelState.IsValid)
            {
                NOTICE nt = new NOTICE();
                nt.N_CLICK = 0;
                nt.N_CONTENTS = model.N_CONTENTS;
                nt.N_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                nt.N_PICTURE = pp == "" ? "" : ss;
                nt.N_POSTER = aName;
                nt.N_POSTERID = aID;
                nt.N_Title = model.N_Title;
                conn.NOTICE.Add(nt);
                conn.SaveChanges();
                return Content("<script>alert('公告发布成功'); history.go(-1)</script>");
            }
            return View();
        }

        public ActionResult noticeManager()
        {
            var aa = conn.NOTICE.OrderByDescending(r => r.N_DATETIME).ToList();
            return View(aa);
        }
        
        public ActionResult aNotice(int noticeID)
        {
            var postList = conn.NOTICE.FirstOrDefault(r => r.N_ID == noticeID);
            ViewBag.post = postList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult aNotice(aWrite write,int noticeID)
        {
            var id = noticeID;
            NOTICE list = conn.NOTICE.FirstOrDefault(a => a.N_ID == id);
            string ss;
            ss = "N" + System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);

            NOTICE notice = (from a in conn.NOTICE
                         where a.N_ID == id
                         select a).Single();
            notice.N_Title = write.N_Title;
            notice.N_CONTENTS = write.N_CONTENTS;
            notice.N_PICTURE = img(ss, id);
            conn.Entry<NOTICE>(notice).State = System.Data.EntityState.Modified;
            conn.SaveChanges();
            return Content("<script>alert('修改成功');history.go(-1)</script>");
        }
        [HttpPost]
        public ActionResult aDelete(int Id)
        {
            NOTICE notice = conn.NOTICE.Find(Id);
            conn.NOTICE.Remove(notice);
            conn.SaveChanges();
            return View();
        }

        #region img
        private string img(string createStatus,int id)
        {
            var list = conn.NOTICE.FirstOrDefault(a => a.N_ID == id);
            var url = list.N_PICTURE;
            var jp = Path.GetExtension(Request.Files["file"].FileName);
            if (jp == "")
            {
                return url;
            }
            else
            {
                string pa = "~/unload/Notice/";
                Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + createStatus);

                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(pa) + url);
                
                return createStatus;
            } 
        #endregion
        }
    }
}
