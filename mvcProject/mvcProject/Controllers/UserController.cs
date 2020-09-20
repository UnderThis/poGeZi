using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mvcProject.Models;
using System.Web.Security;
using WebMatrix.Data;
using WebMatrix.WebData;
using System.Web.Mvc;
using System.IO;


namespace mvcProject.Controllers
{ 
    public class UserController : Controller
    {
        Wang conn = new Wang();
        //
        // GET: /User/

        #region 个人信息
        public ActionResult Index()
        {
            var ID = (int)Session["userID"];
            var list = conn.Users.FirstOrDefault(a => a.ID == ID);
            ViewBag.user = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterModel model)
        {
            var id = Session["userAccounts"];
            Users list = conn.Users.FirstOrDefault(a => a.Accounts == id);
            string ss;
            ss = "P" + System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);



            Users user = (from a in conn.Users
                          where a.Accounts == id
                          select a).Single();
            user.Nickname = model.Nickname;
            user.UserLogo = img(ss);
            conn.Entry<Users>(user).State = System.Data.EntityState.Modified;
            conn.SaveChanges();
            return Content("<script>alert('修改成功');history.go(-1)</script>");
        }
        
        #endregion
        #region 发帖管理
        public ActionResult postManage( string aid)
        {
             if (!string.IsNullOrEmpty(aid))
            {
                int ida = System.Convert.ToInt32(aid);
                var lista = conn.POST.OrderByDescending(r => r.P_DATETIME).Where(r => r.P_POSTERID == ida).ToList();
                return View(lista);
            }
            var id = (int)Session["userID"];
                var list = conn.POST.OrderByDescending(r => r.P_DATETIME).Where(r => r.P_POSTERID == id).ToList();
                return View(list);
            
            
        }


        public ActionResult userPost(int postId)
        {
            var postList = conn.POST.FirstOrDefault(r => r.P_ID == postId);
            var commentList = (from a in conn.POSTCOMMENT
                               where a.PC_POSTID == postId
                               select a).ToList();
            ViewBag.post = postList;
            ViewBag.comment = commentList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult userPost(Write write, int postId)
        {
            var id = postId;
            POST list = conn.POST.FirstOrDefault(a => a.P_ID == id);
            string ss;

            ss = "P" + System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);

            POST post = (from a in conn.POST
                         where a.P_ID == id
                         select a).Single();
            post.P_Title = write.P_Title;
            post.P_CONTENTS = write.P_CONTENTS;
            post.P_PICTURE = img1(ss, id);
            conn.Entry<POST>(post).State = System.Data.EntityState.Modified;
            conn.SaveChanges();
            return Content("<script>alert('修改成功');history.go(-1)</script>");
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            POST post = conn.POST.Find(Id);
            conn.POST.Remove(post);
            conn.SaveChanges();
            return View();
        }
        
        #endregion
        #region 账号管理
        public ActionResult userManage()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult userManage(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var id = Session["userAccounts"];
                try
                {
                    Users user = (from a in conn.Users
                                  where a.Accounts == id && a.Password == model.OldPassword
                                  select a).Single();
                    user.Password = model.NewPassword;
                    conn.Entry<Users>(user).State = System.Data.EntityState.Modified;
                    conn.SaveChanges();
                    return Content("<script>alert('修改成功');history.go(-1)</script>");

                }
                catch (Exception ex2)
                {
                    return Content("<script>alert('密码错误');history.go(-1)</script>");
                }
            }
            return View();
        }

        
        #endregion

        #region 图片
        private string img(string createStatus)
        {
            var aa = Session["userAccounts"];
            var list = conn.Users.FirstOrDefault(a => a.Accounts == aa);

            var url = list.ToString();
            var jp = Path.GetExtension(Request.Files["file"].FileName);
            if (jp == "")
            {
                return url;
            }
            else
            {
                string pa = "~/unload/userLogo/";
                Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + createStatus);
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(pa) + url);
                return createStatus;
            }
        }
        private string img1(string createStatus,int id)
        {
            var list = conn.POST.FirstOrDefault(a => a.P_ID == id);
            var url = list.P_PICTURE;
            var jp = Path.GetExtension(Request.Files["file"].FileName);
            if (jp == "")
            {
                return url;
            }
            else
            {
                string pa = "~/unload/postPicture/";
                Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + createStatus);
                return createStatus;
            }
        }  
        #endregion
    }
}
