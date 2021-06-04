using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class UsuarioViewModel : IRefresh
    {

        public User Usuario { get; set; }

        public UsuarioViewModel()
        {
            Usuario = new User();
        }
        public bool IsLoading { get; set; }
        public event EventHandler Refreshed;
        public int UserId { get; set; }
        public async Task RequestLoad(int UserId)
        {
            this.UserId = UserId;
            await Refresh();
        }
        public async Task Refresh()
        {
            try
            {
                
                IsLoading = true;
                Usuario = await User.GetById(UserId);
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
