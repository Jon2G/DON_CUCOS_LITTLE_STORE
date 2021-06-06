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
        public List<MovementPart> movementParts { get; set; }
        public MovementsPageViewModel()
        {
            Movements = new List<Movement>();
            movementParts = new List<MovementPart>();
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
                this.movementParts.AddRange(await MovementPart.GetAll());
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
