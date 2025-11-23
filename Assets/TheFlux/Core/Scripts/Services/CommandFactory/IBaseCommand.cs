using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface IBaseCommand
    {
        void SetObjectResolver(Container diContainer);
        void ResolveDependencies();
    }
}