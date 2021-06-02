using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Interfaces;
using Tabler.Docs.Models;

namespace Tabler.Docs.ViewModels
{
    public class UsuarioViewModel 
    {
        public Usuario Usuario { get; set; }

        public UsuarioViewModel()
        {
            Usuario = new Usuario();
        }

    }
}
