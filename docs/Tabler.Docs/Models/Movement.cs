using Kit.Sql.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabler.Docs.Data;
using Tabler.Docs.ViewModels;

namespace Tabler.Docs.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public User User { get; set; }
        public MovementConcept Concept { get; set; }
        public char Type { get; set; }
        public DateTime Date_m { get; set; }
        public string Observations { get; set; }

        public List<MovementPart> Parts { get; set; }
        public Movement()
        {
            Parts = new List<MovementPart>();
        }

        public async Task Save()
        {
            await Task.Yield();
            this.Id = AppData.SQL.Single<int>("SP_ADD_MOVEMENT", CommandType.StoredProcedure,
                    new SqlParameter("USER_ID", AppData.Current.User.Id),
                    new SqlParameter("MOVEMENT_CONCEPT_ID", Concept.Id),
                    new SqlParameter("TYPE", Concept.Type),
                    new SqlParameter("DATE_M", DateTime.Now),
                    new SqlParameter("DESCRIPTION", Observations));
            Parts.ForEach(x => x.Save(this));
        }
        public static async Task<List<Movement>> GetAll(char type)
        {
            await Task.Yield();
            List<Movement> movements = new List<Movement>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLMOVEMENTS WHERE TYPE=@TYPE",
                new SqlParameter("TYPE", type)))
            {
                movements.Add(await GetById(id));
            }
            return movements;
        }

        public static async Task<Movement> GetById(int movementId)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GET_MOVEMENT",
                CommandType.StoredProcedure, new SqlParameter("ID", movementId)))
            {
                if (reader.Read())
                {
                    return new Movement()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Concept = await MovementConcept.GetById(Convert.ToInt32(reader[1])),
                        User = await User.GetById(Convert.ToInt32(reader[2])),
                        Type = Convert.ToChar(reader[3]),
                        Date_m = Convert.ToDateTime(reader[4]),
                        Observations = Convert.ToString(reader[5])
                    };
                }
            }
            return new Movement();
        }

        public async Task Load()
        {
            await Task.Yield();
            this.Parts.Clear();
            foreach (int idpart in AppData.SQL.Lista<int>("SP_GET_MOVEMENTPART",CommandType.StoredProcedure,0, new SqlParameter("ID", this.Id)))
            {
                Parts.Add(await MovementPart.GetById(idpart,this.Type));
            }

        }
    }
}
