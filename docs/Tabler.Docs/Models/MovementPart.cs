using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;

namespace Tabler.Docs.Models
{
    public class MovementPart
    {
        public int Id { get; set; }
        public int IdMovement { get; set; }
        public Product Product { get; set; }
        public float Quantity { get; set; }
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
        public float NewStockB { get; set; }
        public char Type { get; set; }
        public MovementPart()
        {

        }
        public MovementPart(char Type)
        {
            this.Type = Type;

        }
        public static async Task<List<MovementPart>> GetAll()
        {
            await Task.Yield();
            List<MovementPart> movementsparts = new List<MovementPart>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLMOVEMENTPART"))
            {
                movementsparts.Add(await GetById(id));
            }
            return movementsparts;
        }
        public static async Task<MovementPart> GetById(int movementpId)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GET_MOVEMENTPART",
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
                        Type = Convert.ToChar(reader[6])
                    };
                }
            }
            return new MovementPart();
        }
    }
}
