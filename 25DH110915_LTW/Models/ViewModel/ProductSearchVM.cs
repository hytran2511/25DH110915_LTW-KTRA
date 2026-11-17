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
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }

        // Thêm các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;

        public List<Category> Categories { get; set; }
        public IPagedList<Product> Products { get; set; }
    }
}
