using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
  public  class MovementViewModel : IRefresh
    {

        public Movement movement { get; set; }
        public Product SelectedProduct { get; set; }
        public float SelectedQuantity { get; set; }
        public List<Product> products { get; set; }
        public MovementConcept SelectedConcept { get; set; }
        public List<MovementConcept> movementConcepts { get; set; }
        public MovementViewModel()
        {
            movement = new Movement();
            products = new List<Product>();
            SelectedProduct = new Product();
            SelectedConcept = new MovementConcept();
            SelectedQuantity = 0;
            movementConcepts = new List<MovementConcept>(); 
        }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public int MovementId { get; set; }
        public async Task RequestLoad(int UserId)
        {
            this.MovementId = UserId;
            await Refresh();
        }
        public void Agregar()
        {
            this.movement.Parts.Add(new MovementPart(this.movement.Type)
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
            this.movement.Concept = SelectedConcept;
        }
        public async Task Refresh()
        {
            try
            {
                IsLoading = true;
                movement = await Movement.GetById(MovementId);
                products = await Product.GetAll();
                movementConcepts = await MovementConcept.GetAll();
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
