using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;

namespace CucoStore.Docs.Models
{
    public class GroupLine
    {
        public bool IsLoading { get; set; }

        public List<Product> Products { get; set; }
        public Category Category { get; set; }
        public GroupLine(Category Linea)
        {
            this.Category = Linea;
            this.Products = new List<Product>();
        }

        public async Task Refresh(string search=null)
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                Products.Clear();
                IsLoading = true;
                if (string.IsNullOrEmpty(search))
                {
                    Products.AddRange(await Product.GetByCategory(this.Category));
                }
                else
                {
                    Products.AddRange(await Product.GetByCategory(this.Category, search));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
