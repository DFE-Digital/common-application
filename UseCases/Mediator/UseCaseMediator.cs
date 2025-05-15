using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DfE.Common.Application.UseCases.Mediator
{
    /// <summary>
    /// A concrete implementation of the IUsecaseMediator interface.
    /// This mediator is responsible for dispatching requests to their corresponding UseCases.
    /// It uses a service factory to resolve UseCase's from the dependency injection container and a
    /// dictionary to map request types to UseCase types.
    /// </summary>
    public sealed class UseCaseMediator : IUseCaseMediator
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly ConcurrentDictionary<Type, Type> _useCaseInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseMediator"/> class.
        /// </summary>
        /// <param name="serviceFactory">A delegate used to resolve services from the DI container.</param>
        /// <param name="useCaseInfos">A mapping of request types to their corresponding handler types.</param>
        public UseCaseMediator(ServiceFactory serviceFactory, ConcurrentDictionary<Type, Type> useCaseInfos)
        {
            _serviceFactory = serviceFactory;
            _useCaseInfos = useCaseInfos;
        }

        /// <summary>
        /// Sends a use case request to the appropriate UseCase and returns the response.
        /// </summary>
        /// <typeparam name="TUseCaseRequest">The type of the request.</typeparam>
        /// <typeparam name="TUseCaseResponse">The type of the response.</typeparam>
        /// <param name="request">The request instance to be handled.</param>
        /// <returns>A task representing the asynchronous operation, containing the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the request is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if no UseCase is found for the request type.</exception>
        public Task<TUseCaseResponse> Send<TUseCaseRequest, TUseCaseResponse>(TUseCaseRequest request)
            where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>
        {
            if (request is null){
                throw new ArgumentNullException(nameof(request));
            }

            // Get the runtime type of the request.
            Type requestType = request.GetType();

            // Check if a UseCase is registered for this request type.
            if (!_useCaseInfos.ContainsKey(requestType)){
                throw new InvalidOperationException($"No UseCase found for {requestType.FullName}");
            }

            // Get the UseCase type from the dictionary
            Type useCaseType = _useCaseInfos[requestType];

            // Resolve the UseCase from the service factory and cast it to the correct interface.
            IUseCase<TUseCaseRequest, TUseCaseResponse> useCase =
                _serviceFactory.GetInstanceByType<IUseCase<TUseCaseRequest, TUseCaseResponse>>(useCaseType);

            // Invoke the UseCase's method and return the result.
            return useCase.HandleRequest(request);
        }
    }
}
