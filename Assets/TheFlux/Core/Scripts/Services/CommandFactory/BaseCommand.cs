using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public abstract class BaseCommand : IBaseCommand
    {
        protected IObjectResolver ObjectResolver;

        public void SetObjectResolver(IObjectResolver objectResolver)
        {
            ObjectResolver = objectResolver;
        }

        public abstract void ResolveDependencies();
    }
}