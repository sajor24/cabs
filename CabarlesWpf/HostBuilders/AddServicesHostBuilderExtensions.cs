using System;
using System.Collections.Generic;
using System.Text;

namespace CabarlesWpf.HostBuilders
{
    internal class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MovieStore>();

                services.AddTransient<ICommandHandler<CreateMovieCommand>, CreateMovieCommandHandler>();
                services.AddTransient<ICommandHandler<UpdateMovieCommand>, UpdateMovieCommandHandler>();
                services.AddTransient<ICommandHandler<DeleteMovieCommand>, DeleteMovieCommandHandler>();
                services.AddTransient<IQueryHandler<GetAllMoviesQuery, List<Movie>>, GetAllMoviesQueryHandler>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            });

            return hostBuilder;
        }
    }
}
