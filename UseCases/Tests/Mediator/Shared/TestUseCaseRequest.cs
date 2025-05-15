using DfE.Common.Application.UseCases;

namespace DfE.Common.Application.Tests.Mediator.Shared
{
    /// <summary>
    /// Defines a request that implements IUseCaseRequest<string>, meaning it expects a string response.
    /// </summary>
    /// <param name="name"></param>
    public class TestUseCaseRequest : IUseCaseRequest<string>
    {
        /// <summary>
        /// Initializes a new instance of the TestRequest class with the specified name.
        /// </summary>
        /// <param name="name">The string name result.</param>
        public TestUseCaseRequest(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The string name result.
        /// </summary>
        public string? Name { get; }
    }
}
