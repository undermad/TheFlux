using System.Threading;
using Cysharp.Threading.Tasks;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.StateMachineService;
using VContainer;

namespace TheFlux.Game.Scripts.Commands
{
    public class SwitchStateCommand : BaseCommand, ICommandVoid
    {
        private StateMachineService stateMachineService;
        private IGameState newGameState;

        public SwitchStateCommand SetNewGameState(IGameState newGameState)
        {
            this.newGameState = newGameState;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            stateMachineService = ObjectResolver.Resolve<StateMachineService>();
        }

        public void Execute()
        {
            stateMachineService.SwitchState(newGameState);
        }
    }
}