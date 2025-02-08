using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;

namespace Hackaton.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUsuarioRoleRepository, UsuarioRoleRepository>();

            return services;
        }
    }
}
