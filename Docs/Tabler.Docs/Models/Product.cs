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
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
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
        private string _Picture;
        public string Picture { 
            get=> string.IsNullOrEmpty(_Picture)? "_content/CucoStore.Docs/img/LogoWhite.png":_Picture;
            set => _Picture = value;
        }
        private int _Disccount;

        public int Disccount
        {
            get => _Disccount;
            set
            {
                if (value > 100)
                {
                    _Disccount = 100;
                    return;
                }
                _Disccount = value;
            }
        }

        public float Price { get; set; }
        public float DisccountPrice
        {
            get
            {
                if (Disccount <= 0)
                {
                    return 0;
                }
                if (Price <= 0)
                {
                    return 0;
                }
                return Price - (Price * (Disccount / 100f));
            }
        }
        public float Saved
        {
            get => Price - DisccountPrice;
        }
        public float Stock { get; set; }
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public bool Disabled { get; set; }
        public bool HasStock => Stock > 0;
        public bool HasDisscount => Disccount > 0;

        public Product() { }


        internal static float GetStock(int id)
        {
            return AppData.SQL.Single<float>("SP_GETSTOCK", CommandType.StoredProcedure, new SqlParameter("ID", id));
        }

        /// <summary>
        /// Se lee en la base de datos la información de un producto
        /// </summary>
        /// <param name="Codigo">El código del prodcuto que se desea obtener</param>
        /// <returns>Se regresa un producto con sus datos cargados</returns>
        public static async Task<Product> GetById(int Id)
        {
            if (Id <= 0) { return await Task.FromResult(new Product()); }
            await Task.Yield();
            Product producto = null;
            using (IReader reader = AppData.SQL.Read("SP_GET_PRODUCT_BY_ID", CommandType.StoredProcedure, 
                new SqlParameter("ID", Id)))
            {
                if (reader.Read())
                {
                    producto = new Product()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Code = Convert.ToString(reader[1]),
                        Name = Convert.ToString(reader[2]),
                        Description = Convert.ToString(reader[3]),
                        Category = Category.GetById(Convert.ToInt32(reader[4])),
                        Supplier = Supplier.GetById(Convert.ToInt32(reader[5])),
                        Unit = Convert.ToString(reader[6]),
                        Picture = reader[7].ToString(),
                        Stock = Convert.ToSingle(reader[8]),
                        Minimum = Convert.ToSingle(reader[9]),
                        Maximum = Convert.ToSingle(reader[10]),
                        Price = Convert.ToSingle(reader[11]),
                        Disabled = Convert.ToBoolean(reader[12]),
                        Disccount = Convert.ToInt32(reader[13])
                    };
                }
            }
            return producto;
        }



        public static async Task<List<Product>> GetByCategory(Category Category, string search)
        {
            List<Product> productos = new List<Product>();
            foreach (int id in AppData.SQL.Lista<int>("SP_GET_PRODUCTS_BY_CATEGORY_FIND", CommandType.StoredProcedure, 0,
                new SqlParameter("CATEGORY_ID", Category.Id),
                new SqlParameter("SEARCH", search)
                ))
            {
                productos.Add(await GetById(id));
            }
            return productos;
        }
        public static async Task<List<Product>> GetByCategory(Category Category)
        {
            List<Product> productos = new List<Product>();
            foreach (int id in AppData.SQL.Lista<int>("SP_GET_PRODUCTS_BY_CATEGORY", CommandType.StoredProcedure, 0,
                new SqlParameter("CATEGORY_ID", Category.Id)))
            {
                productos.Add(await GetById(id));
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
            foreach (int id in AppData.SQL.Lista<int>("SP_GET_PRODUCTS", CommandType.StoredProcedure))
            {
                productos.Add(await GetById(id));
            }
            return productos;
        }

        public static async Task<List<Product>> Search(string Search)
        {
            List<Product> productos = new List<Product>();
            foreach (int id in AppData.SQL.Lista<int>("SP_SEARCH_PRODUCT", CommandType.StoredProcedure, 0,
                new SqlParameter("SEARCH", Search)))
            {
                productos.Add(await GetById(id));
            }
            return productos;
        }



        public void Save()
        {
            AppData.SQL.EXEC("SP_ABC_PRODUCT", CommandType.StoredProcedure,
                new SqlParameter("ID", Id),
                new SqlParameter("CODE", Code),
                new SqlParameter("NAME", Name),
                new SqlParameter("DESCRIPTION", Description),
                new SqlParameter("CATEGORY_ID", Category.Id),
                new SqlParameter("SUPPLIER_ID", Supplier.Id),
                new SqlParameter("STOCK", Stock),
                new SqlParameter("UNIT", Unit),
                new SqlParameter("IMAGE", Picture),
                new SqlParameter("MINIMUM", Minimum),
                new SqlParameter("MAXIMUM", Maximum),
                new SqlParameter("PRICE", Price),
                new SqlParameter("DISABLED", Disabled),
                new SqlParameter("DISCCOUNT", Disccount)
                );
        }
        public override bool Equals(object? obj)
        {
            if (obj is Product product)
            {
                return product.Id == this.Id;
            }
            return false;
        }

    }
}
