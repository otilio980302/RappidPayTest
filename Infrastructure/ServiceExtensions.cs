using RapidPayTest.Infrastructure.Repositories;
using RapidPayTest.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using RapidPayTest.Application.Interfaces.Repositories;
using RapidPayTest.Application.Interfaces.Services;
using RapidPayTest.Infrastructure.Data;
using RapidPayTest.Infrastructure.Repositories;
using RapidPayTest.Application.Interfaces.Services.Security;
using RapidPayTest.Identity.Services;

namespace RapidPayTest.Infrastructure
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
            services.AddScoped<ICardManagementRepository, CardManagementRepository>();
            #endregion

            #region Services 
            services.AddScoped<ICardManagementService, CardManagementService>();
            #endregion
        }
    }
}
