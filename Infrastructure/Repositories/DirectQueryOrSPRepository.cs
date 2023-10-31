using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.Interfaces.Repositories;
using RappidPayTest.Application.Wrappers;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RappidPayTest.Infrastructure.Repositories
{
    public class DirectQueryOrSPRepository : DapperBaseRepository, IDirectQueryOrSPRepository
    {

        public DirectQueryOrSPRepository(IDbConnection connection) : base(connection)
        {

        }


        public async Task<Response<SpResult>> SpTestResult(string nombre, string apellido)
        {

            var sSql = "dbo.GetInfoTestDapper";

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Nombre", nombre, DbType.String, ParameterDirection.Input);
            parameters.Add("@Apellido", apellido, DbType.String, ParameterDirection.Input);

            var dbResult = await base._connection.QueryAsync<SpResult>(sSql, commandType: CommandType.StoredProcedure, param: parameters);

            return new Response<SpResult>(dbResult.LastOrDefault());

        }




    }
}
