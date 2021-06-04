using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabler.Docs.Models
{
    public class SalePart
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Total => Price * Quantity;

        public SalePart()
        {
            
        }

    }
}
