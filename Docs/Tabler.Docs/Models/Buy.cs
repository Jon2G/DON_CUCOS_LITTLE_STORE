using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CucoStore.Docs.Data;
using CucoStore.Docs.ViewModels;

namespace CucoStore.Docs.Models
{
    public class Buy
    {
        public int Id { get; set; }
        public User User { get; set; }

        public DateTime Date { get; set; }
        public string Observations { get; set; }

        public List<BuyPart> Parts { get; set; }
        public Buy()
        {
            Parts = new List<BuyPart>();
            User = new User();
        }

        public async Task Save()
        {
            await Task.Yield();
            this.Id = AppData.SQL.Single<int>("SP_ADD_BUY", CommandType.StoredProcedure,
                    new SqlParameter("USER_ID", AppData.Current.User.Id),
                    new SqlParameter("DATE", DateTime.Now),
                    new SqlParameter("DESCRIPTION", Observations??String.Empty));
            Parts.ForEach(x => x.Save(this));
            await SaveMovement();
        }
        private async Task SaveMovement()
        {
            Movement movement = new Movement
            {
                Concept = await MovementConcept.GetById(1),
                Date = DateTime.Now,
                Id = 0,
                Observations = $"Compra #{this.Id}",
                Type = 'E',
                Parts = new List<MovementPart>(Parts.Select(x => new MovementPart('E')
                {
                    Id = 0,
                    IdMovement = 0,
                    InitiallyStock = x.Product.Stock,
                    NewStockB = x.Product.Stock + x.Quantity,
                    Type = 'E',
                    Quantity = x.Quantity,
                    Product = x.Product
                }))
            };
            await movement.Save();
        }
        public static async Task<List<Buy>> GetAll()
        {
            await Task.Yield();
            List<Buy> buys = new List<Buy>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GET_ALL_BUYS"))
            {
                buys.Add(await GetById(id));
            }
            return buys;
        }

        public static async Task<Buy> GetById(int movementId)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GET_BUY_BY_ID",
                CommandType.StoredProcedure, new SqlParameter("ID", movementId)))
            {
                if (reader.Read())
                {
                    return new Buy()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        User = await User.GetById(Convert.ToInt32(reader[1])),
                        Date = Convert.ToDateTime(reader[2]),
                        Observations = Convert.ToString(reader[3])
                    };
                }
            }
            return new Buy();
        }

        public async Task Load()
        {
            await Task.Yield();
            this.Parts.Clear();
            foreach (int idpart in AppData.SQL.Lista<int>("SP_GET_BUY_PARTS_BY_ID", CommandType.StoredProcedure,0, new SqlParameter("ID", this.Id)))
            {
                Parts.Add(await BuyPart.GetById(idpart));
            }

        }
    }
}
