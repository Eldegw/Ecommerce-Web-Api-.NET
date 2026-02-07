using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ecom.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

          
            optionsBuilder.UseSqlServer(
                "Data Source=.;Initial Catalog=Ecom;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}