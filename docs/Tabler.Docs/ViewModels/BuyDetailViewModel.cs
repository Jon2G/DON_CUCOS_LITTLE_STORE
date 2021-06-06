using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
  public  class BuyDetailViewModel : IRefresh
    {
        public Buy Buy { get; set; }
        public Product SelectedProduct { get; set; }

        private float _SelectedQuantity;
        public float SelectedQuantity
        {
            get => _SelectedQuantity;
            set
            {
                if (value <= 0)
                {
                    _SelectedQuantity = 1;
                    return;
                }
                _SelectedQuantity = value;
            }
        }
        public List<Product> Products { get; set; }


        public BuyDetailViewModel()
        {
            Buy = new Buy();
            Products = new List<Product>();
            SelectedProduct = new Product();
            SelectedQuantity = 0;
        }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
     

        public void Agregar()
        {
            if (this.Buy.Parts.FirstOrDefault(x => x.Product.Equals(this.SelectedProduct)) is BuyPart ajuste)
            {
                ajuste.Quantity += (float)this.SelectedQuantity;
                SelectedProduct = new Product();
                SelectedQuantity = 0;
                return;
            }
            this.Buy.Parts.Add(new BuyPart()
            {
                Product = this.SelectedProduct,
                Quantity = this.SelectedQuantity,
                InitiallyStock = Product.GetStock(SelectedProduct.Id)
            });
            SelectedProduct = new Product();
            SelectedQuantity = 0;
        }


        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                Buy = new Buy();
                Products = await Product.GetAll();
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
