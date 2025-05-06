using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.Persistence.Context;
using Evenda.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evenda.Persistence.Registrar
{
    public static class PersistenceLayerRegistrar
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            #region Repositories

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            #endregion

            #region UnitOfWork

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            #endregion


            return services;
        }
    }
}
