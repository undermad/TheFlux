namespace TheFlux.Core.Scripts.Services.CommandFactory
{
    public interface ICommandWithResult<T> : IBaseCommand
    {
        T Execute();
    }
}