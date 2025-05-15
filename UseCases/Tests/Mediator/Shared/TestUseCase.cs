using DfE.Common.Application.UseCases;

namespace DfE.Common.Application.Tests.Mediator.Shared
{
    /// <summary>
    /// Implements the use case handler for <see cref="TestUseCaseRequest"/>, returning a greeting string.
    /// </summary>
    public class TestUseCase : IUseCase<TestUseCaseRequest, string>
    {
        /// <summary>
        /// Handles the request and returns a greeting message.
        /// </summary>
        /// <param name="request">
        /// Instance of the <see cref="TestUseCaseRequest"/> class with the specified name.
        /// </param>
        /// <returns>The string name result.</returns>
        public Task<string> HandleRequest(TestUseCaseRequest request) =>
            Task.FromResult($"Hello, {request.Name}");
    }
}
