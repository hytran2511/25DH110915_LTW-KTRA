using System;

namespace _25DH110915_LTW.Models.ViewModel
{
    public class CategoryStatVM
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal AvgPrice { get; set; }
    }
}
