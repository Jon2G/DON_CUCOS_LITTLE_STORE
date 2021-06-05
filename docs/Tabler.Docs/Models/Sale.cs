using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
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
        public float Payed => Payment.Total;

        public float ToPay
        {
            get
            {
                var topay = Total - Payment.Total;
                if (topay < 0)
                {
                    topay = 0;
                }
                return topay;
            }
        }
        public float Change => Payed - Total;

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

        public void Confirm()
        {
            this.Save();
            this.Payment.Save(this);
            foreach (var part in Parts)
            {
                part.Save(this);
            }
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
                new SqlParameter("TOTAL", this.Total));
        }
    }
}
