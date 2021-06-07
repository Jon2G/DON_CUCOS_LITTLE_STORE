using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
   public class BuyPageViewModel 
    {
        public bool IsLoading { get; set; }
        public List<Buy> Buys { get; set; } 
        public Buy SelectedBuy { get; set; }
        public BuyPageViewModel()
        {
            Buys = new List<Buy>();
            SelectedBuy = new Buy();
        }




    }
}
