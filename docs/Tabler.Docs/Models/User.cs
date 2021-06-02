using Kit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tabler.Docs.Models
{
  public  class User : ModelBase
    {
        public int Id { get; set; }
        public int Permisos { get; set; }
        public int Key_Id { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        private string _Imagen;
        public string Imagen { get => _Imagen; set { _Imagen = value; OnPropertyChanged(); } }

        public User() { }
        public User(int Id, int Permisos, int Key_Id ,string Nickname, string Name, string Password, string Imagen)
        {
            this.Id = Id;
            this.Permisos = Permisos;
            this.Key_Id = Key_Id;
            this.Nickname = Nickname;
            this.Name = Name;
            this.Password = Password;
            this.Imagen = Imagen;
        }
    }
}
