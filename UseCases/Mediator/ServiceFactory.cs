using System;

namespace DfE.Common.Application.UseCases.Mediator
{
    /// <summary>
    /// A delegate that defines a factory method for resolving services by type.
    /// This is used to abstract the service resolution logic, typically backed by a DI container.
    /// </summary>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <returns>The resolved service instance.</returns>
    public delegate object ServiceFactory(Type serviceType);

    /// <summary>
    /// Provides extension methods for the ServiceFactory delegate.
    /// </summary>
    internal static class ServiceFactoryExtensions
    {
        /// <summary>
        /// Resolves a service using the factory and casts it to the specified type.
        /// </summary>
        /// <typeparam name="TCast">The type to cast the resolved service to.</typeparam>
        /// <param name="factory">The service factory delegate.</param>
        /// <param name="type">The type of service to resolve.</param>
        /// <returns>The resolved and casted service instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided type is null.</exception>
        public static TCast GetInstanceByType<TCast>(this ServiceFactory factory, Type type)
        {
            if (type is null){
                throw new ArgumentNullException(nameof(type));
            }

            // Resolve the service and cast it to the desired type.
            return (TCast)factory(type);
        }
    }
}
