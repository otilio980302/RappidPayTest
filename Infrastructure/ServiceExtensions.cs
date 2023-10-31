using RappidPayTest.Infrastructure.Repositories;
using RappidPayTest.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using RappidPayTest.Application.Interfaces.Repositories;
using RappidPayTest.Application.Interfaces.Services;
using RappidPayTest.Infrastructure.Data;
using RappidPayTest.Infrastructure.Repositories;

namespace RappidPayTest.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PrincipalContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("default"),
                b => b.MigrationsAssembly(typeof(PrincipalContext).Assembly.FullName)));

            services.AddScoped<DbContext, PrincipalContext>();

            //SI necesito usar dapper o otro dataAccess solo es quitar este comentario//
            services.AddScoped<IDbConnection>(db =>
              new SqlConnection(configuration.GetConnectionString("default")));


            #region Repositories
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(EFRepository<>));
            services.AddScoped<IDirectQueryOrSPRepository, DirectQueryOrSPRepository>();
            #endregion

            #region Services 
            services.AddScoped<IPrioridadesService, PrioridadesService>();
            services.AddScoped<ICardManagementService, CardManagementService>();
            #endregion
        }
    }
}
