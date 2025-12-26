using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services , IConfiguration  configuration )
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           
        
            //Apply Unit Of Work
            services.AddScoped<IUnitOfWork ,UnitOfWork>();

            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            });

            return services;
        }


    }
}
