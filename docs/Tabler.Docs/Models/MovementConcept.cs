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
    public class MovementConcept
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }

        public static async Task<MovementConcept> GetById(int movementId)
        {
            await Task.Yield();
            using (IReader reader = AppData.SQL.Read("SP_GETIDMOVEMENTS",
                CommandType.StoredProcedure, new SqlParameter("ID", movementId)))
            {
                if (reader.Read())
                {
                    return new MovementConcept()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Tag = Convert.ToString(reader[1]),
                        Description = Convert.ToString(reader[2])

                    };
                }
            }
            return new MovementConcept();
        }

        public async Task Save()
        {
            await Task.Yield();
            AppData.SQL.EXEC("SP_ADD_MOVEMENTCONCEPT", CommandType.StoredProcedure,
                new SqlParameter("TAG", Tag),
                new SqlParameter("DESCRIPTION", Description)
            );
        }

        public static async Task<List<MovementConcept>> GetAll()
        {
            await Task.Yield();
            List<MovementConcept> movementConcepts = new List<MovementConcept>();
            foreach (int id in AppData.SQL.Lista<int>("SELECT *FROM VIEW_GETALLMOVEMENTCONCEPT", 0))
            {
                movementConcepts.Add(await GetById(id));
            }
            return movementConcepts;
        }
    }
}
