using ShopSWE.Models.Enity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSWE.Models.ViewModels
{
    [Serializable]
    public class CartItem
    {
        public Sanpham1 sanpham{ get; set; }
        public int Quantity { get; set; }

    }
}