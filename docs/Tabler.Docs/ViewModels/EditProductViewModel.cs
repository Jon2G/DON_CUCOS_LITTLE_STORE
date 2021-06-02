using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class EditProductViewModel : IRefresh
    {
        public string ProductId { get; private set; }
        public Producto Product { get; set; }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public List<Category> Categories { get; set; }

        public EditProductViewModel()
        {
            this.Product = new Producto();
            this.Categories = new List<Category>();
        }
        public async Task RequestLoad(string ProductId)
        {
            this.ProductId = ProductId;
            await Refresh();
        }

        public async Task RefreshCategories()
        {
            Categories.Clear();
            this.Categories.AddRange(await Category.GetAll());
        }
        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                Product = await Producto.Obtener(ProductId);
               await RefreshCategories();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
