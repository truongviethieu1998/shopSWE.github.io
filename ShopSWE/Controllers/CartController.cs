using ShopSWE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopSWE.Controllers
{
    public class CartController : Controller
    {
        private const string Cartsession = "Cartsession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[Cartsession];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(int sanphamID, int quantity)
        {
            var cart = Session[Cartsession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.sanpham.ID == sanphamID))
                {
                    foreach (var item in list)
                    {
                        if (item.sanpham.ID == sanphamID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng
                    var item = new CartItem();
                    item.sanpham.ID = sanphamID;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //gán vào session
                Session[Cartsession] = list;
            }
            else
            {
                //tạo mới đối tượng
                var item = new CartItem();
                item.sanpham.ID = sanphamID;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                //gán vào session
                Session[Cartsession] = list;
            }
            return RedirectToAction("Index");
        }
    }
}