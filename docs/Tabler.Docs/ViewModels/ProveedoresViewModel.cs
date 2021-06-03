using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
  public class ProveedoresViewModel
    {
        public Supplier Supplier { get; set; }
        public ProveedoresViewModel()
        {
            Supplier = new Supplier();
        }
    }
}
