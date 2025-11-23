using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public abstract class BaseCommand : IBaseCommand
    {
        protected Container DiContainer;

        public void SetObjectResolver(Container diContainer)
        {
            DiContainer = diContainer;
        }

        public abstract void ResolveDependencies();
    }
}