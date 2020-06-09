using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopSWE.Models.Enity;

namespace ShopSWE.Models.ViewModels
{
    public class giohang
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Img { get; set; }
        public int soluong { get; set; }
    }
}