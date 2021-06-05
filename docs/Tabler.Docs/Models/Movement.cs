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
    public class Movement
    {
        public int Id { get; set; }
        public User User { get; set; }
        public MovementConcept Concept { get; set; }
        public char Type { get; set; }
        public DateTime Date_m { get; set; }
        public string Observations { get; set; }
        public MovementConcept MC { get; set; }
        public List<MovementPart> Parts { get; set; }
        public Movement()
        {
            Parts = new List<MovementPart>();
        }
        public async Task Save()
        {
            await Task.Yield();
            AppData.SQL.EXEC("SP_ADD_MOVEMENT", CommandType.StoredProcedure,
                new SqlParameter("USER_ID", AppData.Current.User.Id),
                new SqlParameter("MOVEMENT_CONCEPT_ID", Concept.Id),
                new SqlParameter("TYPE", Type),
                new SqlParameter("DATE_M", Date_m),
                new SqlParameter("DESCRIPTION", Observations)
            );
            MovementConcept movementConcept = await MovementConcept.GetById(Concept.Id);
            this.MC.Id = movementConcept.Id;
            this.MC.Save();

        }
        public static async Task<List<Movement>> GetAll()
        {
            await Task.Yield();
            List<Movement> movements= new List<Movement>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLMOVEMENTS"))
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
                        Concept =await MovementConcept.GetById(Convert.ToInt32(reader[1])),
                        User =await User.GetById(Convert.ToInt32(reader[2])),
                        Type = Convert.ToChar(reader[3]),
                        Date_m = Convert.ToDateTime(reader[4]),
                        Observations = Convert.ToString(reader[5])
                    };
                }
            }
            return new Movement();
        }
    }
}
