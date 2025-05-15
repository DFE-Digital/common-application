using DfE.Common.Application.Tests.Mediator.Shared;
using DfE.Common.Application.UseCases.Mediator;
using DfE.Common.Application.UseCases.Mediator.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DfE.Common.Application.Tests
{
    public class UseCaseMediatorExtensionsTests
    {
        [Fact]
        public void AddMediator_Should_Register_UseCase_And_Mediator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMediator(ServiceLifetime.Scoped, typeof(TestUseCase).Assembly);
            var provider = services.BuildServiceProvider();

            // Assert
            //
            // Check that the handler is registered
            Assert.NotNull(provider.GetService<TestUseCase>());
            // Check that the mediator is registered
            Assert.NotNull(provider.GetService<IUseCaseMediator>());
            // Check that the mediator can resolve and execute the use case
            string? result =
                provider.GetService<IUseCaseMediator>()?
               .Send<TestUseCaseRequest, string>(new TestUseCaseRequest("Test"))?.Result;

            Assert.Equal("Hello, Test", result);
        }
    }
}