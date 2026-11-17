using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _25DH110915_LTW.Models;
using PagedList.Mvc; 
using PagedList;     

namespace _25DH110915_LTW.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }

        // Thêm các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } // Trang hiện tại
        public int PageSize { get; set; } = 10; // Số sản phẩm mỗi trang (mặc định 10)

        // Đổi List<Product> thành PagedList.IPagedList<Product>
        public IPagedList<Product> Products { get; set; }
    }
}