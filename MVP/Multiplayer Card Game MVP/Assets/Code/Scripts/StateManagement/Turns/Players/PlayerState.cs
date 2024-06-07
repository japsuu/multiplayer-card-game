using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public abstract class PlayerState : StateBase
    {
        protected abstract bool ShowHand { get; }
        protected abstract bool AllowMovement { get; }
        protected abstract bool AllowCardDiscard { get; }
        protected abstract bool AllowCardActivation { get; }
        protected abstract bool AllowCardPlay { get; }
        protected abstract bool AllowSkip { get; }
        protected abstract bool AllowEndTurn { get; }
        
        
        public PlayerState(bool needsExitTime) : base(needsExitTime)
        {
            
        }


        public override void OnEnter()
        {
            UpdateGameState();
        }


        private void UpdateGameState()
        {
            // TODO: Compare abstract values to GameState values. If changed, update them.
        }
    }
}