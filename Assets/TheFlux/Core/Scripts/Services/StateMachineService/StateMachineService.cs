using TheFlux.Core.Scripts.Mvc.LoadingScreen;
using VContainer;

namespace TheFlux.Core.Scripts.Services.StateMachineService
{
    public class StateMachineService
    {
        private LoadingScreenController loadingScreenController;

        [Inject]
        public StateMachineService(LoadingScreenController loadingScreenController)
        {
            this.loadingScreenController = loadingScreenController;
        }
        
        
        
        
        
        
    }
}