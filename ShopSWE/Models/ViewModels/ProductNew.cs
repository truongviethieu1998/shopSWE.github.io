using ShopSWE.Models.Enity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSWE.Models.ViewModels
{
    public class ProductNew
    {
        SHOPSWEEntities db = null;
        public ProductNew()
        {
            db = new SHOPSWEEntities();
        }
        public List<Sanpham1> ListNewProduct(int top)
        {
            return db.Sanpham1.OrderByDescending(x => x.ID).Take(top).ToList();
        }
        public List<Sanpham1> BestsellerProduct(int top)
        {
            return db.Sanpham1.Where(x=>x.BestSeller != null).OrderByDescending(x => x.ID).Take(top).ToList();
        }
    }
}