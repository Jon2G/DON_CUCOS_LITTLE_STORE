using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
   public class MovementsPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
        public List<Movement> Movements { get; set; }
        public MovementsPageViewModel()
        {
            Movements = new List<Movement>();
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
                this.Movements.Clear();
                this.Movements.AddRange(await Movement.GetAll());
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
