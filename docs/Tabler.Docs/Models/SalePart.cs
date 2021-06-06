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
    public class SalePart
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Total => Price * Quantity;
        public string ProductName => Product.Name;
        public SalePart()
        {
            Quantity = 1;
        }

        public SalePart(Product product):this()
        {
            Product = product;
            Price = product.Price;
        }

        public void Save(Sale sale)
        {
            this.Id = AppData.SQL.Single<int>("SP_SAVE_SALE_PART", CommandType.StoredProcedure,
                new SqlParameter("SALE_ID", sale.Id),
                new SqlParameter("PRODUCT_ID", this.Product.Id),
                new SqlParameter("PRICE", this.Price),
                new SqlParameter("QUANTITY", this.Quantity),
                new SqlParameter("TOTAL", this.Total));
        }
    }
}
