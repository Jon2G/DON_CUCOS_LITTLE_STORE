using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public CustomerViewModel()
        {
            Customer = new Customer();
        }
    }
}
