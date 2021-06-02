using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabler.Docs.Models
{
    class Permisos
    {
     public int ID { get; set; }
     public bool STOCK_OUT { get; set; }
     public bool STOCK_IN { get; set; }
     public bool REPORTS_READ { get; set; }
     public bool USER_MANAGER { get; set; }
    }
}
