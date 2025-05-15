using DfE.Common.Application.Tests.Mediator.Shared;
using DfE.Common.Application.UseCases.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace DfE.Common.Application.Tests
{
    public class UseCaseMediatorTests
    {
        [Fact]
        public async Task Send_Should_Invoke_Handler_And_Return_Response()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<TestUseCase>(); // Register the handler manually

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            // Create the service factory using the service provider
            ServiceFactory factory = serviceProvider.GetRequiredService;
            // Map the request type to the handler type
            var useCaseInfos = new ConcurrentDictionary<Type, Type>();
            useCaseInfos[typeof(TestUseCaseRequest)] = typeof(TestUseCase);

            var mediator = new UseCaseMediator(factory, useCaseInfos);
            var request = new TestUseCaseRequest("Fred");

            // Act
            string result = await mediator.Send<TestUseCaseRequest, string>(request);

            // Assert
            Assert.Equal("Hello, Fred", result);
        }

        [Fact]
        public Task Send_WithNullRequest_ThrowsExpectedArgumentNullException()
        {
            // Arrange  
            var services = new ServiceCollection();
            services.AddScoped<TestUseCase>(); // Register the handler manually

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            // Create the service factory using the service provider
            ServiceFactory factory = serviceProvider.GetRequiredService;
            // Map the request type to the handler type
            var useCaseInfos = new ConcurrentDictionary<Type, Type>();
            useCaseInfos[typeof(TestUseCaseRequest)] = typeof(TestUseCase);

            var mediator = new UseCaseMediator(factory, useCaseInfos);

            // Act, assert 
            return Assert.ThrowsAsync<ArgumentNullException>(async () =>
                 await mediator.Send<TestUseCaseRequest, string>(request: null!));
        }

        [Fact]
        public Task Send_WithUnmatchedUseCaseRequest_ThrowsExpectedArgumentNullException()
        {
            // Arrange  
            var services = new ServiceCollection();
            services.AddScoped<TestUseCase>(); // Register the handler manually

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            // Create the service factory using the service provider
            ServiceFactory factory = serviceProvider.GetRequiredService;
            // Map the request type to the handler type
            var useCaseInfos = new ConcurrentDictionary<Type, Type>();

            var mediator = new UseCaseMediator(factory, useCaseInfos);
            var request = new TestUseCaseRequest("Fred");

            // Act, assert 
            return Assert.ThrowsAsync<InvalidOperationException>(async () =>
                 await mediator.Send<TestUseCaseRequest, string>(request));
        }
    }
}