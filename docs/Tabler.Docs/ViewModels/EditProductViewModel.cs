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
        public Product Product { get; set; }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;

        public Task RequestLoad(string ProductId)
        {
            this.Product = new Product(ProductId);
            return Refresh();
        }
        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(1000);
                Product = await Product.GetById(Product.Id);
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
