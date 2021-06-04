using System;
using System.Collections.Generic;
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
        public double Total => Parts.Sum(x=>x.Total);

        public Sale()
        {
            Parts = new List<SalePart>();
            Customer = new Customer();
            User = AppData.Current.User;
        }
    }
}
