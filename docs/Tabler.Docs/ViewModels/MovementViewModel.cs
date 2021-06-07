using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
  public  class MovementViewModel : IRefresh
    {
        public char Type { get; set; }
        public Movement Movement { get; set; }
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
        public MovementConcept SelectedConcept { get; set; }
        public List<MovementConcept> Concepts { get; set; }
        public MovementViewModel()
        {
            Movement = new Movement();
            Products = new List<Product>();
            SelectedProduct = new Product();
            SelectedConcept = new MovementConcept();
            SelectedQuantity = 0;
            Concepts = new List<MovementConcept>(); 
        }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
     

        public void Agregar()
        {
            if (this.Movement.Parts.FirstOrDefault(x => x.Product.Equals(this.SelectedProduct)) is MovementPart ajuste)
            {
                ajuste.Quantity += (float)this.SelectedQuantity;
                SelectedProduct = new Product();
                SelectedQuantity = 0;
                return;
            }
            this.Movement.Parts.Add(new MovementPart(this.Type)
            {
                Product = this.SelectedProduct,
                Quantity = this.SelectedQuantity,
                InitiallyStock = Product.GetStock(SelectedProduct.Id)
            });
            SelectedProduct = new Product();
            SelectedQuantity = 0;
        }
        public async void AgregarIdConcept()
        {
            this.Movement.Concept = SelectedConcept;
        }

        public Task Refresh(char Type)
        {
            this.Type = Type;
            return Refresh();
        }
        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                Movement = new Movement();
                Products = await Product.GetAll();
                Concepts = await MovementConcept.GetAll(Type);
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
