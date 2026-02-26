using System;
using System.Collections.Generic;
using System.Text;

namespace CabarlesWpf.HostBuilders
{
    internal class AddDbContextHostBuilderExtensions
    {

        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<MoviesDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient);
            });

            return hostBuilder;
        }
    }
}

