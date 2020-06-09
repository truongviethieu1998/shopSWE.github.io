using ShopSWE.Models.Enity;
using ShopSWE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Configuration;
using System.IO;


namespace ShopSWE.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public SHOPSWEEntities db = new SHOPSWEEntities();
        public ActionResult Index()
        {
            var Product = new ProductNew();
            ViewBag.ProductNew = Product.ListNewProduct(12);
            ViewBag.ProductBestSeller = Product.BestsellerProduct(12);
            return View();
        }
        //[OutputCache(Duration = int.MaxValue,VaryByParam = "ID",Location = System.Web.UI.OutputCacheLocation.Server)]
        [OutputCache(CacheProfile = "Cache1DayProductDetail")]
        public ActionResult Detail(int ID)
        {
            Sanpham1 sanpham1 = db.Sanpham1.Find(ID);
            if (sanpham1 == null)
            {
                return HttpNotFound();
            }
            return View(sanpham1);
        }
        public ActionResult AddToCart(int ID)
        {
            if (Session["giohang"] == null)
            {
                giohang a = new giohang();
                var sp = db.Sanpham1.Where(x => x.ID == ID).Single();
                a.ID = sp.ID;
                a.Name = sp.Name;
                a.Price = sp.Price;
                a.Img = sp.Img;
                a.soluong = 1;
                List<giohang> giohangs = new List<giohang>();
                giohangs.Add(a);
                Session["giohang"] = giohangs;
            }
            else
            {
                List<giohang> giohangs = (List<giohang>)Session["giohang"];
                var test = giohangs.Find(x => x.ID == ID);
                if (test == null)
                {
                    giohang a = new giohang();
                    var sp = db.Sanpham1.Where(x => x.ID == ID).Single();
                    a.ID = sp.ID;
                    a.Name = sp.Name;
                    a.Price = sp.Price;
                    a.Img = sp.Img;
                    a.soluong = 1;
                    giohangs.Add(a);
                }
                else
                {
                    test.soluong++;
                }
                Session["giohang"] = giohangs;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int ID)
        {
            List<giohang> giohangs = (List<giohang>)Session["giohang"];
            var test = giohangs.Find(x => x.ID == ID);
            if (test != null)
            {
                giohangs.Remove(test);
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            Session["giohang"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult Pay()
        {
            return View();
        }
        public ActionResult AddOrder(string ShipName, string ShipMobile, string ShipEmail, string ShipAddress)
        {
            int orderID = 0;
            List<giohang> giohangs = (List<giohang>)Session["giohang"];
            Order1 objOrder = new Order1()
            {
                ShipName = ShipName,
                ShipAddress = ShipAddress,
                ShipEmail = ShipEmail,
                ShipMobile = ShipMobile,
                CreateDate = DateTime.Now
            };
            db.Order1.Add(objOrder);
            db.SaveChanges();
            orderID = objOrder.OrderID;

            List<OrderDetail> objOrderDetails = new List<OrderDetail>();
            foreach (var item in giohangs)
            {
                //OrderDetail oD1 = new OrderDetail();
                //oD1.Quantity = item.soluong;
                //oD1.Name = item.Name;
                //oD1.SanphamID = item.ID;
                //oD1.OrderID = orderID;
                //oD1.Price = item.Price;
                //oD1.TotalPrice = item.Price * item.soluong;
                //objOrderDetails.Add(oD1);

                //OrderDetail oD = new OrderDetail()
                //{
                //    Name = item.Name,
                //    Quantity = item.soluong,
                //    SanphamID = item.ID,
                //    OrderID = orderID,
                //    Price = item.Price,
                //    TotalPrice = item.Price * item.soluong
                //};
                //objOrderDetails.Add(oD);
                ////
                objOrderDetails.Add(new OrderDetail
                {
                    Quantity = item.soluong,
                    Name = item.Name,
                    SanphamID = item.ID,
                    OrderID = orderID,
                    Price = item.Price,
                    TotalPrice = item.Price * item.soluong
                });
            }

            //save vào db
            db.OrderDetails.AddRange(objOrderDetails.AsEnumerable());
            db.SaveChanges();


            //gửi email
            //string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/client/template/email.html"));
            //content = content.Replace("{{CustomerName}}", ShipName);
            //content = content.Replace("{{Phone}}", ShipMobile);
            //content = content.Replace("{{Email}}", ShipEmail);
            //content = content.Replace("{{Address}}", ShipAddress);

            //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            //new MailHelper().SendMail(ShipEmail, "Đơn hàng mới từ SWE SHOP", content);
            //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ SWE SHOP", content);
            int total = 0;
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/client/template/email.html"));
            foreach (giohang item in giohangs)
            {
                content = content.Replace("{{NameProduct}}", item.Name);
                content = content.Replace("{{soluong}}", item.soluong.ToString());
                string.Format("{0:#,##0}", total += item.soluong * item.Price);
            }
            string recipient = Request["ShipEmail"];
            content = content.Replace("{{CustomerName}}", ShipName);
            content = content.Replace("{{Phone}}", ShipMobile);
            content = content.Replace("{{Email}}", ShipEmail);
            content = content.Replace("{{Address}}", ShipAddress);
            content = content.Replace("{{Total}}", total.ToString());

            WebMail.SmtpServer = "smtp.gmail.com";
            WebMail.SmtpPort = 587;
            WebMail.SmtpUseDefaultCredentials = true;
            WebMail.EnableSsl = true;
            WebMail.UserName = "truongviethieu98@gmail.com";
            WebMail.Password = "truongviethieu";
            WebMail.Send(to: recipient, subject: "Đơn Hàng Từ SWE", body: content);
            Session["giohang"] = null;
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult AddOrder(Order1 data2, OrderDetail[] arr)
        //{
        //    int orderID = 0;
        //    data2.CreateDate = DateTime.Now;           
        //    db.Order1.Add(data2);
        //    db.SaveChanges();
        //    orderID = data2.OrderID;

        //    foreach (OrderDetail item in arr)
        //    {
        //        item.OrderID = orderID;
        //    }
        //    db.OrderDetails.AddRange(arr.AsEnumerable());
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult SuccessOrder()
        {
            //Order1 order = db.Order1.Find(OrderID);
            //if (order == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(order);
            return View();
        }
        public ActionResult SearchItem(string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<Sanpham1> searchItem = db.Sanpham1.Where(e => e.Name.Contains(productName)).ToList();
                return View(searchItem);
            }
            catch
            {
                return RedirectToAction("ActionDenied", "Common");
            }
        }
        public ActionResult Top()
        {
            return View();
        }
        public PartialViewResult PaggingTop(int pageIndex = 1, int pageSize = 24)
        {
            SanphamViewModels model = new SanphamViewModels();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Sanpham1.Where(x => x.Category == 0).OrderByDescending(x => x.ID);
            model.sanphams = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        public ActionResult Outerwear()
        {
            return View();
        }
        public PartialViewResult PaggingOuterwear(int pageIndex = 1, int pageSize = 24)
        {
            SanphamViewModels model = new SanphamViewModels();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Sanpham1.Where(x => x.Category == 1).OrderByDescending(x => x.ID);
            model.sanphams = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        public ActionResult Bottom()
        {
            return View();
        }
        public PartialViewResult PaggingBottom(int pageIndex = 1, int pageSize = 24)
        {
            SanphamViewModels model = new SanphamViewModels();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Sanpham1.Where(x => x.Category == 2).OrderByDescending(x => x.ID);
            model.sanphams = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        public ActionResult Accessories()
        {
            return View();
        }
        public PartialViewResult PaggingAccessories(int pageIndex = 1, int pageSize = 24)
        {
            SanphamViewModels model = new SanphamViewModels();
            int upper = (pageIndex - 1) * pageSize;
            var emps = db.Sanpham1.Where(x => x.Category == 3).OrderByDescending(x => x.ID);
            model.sanphams = emps.Skip(upper).Take(pageSize).ToList();
            model.pageIndex = pageIndex;
            model.pageSize = pageSize;
            model.TotalRecord = emps.Count();
            decimal totalPage = (decimal)model.TotalRecord / model.pageSize;
            model.TotalPage = decimal.ToInt32(Math.Ceiling(totalPage));
            return PartialView(model);
        }
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}