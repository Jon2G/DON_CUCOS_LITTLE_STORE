using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class QuickLoginViewModel : LoginPageViewModel
    {
        public QuickLoginViewModel() : base()
        {
            User = new User();
        }
    }
}
