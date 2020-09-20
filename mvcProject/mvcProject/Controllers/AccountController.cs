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
    
    public class AccountController : Controller
    {

        Wang conn = new Wang();

        [HttpGet]
        public ActionResult Account()
        {
            
            return View();
        }

        //
        // GET: /Account/
        [HttpPost]
        public ActionResult Account(Models.LoginModel model)
        {

            var script = string.Format("alert('aaa');" );
            
            if (ModelState.IsValid)
            {
                var admin = conn.adminUser.FirstOrDefault(a => a.userName == model.Accounts);
                var select = conn.Users.FirstOrDefault(r => r.Accounts == model.Accounts);

                if (select == null)
                {
                    if (admin == null || admin.password != model.Password)

                        //return Content("账号或密码错误"); 
                        return Content("<script>alert('账号或密码错误');history.go(-1);location.reload();</script>", "text/html");

                    else if (admin.userName == model.Accounts && admin.password == model.Password)
                    {
                        Session["adminName"] = admin.userName;
                        Session["adminID"] = admin.ID;
                        return RedirectToAction("Index", "Admin", new { area="Admin"});
                    }
                }
                else if (select.Accounts == model.Accounts && select.Password == model.Password)
                {
                     
                    Session["userName"] = select.Nickname;
                    Session["userLogo"] = select.UserLogo;
                    Session["userAccounts"] = model.Accounts;
                    Session["userID"] = select.ID;
                    return RedirectToAction("Index", "Home");
                }
                    
                else if (select.Password != model.Password)
                    //return Content("账号或密码错误");
                
                    
                    return Content("<script>alert('账号或密码错误');history.go(-1);location.reload();</script>", "text/html");
                 
                
            }
            return Content("数据有误");
            //var id = Request["username"];
            //var password = Request["password"];
            //var select = conn.Users.FirstOrDefault(r => r.Accounts == id);
             
            //if (select == null)
            //{
            //    return View();
            //}
            //else if (select.Accounts == id &&select.Password == password)
            //{
            //    return Content("zhuce");
            //}
            //else
            //{
            //    return Content("账号密码错误");
            //}

             
             
        }
        public ActionResult Register()
        {
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model )
        {
            
            string ss;
            string pa = "~/unload/userLogo/";
            ss ="U"+ System.DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(Request.Files["file"].FileName);
            var jp = Path.GetExtension(Request.Files["file"].FileName);
            Request.Files["file"].SaveAs(System.Web.HttpContext.Current.Server.MapPath(pa) + ss);
            if (ModelState.IsValid)
            {
                Users post1 = conn.Users.FirstOrDefault(a => a.Accounts == model.UserName);
                if (post1 == null)
                {
                    // 尝试注册用户
                    Users user = new Users();
                    user.Accounts = model.UserName;
                    user.Password = model.Password;
                    user.E_mail = model.E_mail;
                    user.Sex = model.Sex;
                    user.Nickname = model.Nickname;
                    user.UserLogo = jp == "" ? "old.jpg" : ss;
                    user.RegisterDate = DateTime.Parse(DateTime.Now.ToString());
                    conn.Users.Add(user);
                    conn.SaveChanges();

                    return Content("<script>alert('你已注册成功');window.location.href='/Account/Account'</script>");
                }
                else
                    return Content("该账户已被注册");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

     











        #region 帮助程序
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

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion
    }
}
