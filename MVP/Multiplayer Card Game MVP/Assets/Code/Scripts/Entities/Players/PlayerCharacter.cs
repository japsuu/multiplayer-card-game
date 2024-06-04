using System;
using Cards;
using DamageSystem;
using PhaseSystem;
using UnityEngine;

namespace Entities.Players
{
    /// <summary>
    /// Represents a player character on the board.
    /// </summary>
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCharacter : BoardEntity
    {
        public static event Action<PlayerCharacter> LocalPlayerCreated;
        public static event Action LocalPlayerDestroyed;
        public static PlayerCharacter LocalPlayer { get; private set; }
        
        public PlayerMovement Movement { get; private set; }


        public static void SetLocalPlayer(PlayerCharacter player)
        {
            LocalPlayer = player;
            LocalPlayerCreated?.Invoke(player);
        }


        protected override void OnHealthChanged(HealthChangedArgs args)
        {
            base.OnHealthChanged(args);

            if (args.IsDamage)
            {
                PlayerHandManager.Instance.InvokeOnAttacked(Health.LastDamagingEntity, args.HealthDifference);
            }
        }


        protected override void OnDied()
        {
            base.OnDied();
            
            GameLoopManager.Instance.StopGameLoop();
        }

        
        protected override void Awake()
        {
            base.Awake();
            
            Movement = GetComponent<PlayerMovement>();
        }


        private void OnDestroy()
        {
            LocalPlayerDestroyed?.Invoke();
            LocalPlayer = null;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerManager.Add(this);
        }
        
        
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerManager.Remove(this);
        }
    }
}