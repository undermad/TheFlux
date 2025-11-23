using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public class CommandFactory : ICommandFactory
    {
        private readonly Container diContainer;

        public CommandFactory(Container diContainer)
        {
            this.diContainer = diContainer;
        }
        
        public T CreateCommandVoid<T>() where T : ICommandVoid, new()
        {
            var command = new T();
            command.SetObjectResolver(diContainer);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandWithResult<T,V>() where T : ICommandWithResult<V>, new()
        {
            var command = new T();
            command.SetObjectResolver(diContainer);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandAsync<T>() where T : ICommandAsync, new()
        {
            var command = new T();
            command.SetObjectResolver(diContainer);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandAsyncWithResult<T,V>() where T : ICommandAsyncWithResult<V>, new()
        {
            var command = new T();
            command.SetObjectResolver(diContainer);
            command.ResolveDependencies();
            return command;
        }
    }
}