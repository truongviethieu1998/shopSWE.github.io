using ShopSWE.Models.Enity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopSWE.Models.ViewModels
{
    public class SanphamViewModels
    {
        public List<Sanpham1> sanphams { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
    }
}