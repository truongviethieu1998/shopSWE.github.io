using Dapper;
using ShopSWE.Models.Enity;
using ShopSWE.Models.ViewModels;
using ShopSWE.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopSWE.Areas.Admin.Controllers
{
    public class danhsachsanphamController : BaseController
    {
        // GET: Admin/danhsachsanpham
        public SHOPSWEEntities db = new SHOPSWEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Pagging(int pageIndex = 1, int pageSize = 20)
        {
            SanphamViewModels model = new SanphamViewModels();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Sanpham1.OrderBy(x => x.ID);
            model.sanphams = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult Add(Sanpham1 sanpham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add vào db
                    db.Sanpham1.Add(sanpham);
                    db.SaveChanges();
                    return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Tên input, lỗi
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    return Json(new { StatusCode = 500, Message = allErrors.FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { StatusCode = 505, Message = "Lỗi Thêm Mới" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit(int ID = 0)
        {
            Sanpham1 sanpham1 = db.Sanpham1.Find(ID);
            if (sanpham1 == null)
            {
                return HttpNotFound();
            }
            return View(sanpham1);
        }
        [HttpPost]
        public ActionResult Edit(Sanpham1 sanpham1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanpham1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanpham1);
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            bool result = false;
            var u = db.Sanpham1.Where(x => x.ID == ID).FirstOrDefault();
            if (u != null)
            {
                db.Sanpham1.Remove(u);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int ID)
        {
            Sanpham1 sanpham1 = db.Sanpham1.Find(ID);
            if (sanpham1 == null)
            {
                return HttpNotFound();
            }
            return View(sanpham1);
        }
        public ActionResult Donhang()
        {
            return View();
        }
        public PartialViewResult PaggingOrder(int pageIndex = 1, int pageSize = 20)
        {
            OrderViewModel model = new OrderViewModel();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Order1.OrderBy(x => x.OrderID);
            model.orders = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        public ActionResult DetailDonHang(int? OrderID)
        {
            if (OrderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<OrderDetail> orderDetail = db.OrderDetails.Where(e => e.OrderID == OrderID).ToList();
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }
        public ActionResult SearchName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }                
                List<Order1> searchName = db.Order1.Where(e => e.ShipName.Contains(Name)).ToList();
                return View(searchName);
            }
            catch
            {
                return RedirectToAction("ActionDenied", "Common");
            }
        }
    }
}