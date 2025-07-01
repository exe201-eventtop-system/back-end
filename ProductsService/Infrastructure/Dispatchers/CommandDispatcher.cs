using Application.Commons.Dispatchers.Commands;
using Application.Commons.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dispatchers
{
    public class CommandDispatcher: ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
            return handler.Handle(command, cancellationToken);
        }
    }
}
