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
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public string Unit { get; set; }
        public string Picture { get; set; }


        public float Stock { get; set; }
        public float Minimum { get; set; }
        public float Maximum { get; set; }
        public float Price { get; set; }
        public bool Disabled { get; set; }

        public Product() { }

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
        public static async Task<Product> Obtener(string Codigo)
        {
            if (string.IsNullOrEmpty(Codigo)) { return await Task.FromResult(new Product()); }
            await Task.Yield();
            Product producto = null;
            if (SQLHelper.IsInjection(Codigo))
            {
                CustomMessageBox.Show("Intento de modificación invalido", "Atención", CustomMessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return producto;
            }
            using (IReader leector = AppData.SQL.Read("SELECT * FROM PRODUCTOS WHERE OCULTO=0 AND CODIGO='" + Codigo + "'"))
            {
                if (leector.Read())
                {
                    producto = new Product()
                    {
                        Id = Convert.ToInt32(leector["ID"]),
                        Code = Convert.ToString(leector["CODIGO"]),
                        Name = Convert.ToString(leector["NOMBRE"]),
                        Description = Convert.ToString(leector["DESCRIPCION"]),
                        Category = Category.GetById(Convert.ToInt32(leector[0])),
                        Unit = Convert.ToString(leector["UNIDAD"]),
                        Picture = leector["IMAGEN"].ToString(),
                        Supplier = Supplier.GetById(Convert.ToInt32(leector[1])),
                        Stock = Convert.ToSingle(leector["EXISTENCIA"]),
                        Minimum = Convert.ToSingle(leector["MINIMO"]),
                        Maximum = Convert.ToSingle(leector["MAXIMO"]),
                        Price = Convert.ToSingle(leector["PRECIO"])
                    };
                }
            }
            return producto;
        }

     

        public static async Task<List<Product>> GetByCategory(Category Category)
        {
            List<Product> productos = new List<Product>();
            foreach (string codigo in AppData.SQL.Lista<string>("SP_GET_PRODUCTS_BY_CATEGORY", CommandType.StoredProcedure, 0,
                new SqlParameter("CATEGORY_ID", Category.Id)))
            {
                productos.Add(await Obtener(codigo));
            }
            return productos;
        }
        public static Task<List<Product>> GetByCategory(string CategoryName)
        {
            Category category = Category.GetByName(CategoryName);
            return GetByCategory(category);
        }
        public static async Task<List<Product>> GetAll()
        {
            List<Product> productos = new List<Product>();
            foreach (string codigo in AppData.SQL.Lista<string>("SP_GET_PRODUCTS", CommandType.StoredProcedure))
            {
                productos.Add(await Obtener(codigo));
            }
            return productos;
        }

        public static async Task<List<Product>> Search(string Search)
        {
            List<Product> productos = new List<Product>();
            foreach (string codigo in AppData.SQL.Lista<string>("SP_SEARCH_PRODUCT", CommandType.StoredProcedure, 0,
                new SqlParameter("SEARCH", Search)))
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

        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_PRODUCT", CommandType.StoredProcedure,
                new SqlParameter("ID", Id),
                new SqlParameter("ID", Id),
                new SqlParameter("CODE", Code),
                new SqlParameter("NAME", Name),
                new SqlParameter("DESCRIPTION", Description),
                new SqlParameter("CATEGORY_ID", Category.Id),
                new SqlParameter("SUPPLIER_ID", Id),
                new SqlParameter("UNIT", Unit),
                new SqlParameter("IMAGE", Picture),
                new SqlParameter("MINIMUM", Minimum),
                new SqlParameter("MAXIMUM", Maximum),
                new SqlParameter("PRICE", Price),
                new SqlParameter("DISABLED", Disabled)
                );
            /*
             @ID INT,@CODE VARCHAR(100),@NAME VARCHAR(100),@DESCRIPTION VARCHAR(100),
@CATEGORY_ID INT,@SUPPLIER_ID INT,@UNIT VARCHAR(100),@IMAGE VARCHAR(MAX),@STOCK REAL,@MINIMUM REAL,@MAXIMUM REAL,
@PRICE REAL,@DISABLED BIT
             */
        }


    }
}
