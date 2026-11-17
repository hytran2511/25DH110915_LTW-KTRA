using System;
using System.Collections.Generic;
using _25DH110915_LTW.Models;
using PagedList;

namespace _25DH110915_LTW.Models.ViewModel
{
    public class HomeProductVM
    {
        // Tiêu chí tìm kiếm
        public string SearchTerm { get; set; }

        // Hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6; // Hiển thị 6 sản phẩm/trang

        // Danh sách 10 sản phẩm bán chạy nhất
        public List<Product> FeaturedProducts { get; set; }

        // Danh sách 20 sản phẩm mới nhất đã phân trang
        public IPagedList<Product> NewProducts { get; set; }
    }
}