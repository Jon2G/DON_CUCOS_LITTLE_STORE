using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;

namespace CucoStore.Docs.Models
{
    public class BuyPart
    {
        public int Id { get; set; }
        public int IdBuy { get; set; }
        public Product Product { get; set; }
        private float _Quantity;

        public float Quantity
        {
            get => _Quantity;
            set
            {
                if (value <= 0)
                {
                    _Quantity = 1;
                    return;
                }
                _Quantity = value;
            }
        }

        public float InitiallyStock { get; set; }
        public float Price { get; set; }
        public float NewStock => InitiallyStock + Quantity;
        public float Value => Price * Quantity;

        public float NewStockB { get; set; }
        public char Type { get; set; }
        public BuyPart()
        {

        }
        public BuyPart(char Type)
        {
            this.Type = Type;
        }
        public static async Task<BuyPart> GetById(int movementpId)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GET_BUY_PART_BY_ID",
                CommandType.StoredProcedure, new SqlParameter("ID", movementpId)))
            {
                if (reader.Read())
                {
                    return new BuyPart()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        IdBuy = Convert.ToInt32(reader[1]),
                        Product = await Product.GetById(Convert.ToInt32(reader[2])),
                        Quantity = Convert.ToSingle(reader[3]),
                        InitiallyStock = Convert.ToSingle(reader[4]),
                        NewStockB = Convert.ToSingle(reader[5])
                    };
                }
            }
            return new BuyPart();
        }

        public void Save(Buy buy)
        {
           this.Id= AppData.SQL.Single<int>("SP_ADD_BUY_PARTS", CommandType.StoredProcedure,
                new SqlParameter("BUY_ID", buy.Id),
                new SqlParameter("PRODUCT_ID", Product.Id),
                new SqlParameter("QUANTITY", Quantity)
           );
        }
    }
}
