using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace e_commerce.Models.ViewModel
{
    public class ProductIndexViewModel
    {
        public IPagedList<Product> Products { get; set; }
        //public IQueryable<Product> Products { get; set; }
        public IEnumerable<CategoryWithCount> CatWithCount { get; set; }
        public IEnumerable<SelectListItem> CatFilterItems
        {
            get
            {
                var allCats = CatWithCount.Select(x => new SelectListItem
                {
                    Value = x.CategoryName,
                    Text = x.CatNameWithCount
                }
                 );
                return allCats;
            }
        }

        public string Search { get; set; }
        public string Category { get; set; }
        public string SortBy { get; set; }
        public Dictionary<string, string> Sort { get; set; }

    }

    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return string.Format("{0}({1})", CategoryName, ProductCount);
            }
        }
    }
}