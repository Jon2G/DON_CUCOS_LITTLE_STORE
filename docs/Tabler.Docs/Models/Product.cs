using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Enums;
using Kit.Model;
using Kit.Sql.Helpers;
using Kit.Sql.Readers;
using Kit.WPF.Dialogs.ICustomMessageBox;
using Kit.WPF.Extensions;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
    public class Producto : ModelBase
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Clasificacion { get; set; }
        public string Unidad { get; set; }
        private string _Imagen;
        public string Imagen { get => _Imagen; set { _Imagen = value; OnPropertyChanged(); } }
        public string Proveedor { get; set; }
        public float Existencia { get; set; }
        public float Minimo { get; set; }
        public float Maximo { get; set; }
        public float Precio { get; set; }

        public Producto() { }
        public Producto(int Id, string Codigo, string Nombre, string Descripcion, string Clasificacion, string Unidad, string Imagen, string Proveedor, float Existencia, float Minimo, float Maximo, float Precio)
        {
            this.Id = Id;
            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Descripcion = Descripcion;
            this.Clasificacion = Clasificacion;
            this.Unidad = Unidad;
            this.Imagen = Imagen;
            this.Proveedor = Proveedor;
            this.Existencia = Existencia;
            this.Minimo = Minimo;
            this.Maximo = Maximo;
            this.Precio = Precio;
        }

        public static List<string> ListarProvedores()
        {
            return AppData.SQL.Lista<string>("SELECT DISTINCT PROVEDOR FROM PRODUCTOS WHERE OCULTO=0");
        }
        public static List<string> ListarCategorias()
        {
            return AppData.SQL.Lista<string>("SELECT DISTINCT CLASIFICACION FROM PRODUCTOS WHERE OCULTO=0");
        }

        /// <summary>
        /// Se lee en la base de datos la información de un producto
        /// </summary>
        /// <param name="Codigo">El código del prodcuto que se desea obtener</param>
        /// <returns>Se regresa un producto con sus datos cargados</returns>
        public static async Task<Producto> Obtener(string Codigo)
        {
            if (string.IsNullOrEmpty(Codigo)) { return await Task.FromResult(new Producto()); }
            await Task.Yield();
            Producto producto = null;
            if (SQLHelper.IsInjection(Codigo))
            {
                CustomMessageBox.Show("Intento de modificación invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return producto;
            }
            using (IReader leector = AppData.SQL.Read("SELECT * FROM PRODUCTOS WHERE OCULTO=0 AND CODIGO='" + Codigo + "'"))
            {
                if (leector.Read())
                {
                    int Id = Convert.ToInt32(leector["ID"]);
                    string codigo = Convert.ToString(leector["CODIGO"]);
                    string nombre = Convert.ToString(leector["NOMBRE"]);
                    string descripcion = Convert.ToString(leector["DESCRIPCION"]);
                    string clasificacion = Convert.ToString(leector["CLASIFICACION"]);
                    string unidad = Convert.ToString(leector["UNIDAD"]);
                    string imagen = (string)leector["IMAGEN"];
                    string proveedor = Convert.ToString(leector["PROVEDOR"]);
                    float existencia = Convert.ToSingle(leector["EXISTENCIA"]);
                    float minimo = Convert.ToSingle(leector["MINIMO"]);
                    float maximo = Convert.ToSingle(leector["MAXIMO"]);
                    float precio = Convert.ToSingle(leector["PRECIO"]);
                    producto = new Producto(Id, codigo, nombre, descripcion, clasificacion, unidad, imagen, proveedor, existencia, minimo, maximo, precio);
                }
            }
            return producto;
        }

        public bool Validar()
        {
            this.Descripcion = Descripcion?.Trim() ?? string.Empty;

            this.Codigo = this.Codigo?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(this.Codigo))
            {
                CustomMessageBox.Show("El código de producto no puede estar vacio", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }

            this.Nombre = Nombre?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(this.Nombre))
            {
                CustomMessageBox.Show("El nombre no puede estar vacio", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }

            this.Clasificacion = Clasificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(this.Clasificacion))
            {
                CustomMessageBox.Show("La categoría no puede estar vacia", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }

            this.Unidad = Unidad?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(this.Unidad))
            {
                CustomMessageBox.Show("La unidad no puede estar vacia", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }

            this.Proveedor = Proveedor?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(this.Proveedor))
            {
                CustomMessageBox.Show("El Proveedor no puede estar vacio", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }

            if (Minimo > Maximo || Minimo == Maximo)
            {
                CustomMessageBox.Show("El minimo debe ser menor que el máximo", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public static async Task<List<Producto>> GetByCategory(Category Category)
        {
            List<Producto> productos = new List<Producto>();
            foreach (string codigo in AppData.SQL.Lista<string>("SP_GET_PRODUCTS_BY_CATEGORY", CommandType.StoredProcedure,0,
                new SqlParameter("CATEGORY_ID", Category.Id)))
            {
                productos.Add(await Obtener(codigo));
            }
            return productos;
        }
        public static Task<List<Producto>> GetByCategory(string CategoryName)
        {
            Category category = Category.GetByName(CategoryName);
            return GetByCategory(category);
        }
        public static async Task<List<Producto>> Listar()
        {
            List<Producto> productos = new List<Producto>();
            foreach (string codigo in AppData.SQL.Lista<string>("SP_GET_PRODUCTS",CommandType.StoredProcedure))
            {
                productos.Add(await Obtener(codigo));
            }
            return productos;
        }

        public static async Task<List<Producto>> Buscar(string Categoria, string Busqueda)
        {
            //List<Producto> productos = new List<Producto>();
            //if (SQLHelper.IsInjection(Categoria, Busqueda))
            //{
            //    return productos;
            //}

            //return AppData.SQL.Lista<string
            //    >("SELECT CODIGO FROM PRODUCTOS WHERE (CLASIFICACION = '" + Categoria + "' OR '" + Categoria + "'='') AND OCULTO=0 AND NOMBRE LIKE '%" + Busqueda + "%'  ORDER BY NOMBRE")
            //    .Select(x => Obtener(x)).ToList();

            List<Producto> productos = new List<Producto>();
            foreach (string codigo in AppData.SQL.Lista<string>("SELECT CODIGO FROM PRODUCTOS WHERE OCULTO=0 ORDER BY NOMBRE"))
            {
                productos.Add(await Obtener(codigo));
            }
            return productos;
        }

        public static float ObtenerExistencia(string CodigoProducto)
        {
            if (SQLHelper.IsInjection(CodigoProducto))
            {
                return 0;
            }
            return AppData.SQL.Single<float>($"SELECT EXISTENCIA FROM PRODUCTOS WHERE CODIGO='{CodigoProducto}'"); ;
        }
        public static int ObtenerId(string CodigoProducto)
        {
            if (SQLHelper.IsInjection(CodigoProducto))
            {
                return 0;
            }
            return AppData.SQL.Single<int>($"SELECT ID FROM PRODUCTOS WHERE CODIGO='{CodigoProducto}'"); ;
        }

        public bool Existe()
        {
            if (SQLHelper.IsInjection(Codigo))
            {
                CustomMessageBox.Show("Intento de modificación invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return false;
            }
            return AppData.SQL.Exists("SELECT CODIGO FROM PRODUCTOS WHERE CODIGO='" + Codigo + "'");
        }
        /// <summary>
        /// Insertar en la base de datos un nuevo producto
        /// </summary>
        public void Alta()
        {
            if (SQLHelper.IsInjection(Nombre, Descripcion, Clasificacion, Unidad, Proveedor, Codigo))
            {
                CustomMessageBox.Show("Intento de modificación invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return;
            }
            if (Existe())
            {
                Modificacion();
                return;
            }
            //AppData.SQL.EXEC(
            //    "INSERT INTO PRODUCTOS (CODIGO,NOMBRE,DESCRIPCION,CLASIFICACION,UNIDAD,IMAGEN,PROVEDOR,EXISTENCIA,MINIMO,MAXIMO,PRECIO) VALUES(?,?,?,?,?,?,?,?,?,?,?);"
            //    , Codigo, Nombre, Descripcion, Clasificacion, Unidad, Imagen, Proveedor, Existencia, Minimo, Maximo, Precio);

        }
        /// <summary>
        /// Dar de baja en la base de datos un producto
        /// </summary>
        public void Baja()
        {
            if (SQLHelper.IsInjection(Codigo))
            {
                CustomMessageBox.Show("Intento de baja invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return;
            }
         //   AppData.SQL.EXEC("UPDATE PRODUCTOS SET OCULTO = 1 WHERE CODIGO = ?", Codigo);
        }
        /// <summary>
        ///  Actualizar un producto en la base
        /// </summary>
        public void Modificacion()
        {
            if (SQLHelper.IsInjection(Nombre, Descripcion, Clasificacion, Unidad, Proveedor, Codigo))
            {
                CustomMessageBox.Show("Intento de modificación invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return;
            }
            //AppData.SQL.EXEC(
            //    "UPDATE PRODUCTOS SET NOMBRE=?,DESCRIPCION=?,CLASIFICACION=?,UNIDAD=?,IMAGEN=?,PROVEDOR=?,EXISTENCIA=?,MINIMO=?,MAXIMO=?,PRECIO=? WHERE CODIGO=?"
            //    , Nombre, Descripcion, Clasificacion, Unidad, Imagen, Proveedor, Existencia, Minimo, Maximo, Precio, Codigo);
            //return;
        }
    }
}
