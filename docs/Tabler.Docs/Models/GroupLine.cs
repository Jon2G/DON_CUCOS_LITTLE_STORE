using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;

namespace Tabler.Docs.Models
{
    public class GroupLine:IRefresh
    {
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;

        public List<Product> Products { get; set; }
        public Category Category { get; set; }
        public GroupLine(Category Linea)
        {
            this.Category = Linea;
            this.Products = new List<Product>();
        }

        public async Task Refresh()
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                Products.Clear();
                IsLoading = true;
                Products.AddRange(await Product.GetByCategory(this.Category));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this,EventArgs.Empty);
            }
        }
    }
}
