using System.Threading.Tasks;

namespace DfE.Common.Application.UseCases.Mediator
{
    /// <summary>
    /// Defines a mediator interface for sending use case requests and receiving responses.
    /// This abstraction allows decoupling of request senders from their handlers by routing
    /// requests to the appropriate use case handler registered in the dependency injection container.
    /// </summary>
    public interface IUseCaseMediator
    {
        /// <summary>
        /// Sends a use case request and returns the corresponding response.
        /// </summary>
        /// <typeparam name="TUseCaseRequest">
        /// The type of the request, implementing <see cref="IUseCaseRequest{TUseCaseResponse};"/>.
        /// </typeparam>
        /// <typeparam name="TUseCaseResponse">
        /// The type of the response returned by the UseCase handler.</typeparam>
        /// <param name="request">The use case request to send.</param>
        /// <returns>A task representing the asynchronous operation, containing the response.</returns>
        Task<TUseCaseResponse> Send<TUseCaseRequest, TUseCaseResponse>(TUseCaseRequest request)
            where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>;
    }
}