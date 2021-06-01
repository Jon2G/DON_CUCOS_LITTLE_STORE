using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabler.Docs.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Product(string Id)
        {
            this.Id = Id;
        }
        public Product(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public async Task<Product> GetById(string Id)
        {
            return new Product(Id, "Recuperado");
        }
    }
}
