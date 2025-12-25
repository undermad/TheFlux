using System;
using TheFlux.Core.Scripts.Services.LogService;

namespace TheFlux.Game.Scripts.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            LogService.Log($"Called entity constructor: {Id}");
        }
    }
}