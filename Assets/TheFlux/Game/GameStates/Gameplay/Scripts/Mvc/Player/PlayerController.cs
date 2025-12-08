using System.Collections.Generic;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement;
using TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player.PlayerMovement.Data;
using TheFlux.Game.Scripts.CombatSystem;
using TheFlux.Game.Scripts.Entities;
using UnityEngine;
using VContainer;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.Mvc.Player
{
    public class PlayerController : Entity
    {
        private readonly PlayerMovementController playerMovementController;
        private readonly AbilitySystemComponent abilitySystemComponent;
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
            abilitySystemComponent.InitEntryPoint(Id, attributeSets);
        }

        public Transform GetTransform()
        {
            return playerView.transform;
        }
    }
}