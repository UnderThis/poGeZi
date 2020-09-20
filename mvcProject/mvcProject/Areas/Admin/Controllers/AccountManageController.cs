using mvcProject.Areas.Admin.Models;
using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcProject.Areas.Admin.Controllers
{
    public class AccountManageController : Controller
    {
        Wang conn = new Wang();
        //
        // GET: /Admin/Account/

        #region 用户账号
        public ActionResult Index(string id)
        {
            var model = conn.Users.OrderByDescending(x => x.RegisterDate).ToList();
            return View(model);
        }

        public ActionResult userManage(string name, string id)
        {
            var model = conn.Users.OrderByDescending(x => x.RegisterDate).ToList();
            if (!string.IsNullOrEmpty(name))
                model = model.Where(w => w.Accounts.Contains(name)).ToList();
            if (!string.IsNullOrEmpty(id))
            {
                int uid = System.Convert.ToInt32(id);
                Users post = conn.Users.Find(uid);
                conn.Users.Remove(post);
                conn.SaveChanges();
            }
            return PartialView("~/Areas/Admin/Views/AccountManage/userManage.cshtml", model);
        }


        public ActionResult userInfo(int id)
        {
            Session["uid"] = id;
            return View();
        }   

        public ActionResult info( )
        {
            int uid = (int)Session["uid"];
            var model = conn.Users.FirstOrDefault(a => a.ID == uid);
            return View("~/Areas/Admin/Views/AccountManage/info.cshtml", model);
        }

        public ActionResult uPost()
        {
            int uid =(int)Session["uid"];
            var model = conn.POST.OrderByDescending(a => a.P_DATETIME).Where(a => a.P_POSTERID == uid);
            return View(model);
        }
        #endregion

        public ActionResult adminIndex()
        {
             
            return View( );
            
        }
        [HttpPost]
        public ActionResult adminIndex([System.Web.Http.FromBody]adminAddModel model)
        {
            adminUser bo = new adminUser();
            bo.userName = model.Account;
            bo.password = model.Password;
            conn.adminUser.Add(bo);
            conn.SaveChanges();
            return View();
            
        }
        public ActionResult aManage(string name, string id)
        {
            var model = conn.adminUser.OrderByDescending(x => x.ID).ToList();
            if (!string.IsNullOrEmpty(name))
                model = model.Where(w => w.userName.Contains(name)).ToList();
            if (!string.IsNullOrEmpty(id))
            {
                int uid = System.Convert.ToInt32(id);
                adminUser post = conn.adminUser.Find(uid);
                conn.adminUser.Remove(post);
                conn.SaveChanges();
            }
            return PartialView("~/Areas/Admin/Views/AccountManage/aManage.cshtml", model);
        }

    }
}
