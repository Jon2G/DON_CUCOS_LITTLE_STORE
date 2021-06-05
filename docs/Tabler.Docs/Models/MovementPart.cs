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
        public char Type { get; set; }
        public MovementPart(char Type)
        {
            this.Type = Type;

        }
        public async Task Save()
        {
            await Task.Yield();
            AppData.SQL.EXEC("SP_ADD_MOVEMENT", CommandType.StoredProcedure,
                new SqlParameter("MOVEMENT_ID", IdMovement),
                new SqlParameter("PRODUCT_ID", Product.Id),
                new SqlParameter("QUANTITY", Quantity)
            );
        }
    }
}
