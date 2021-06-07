using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Interfaces;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
    public class UserPageViewModel : IRefresh
    {
        public bool IsLoading { get; set; }
        public List<User> Users { get; set; }
        public UserPageViewModel()
        {
            Users = new List<User>();
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
                this.Users.Clear();
                this.Users.AddRange(await User.GetAll());
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
