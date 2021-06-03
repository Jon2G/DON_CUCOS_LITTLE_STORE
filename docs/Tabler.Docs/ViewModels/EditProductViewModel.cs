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
        public int ProductId { get; private set; }
        public Product Product { get; set; }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public EditProductViewModel()
        {
            this.Product = new Product();
            this.Categories = new List<Category>();
            this.Suppliers = new List<Supplier>();
        }
        public async Task RequestLoad(int ProductId)
        {
            this.ProductId = ProductId;
            await Refresh();
        }

        public async Task RefreshSuppliers()
        {
            Suppliers.Clear();
            this.Suppliers.AddRange(await Supplier.GetAll());
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
                Product = await Product.GetById(ProductId);
               await RefreshSuppliers();
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
