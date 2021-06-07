using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
   public class MovementsPageViewModel 
    {
        public  char Type { get; set; }
        public bool IsLoading { get; set; }
        public List<Movement> Movements { get; set; } 
        public Movement SelectedMovent { get; set; }
        public MovementsPageViewModel()
        {
            Movements = new List<Movement>();
            SelectedMovent = new Movement();
        }
    }
}
