using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DfE.Common.Application.UseCases.Mediator.Extensions
{
    /// <summary>
    /// Provides extension methods for registering use case handlers and a mediator into the dependency injection container.
    /// This class scans specified assemblies for implementations of <see cref="IUseCaseRequest{TResponse}"/> and
    /// <see cref="IUseCase{TRequest, TResponse}"/>, automatically registers them with the DI container, and sets
    /// up a mediator to resolve and dispatch requests to the appropriate handlers at runtime.
    /// </summary>

    public static class UseCaseMediatorExtensions
    {
        /// <summary>
        /// Registers all use case handlers and the mediator into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="lifetime">The lifetime of the use case handler services (default is Scoped).</param>
        /// <param name="assemblies">Assemblies to scan for use case handlers and requests.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddMediator(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, params Assembly[] assemblies)
        {
            const string InterfaceName = "IUseCase`2";                  // Name of the generic use case interface.
            var useCaseInfos = new ConcurrentDictionary<Type, Type>();  // Maps request types to handler types.

            foreach (Assembly assembly in assemblies)
            {
                // Find all types implementing IUseCaseRequest<>.
                List<Type> requests = GetClassesImplementingInterface(assembly, typeof(IUseCaseRequest<>));
                // Find all types implementing IUseCase<,>.
                List<Type> handlers = GetClassesImplementingInterface(assembly, typeof(IUseCase<,>));
                // Match each request to its corresponding handler.
                requests.ForEach(request =>
                    useCaseInfos[request] = handlers.SingleOrDefault(handler =>
                        request == handler.GetInterface(InterfaceName)!.GetGenericArguments()[0]));

                // Register each handler with the specified lifetime.
                IEnumerable<ServiceDescriptor> handlerServiceDescriptors =
                    handlers.Select(type => new ServiceDescriptor(type, type, lifetime));

                services.TryAdd(handlerServiceDescriptors); // Add handlers to DI container if not already registered.
            }

            // Register the mediator as a singleton, passing in the service provider and the request-handler map.
            services.AddSingleton<IUseCaseMediator>(serviceProvider =>
                new UseCaseMediator(serviceProvider.GetRequiredService, useCaseInfos));

            return services;
        }

        /// <summary>
        /// Finds all non-abstract, non-interface classes in the assembly that implement a specific generic interface.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="interfaceType">The generic interface type to look for.</param>
        /// <returns>A list of matching types.</returns>
        private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type interfaceType) =>
            assembly.ExportedTypes.Where(type =>
            {
                // Get all generic interfaces implemented by the type.
                IEnumerable<Type> genericInterfaceTypes =
                    type.GetInterfaces().Where(type => type.IsGenericType);

                // Check if any of the interfaces match the specified generic interface.
                bool hasRequestType =
                    genericInterfaceTypes.Any(type =>
                        type.GetGenericTypeDefinition() == interfaceType);

                // Return true if the type is a concrete class implementing the interface.
                return !type.IsInterface && !type.IsAbstract && hasRequestType;
            })
            .ToList();
    }
}
