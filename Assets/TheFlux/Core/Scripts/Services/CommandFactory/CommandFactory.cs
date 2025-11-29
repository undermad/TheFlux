using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IObjectResolver objectResolver;

        [Inject]
        public CommandFactory(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }
        
        public T CreateCommandVoid<T>() where T : ICommandVoid, new()
        {
            var command = new T();
            command.SetObjectResolver(objectResolver);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandWithResult<T,V>() where T : ICommandWithResult<V>, new()
        {
            var command = new T();
            command.SetObjectResolver(objectResolver);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandAsync<T>() where T : ICommandAsync, new()
        {
            var command = new T();
            command.SetObjectResolver(objectResolver);
            command.ResolveDependencies();
            return command;
        }
        
        public T CreateCommandAsyncWithResult<T,V>() where T : ICommandAsyncWithResult<V>, new()
        {
            var command = new T();
            command.SetObjectResolver(objectResolver);
            command.ResolveDependencies();
            return command;
        }
    }
}