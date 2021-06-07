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
    public class SalePart
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        private float _Quantity;

        public float Quantity
        {
            get => _Quantity;
            set
            {
                if (Product is not null && Product.Stock < value)
                {
                    _Quantity = Product.Stock;
                    return;
                }

                _Quantity = value;
            }
        }

        public float Total => Product.Price * Quantity;
        public float DisccountTotal => Product.DisccountPrice * Quantity;
        public bool HasDisscount => Product.HasDisscount;
        public string ProductName => Product.Name;
        public float Price => Product.HasDisscount? Product.DisccountPrice:Product.Price;
        public SalePart()
        {
            Quantity = 1;
        }

        public SalePart(Product product) : this()
        {
            Product = product;
        }

        public void Save(Sale sale)
        {
            this.Id = AppData.SQL.Single<int>("SP_SAVE_SALE_PART", CommandType.StoredProcedure,
                new SqlParameter("SALE_ID", sale.Id),
                new SqlParameter("PRODUCT_ID", this.Product.Id),
                new SqlParameter("PRICE", this.Product.HasDisscount ? this.Product.DisccountPrice : this.Product.Price),
                new SqlParameter("QUANTITY", this.Quantity),
                new SqlParameter("TOTAL",this.HasDisscount? this.DisccountTotal: this.Total));
        }
    }
}
