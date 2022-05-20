using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;
using CucoStore.Docs.Models;

namespace CucoStore.Docs.ViewModels
{
    public class QuickLoginViewModel : LoginPageViewModel
    {
        public QuickLoginViewModel() : base()
        {
            User = new User();
        }
    }
}
