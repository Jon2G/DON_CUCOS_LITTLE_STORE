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
        public Supplier Proveedor { get; set; }
        public ProveedoresViewModel()
        {
            Proveedor = new Supplier();
        }
    }
}
