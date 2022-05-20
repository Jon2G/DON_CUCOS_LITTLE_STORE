using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
    public class LoginPageViewModel
    {
        public User User { get; set; }
        public LoginPageViewModel()
        {
            User = new User();
        }
        public async Task<bool> Login()
        { 
            User =await User.Login();
            if (User.Id > 0)
            {
                AppData.Current.User = User;
                return true;
            }
            return false;
        }
    }
}
