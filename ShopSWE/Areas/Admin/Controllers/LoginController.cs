using ShopSWE.Views.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopSWE.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Index(LoginViewModel model)
        {
            try
            {
                if (model.Email == "truongviethieu" && model.Password == "123456")
                {
                    Session["UserInformation"] = "truongviethieu123456";
                    return Json(new { Code = 200 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Code = 500, ErrorMessage = "Đăng nhập thất bại" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Code = 500, ErrorMessage = "Đăng nhập thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}