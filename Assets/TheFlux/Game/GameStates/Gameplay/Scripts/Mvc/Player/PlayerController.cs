using System.Collections.Generic;
using TheFlux.Core.Scripts.Services.LogService;
using TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement.Data;
using TheFlux.Game.Scripts.Entities;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player
{
    public class PlayerController : Entity
    {
        private readonly PlayerMovementController playerMovementController;
        private AbilitySystemComponent abilitySystemComponent { get; }
        private PlayerMovementData playerMovementData;
        
        private PlayerView playerView;
        
        [Inject]
        public PlayerController(PlayerMovementController playerMovementController, AbilitySystemComponent abilitySystemComponent)
        {
            this.playerMovementController = playerMovementController;
            this.abilitySystemComponent = abilitySystemComponent;
        }

        public void InitEntryPoint(PlayerView playerView, PlayerMovementData playerMovementData, List<AttributeSetData> attributeSets)
        {
            this.playerView = playerView;
            playerMovementController.InitEntryPoint(playerMovementData, playerView);
            LogService.Log($"Called Player Controller Init - Id: {Id}");
            abilitySystemComponent.InitEntryPoint(Id, attributeSets);
        }
        
        public void Resume()
        {
            abilitySystemComponent.Resume();
        }

        public void Pause()
        {
            abilitySystemComponent.Pause();
        }

        public Transform GetTransform()
        {
            return playerView.transform;
        }
    }
}