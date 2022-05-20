using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
   public class StockPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
        public List<Stock> Stocks { get; set; }
        public StockPageViewModel()
        {
            Stocks = new List<Stock>();
        }
        public event EventHandler Refreshed;
        public async Task Refresh()
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                IsLoading = true;
                this.Stocks.Clear();
                this.Stocks.AddRange(await Stock.GetAll());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
                Refreshed?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
