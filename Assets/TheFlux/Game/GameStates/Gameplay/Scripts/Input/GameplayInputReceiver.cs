using System;
using MessagePipe;
using TheFlux.Core.Scripts.Events;
using TheFlux.Core.Scripts.Mvc.InputSystem.InputActions.GameplayInputActions;
using TheFlux.Core.Scripts.Services.CommandFactory;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.Commands;
using TheFlux.Game.Scripts.Input;
using VContainer;
using VContainer.Unity;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Input
{
    public class GameplayInputReceiver : IInputReceiver, IStartable, IDisposable
    {
        private readonly ISubscriber<IInputKeyPressedEvent> gameplayInputSubscriber;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        [Inject]
        public GameplayInputReceiver(ISubscriber<IInputKeyPressedEvent> gameplayInputSubscriber,
            CommandFactory commandFactory)
        {
            this.gameplayInputSubscriber = gameplayInputSubscriber;
            this.commandFactory = commandFactory;
        }

        public void SetupSubscriptions()
        {
            var bag = DisposableBag.CreateBuilder();
            gameplayInputSubscriber.Subscribe(HandleInputKeyPressedEvent).AddTo(bag);
            disposable = bag.Build();
        }

        private void HandleInputKeyPressedEvent(IInputKeyPressedEvent payload)
        {
            switch (payload)
            {
                case GameplayInputKeyPressed e:
                    OnGameInputKeyPressedHandler(e);
                    break;
                case UIInputKeyPressed e:
                    OnUIInputKeyPressedHandler(e);
                    break;
            }
        }

        private void OnGameInputKeyPressedHandler(GameplayInputKeyPressed message)
        {
            switch (message.ActionType)
            {
                case ActionsType.GAMEPLAY_MENU_OPEN:
                    LogService.Log("Opening Menu", LogLevel.Info, LogCategory.UI);
                    commandFactory.CreateCommandVoid<OpenMenuCommand>()
                        .Execute();
                    break;
            }
        }
        
        private void OnUIInputKeyPressedHandler(UIInputKeyPressed message)
        {
            switch (message.ActionType)
            {
                case ActionsType.UI_MENU_CLOSE:
                    commandFactory.CreateCommandVoid<CloseMenuCommand>()
                        .Execute();
                    break;
            }
        }

        public void Start()
        {
            SetupSubscriptions();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}