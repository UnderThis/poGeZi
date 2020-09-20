
using mvcProject.Areas.Admin.Models;
using mvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcProject.Areas.Admin.Controllers
{
    public class aboutPostController : Controller
    {
        Wang conn = new Wang();
        //
        // GET: /Admin/aboutPost/

        public ActionResult Index()
        {
            return View();
        }
        #region 板块管理
        public ActionResult boardManager()
        {
            System.Collections.Generic.List<BOARD> aa = conn.BOARD.ToList();
            ViewBag.board = aa;
            return View();
        }
        [HttpPost]
        public ActionResult boardAdd([System.Web.Http.FromBody]boardManager model)
        {
            if (ModelState.IsValid)
            {
                BOARD bo = new BOARD();
                bo.B_NAME = model.B_NAME;
                bo.B_CONTENTS = model.B_CONTENTS;
                conn.BOARD.Add(bo);
                conn.SaveChanges();
                return RedirectToAction("boardManager");
            }
            return View();
        }
        [HttpPost]
        public ActionResult boardMod([System.Web.Http.FromBody]boardManager model, int id)
        {
            if (ModelState.IsValid)
            {
                 
                try
                {
                    BOARD aa = conn.BOARD.First(r => r.B_NAME == model.B_NAME);
                    return Content("<script>alert('有重复的名字');history.go(-1)</script>");
                }
                catch (Exception ex)
                {
                    BOARD board = (from a in conn.BOARD
                                   where a.B_ID == id
                                   select a).Single();
                    board.B_NAME = model.B_NAME;
                    board.B_CONTENTS = model.B_CONTENTS;
                    conn.Entry<BOARD>(board).State = System.Data.EntityState.Modified;
                    conn.SaveChanges();

                    return Content("<script>alert('修改成功');history.go(-1)</script>");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult boardDel(int id)
        {
            BOARD board = conn.BOARD.Find(id);
            conn.BOARD.Remove(board);
            conn.SaveChanges();
            return View();
        }
        
        #endregion

        #region 帖子管理
        public ActionResult postManage(string kw)
        {

            System.Collections.Generic.List<BOARD> aa = conn.BOARD.ToList();
            ViewBag.board = aa;



            return View();
        }

        public ActionResult postBoard(string kw, string sw, string dw)
        {

            var model = conn.POST.OrderByDescending(x => x.P_DATETIME).ToList();
            if (!string.IsNullOrEmpty(kw))
            {
                if (kw == "all")
                    return PartialView("~/Areas/Admin/Views/aboutPost/postBoard.cshtml", model);
                else
                    model = model.Where(x => x.P_BOARDNAME.Contains(kw)).ToList();
            }
            if (!string.IsNullOrEmpty(sw))
            {
                model = conn.POST
                        .Where(c => c.P_Title.Contains(sw) || c.P_CONTENTS.Contains(sw) || c.P_POSTER.Contains(sw) || c.P_BOARDNAME.Contains(sw))
                        .OrderByDescending(a => a.P_CLICK)
                        .ToList();
            }
            if (!string.IsNullOrEmpty(dw))
            {
                int id = System.Convert.ToInt32(dw);
                POST post = conn.POST.Find(id);
                conn.POST.Remove(post);
                conn.SaveChanges();
            }
            return PartialView("~/Areas/Admin/Views/aboutPost/postBoard.cshtml", model);
        }


        public ActionResult Apost(int id)
        {
            var postList = conn.POST.FirstOrDefault(r => r.P_ID == id);
            var commentList = (from a in conn.POSTCOMMENT
                               where a.PC_POSTID == id
                               select a).ToList();
            ViewBag.post = postList;
            ViewBag.comment = commentList;
            return View();
        }

        public ActionResult pDelete(string id, string co)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int pid = System.Convert.ToInt32(id);
                POST post = conn.POST.Find(pid);
                conn.POST.Remove(post);
                conn.SaveChanges();
            }
            if (!string.IsNullOrEmpty(co))
            {
                int cid = System.Convert.ToInt32(co);
                POSTCOMMENT postcomment = conn.POSTCOMMENT.Find(cid);
                conn.POSTCOMMENT.Remove(postcomment);
                conn.SaveChanges();
            }
            return View();
        }


    

        #endregion

        

    }
}
