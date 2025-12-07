using CleanArchitecture.Mediator.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Mediator.AspNetCore;

/// <summary>
/// Provides extension methods for registering the Mediator and related handlers in ASP.NET Core.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers Mediator, handlers, and pipeline behaviors from the specified assemblies.
        /// </summary>
        /// <param name="services">The service collection to add registrations to.</param>
        /// <param name="assemblies">
        /// Assemblies to scan for ICommandHandler, IQueryHandler, and IPipelineBehavior implementations.
        /// </param>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        public IServiceCollection AddMediator(params Assembly[] assemblies)
        {
            // Register the Mediator itself
            services.AddSingleton<IMediator, Mediator>();

            // Scan and register command handlers
            var commandHandlerType = typeof(ICommandHandler<,>);
            foreach (var assembly in assemblies)
            {
                var handlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
                    .Where(x => x.i.IsGenericType && x.i.GetGenericTypeDefinition() == commandHandlerType);

                foreach (var handler in handlers)
                {
                    services.AddScoped(handler.i, handler.t);
                }
            }

            // Scan and register query handlers
            var queryHandlerType = typeof(IQueryHandler<,>);
            foreach (var assembly in assemblies)
            {
                var handlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
                    .Where(x => x.i.IsGenericType && x.i.GetGenericTypeDefinition() == queryHandlerType);

                foreach (var handler in handlers)
                {
                    services.AddScoped(handler.i, handler.t);
                }
            }

            // Scan and register pipeline behaviors
            var pipelineBehaviorType = typeof(IPipelineBehavior<,>);
            foreach (var assembly in assemblies)
            {
                var behaviors = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
                    .Where(x => x.i.IsGenericType && x.i.GetGenericTypeDefinition() == pipelineBehaviorType);

                foreach (var behavior in behaviors)
                {
                    services.AddScoped(behavior.i, behavior.t);
                }
            }

            return services;
        }
    }
}