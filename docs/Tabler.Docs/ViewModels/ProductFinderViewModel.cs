using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class ProductFinderViewModel
    {
        public List<Product> Products { get; set; }
        public string Search { get; set; }
        public ProductFinderViewModel()
        {
            
        }

        public async Task DoSearch()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Products =await Product.GetAll();
                return;
            }

            Products = await Product.Search(Search);
        }
    }
}
