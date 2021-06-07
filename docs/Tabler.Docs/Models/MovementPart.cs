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
    public class MovementPart
    {
        public int Id { get; set; }
        public int IdMovement { get; set; }
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
        public float NewStock
        {
            get
            {
                if (Type == 'S')
                {
                    return InitiallyStock - Quantity;
                }
                return InitiallyStock + Quantity;
            }

        }

        public float Value
        {
            get
            {
                int mul = Type == 'S' ? -1 : 1;
                return mul * Product.Price * Quantity;
            }
        }
        public float NewStockB { get; set; }
        public char Type { get; set; }
        public MovementPart()
        {

        }
        public MovementPart(char Type)
        {
            this.Type = Type;
        }
        public static async Task<MovementPart> GetById(int movementpId,char type)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GET_MOVEMENTPART_BY_ID",
                CommandType.StoredProcedure, new SqlParameter("ID", movementpId)))
            {
                if (reader.Read())
                {
                    return new MovementPart()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        IdMovement = Convert.ToInt32(reader[1]),
                        Product = await Product.GetById(Convert.ToInt32(reader[2])),
                        Quantity = Convert.ToSingle(reader[3]),
                        InitiallyStock = Convert.ToSingle(reader[4]),
                        NewStockB = Convert.ToSingle(reader[5]),
                        Type = type
                    };
                }
            }
            return new MovementPart();
        }

        public void Save(Movement movement)
        {
           this.Id= AppData.SQL.Single<int>("SP_ADD_MOVENT_PARTS", CommandType.StoredProcedure,
                new SqlParameter("MOVEMENT_ID", movement.Id),
                new SqlParameter("PRODUCT_ID", Product.Id),
                new SqlParameter("QUANTITY", Quantity),
                new SqlParameter("TYPE", Type)
            );
        }
    }
}
