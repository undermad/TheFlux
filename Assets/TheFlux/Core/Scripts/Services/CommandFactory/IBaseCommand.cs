using VContainer;

namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface IBaseCommand
    {
        void SetObjectResolver(IObjectResolver objectResolver);
        void ResolveDependencies();
    }
}