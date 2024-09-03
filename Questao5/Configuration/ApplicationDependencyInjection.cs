using MediatR;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Configuration
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
            services.AddMediatR(typeof(MovimentarContaHandler).Assembly);

            return services;
        }
    }
}
