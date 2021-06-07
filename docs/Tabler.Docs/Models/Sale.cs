using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public List<SalePart> Parts { get; set; }
        public Payment Payment { get; set; }
        public float Total => Parts.Sum(x => x.Total);
        public float DisccountTotal => Parts.Where(x => x.HasDisscount).Sum(x => x.DisccountTotal) + Parts.Where(x => !x.HasDisscount).Sum(x => x.Total);
        public bool HasDisscount => Parts.Any(x => x.HasDisscount);
        public float Payed => Payment.Total;

        public float ToPay
        {
            get
            {
                var topay = HasDisscount ? DisccountTotal - Payment.Total : Total - Payment.Total;
                if (topay < 0)
                {
                    topay = 0;
                }
                return topay;
            }
        }
        public float Change => Payed - (HasDisscount ? DisccountTotal : Total);
        public string Letters => Kit.Extensions.Helpers.EnLetra((decimal)(HasDisscount ? DisccountTotal : Total), "", true, "MXN");
        public Sale()
        {
            Parts = new List<SalePart>();
            Customer = new Customer();
            User = AppData.Current.User;
            Payment = new Payment();
        }

        public void Add(Product product)
        {
            if (Parts.FirstOrDefault(x => x.Product.Equals(product)) is SalePart salePart)
            {
                salePart.Quantity++;
            }
            else
            {
                this.Parts.Insert(0, new SalePart(product));
            }
        }

        public async Task Confirm()
        {
            this.Date = DateTime.Now;
            this.Save();
            this.Payment.Save(this);
            foreach (var part in Parts)
            {
                part.Save(this);
            }
            await SaveMovement();
        }

        private async Task SaveMovement()
        {
            Movement movement = new Movement
            {
                Concept = await MovementConcept.GetById(4),
                Date = DateTime.Now,
                Id = 0,
                Observations = $"Ticket #{this.Id}",
                Type = 'S',
                Parts = new List<MovementPart>(Parts.Select(x => new MovementPart('S')
                {
                    Id = 0,
                    IdMovement = 0,
                    InitiallyStock = x.Product.Stock,
                    NewStockB = x.Product.Stock - x.Quantity,
                    Type = 'S',
                    Quantity = x.Quantity,
                    Product = x.Product
                }))
            };
            await movement.Save();
        }

        private void Save()
        {
            if (Customer.Id <= 0)
            {
                Customer = Customer.Default();
            }
            this.Id = AppData.SQL.Single<int>("SP_SAVE_SALE", CommandType.StoredProcedure,
                new SqlParameter("CUSTOMER_ID", this.Customer.Id),
                new SqlParameter("USER_ID", this.User.Id),
                new SqlParameter("CHANGE", this.Change),
                new SqlParameter("TOTAL", this.HasDisscount ? this.DisccountTotal : this.Total));
        }
    }
}
