using UnityHFSM;

namespace StateManagement.Turns.Players
{
    public abstract class PlayerState : StateBase
    {
        protected virtual bool ShouldShowHand => false;
        protected virtual bool AllowMovement => false;
        protected virtual bool AllowCardDiscard => false;
        protected virtual bool AllowCardActivation => false;
        protected virtual bool AllowCardPlay => false;
        protected virtual bool AllowSkip => false;
        protected virtual bool AllowEndTurn => false;


        protected PlayerState(bool needsExitTime = true, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
            
        }


        public sealed override void OnEnter()
        {
            UpdateGameState();
            
            OnEnterState();
        }
        
        
        public sealed override void OnExit()
        {
            OnExitState();
        }
        
        
        protected virtual void OnEnterState()
        {
            // Override this method to add custom logic when entering the state
        }
        
        
        protected virtual void OnExitState()
        {
            // Override this method to add custom logic when exiting the state
        }


        private void UpdateGameState()
        {
            // Compare the abstract values to GameState values. If different, update GameState.
            if (ShouldShowHand != GameState.ShowHand)
                GameState.SetShowHand(ShouldShowHand);
            
            if (AllowMovement != GameState.AllowMovement)
                GameState.SetAllowMovement(AllowMovement);
            
            if (AllowCardDiscard != GameState.AllowCardDiscard)
                GameState.SetAllowCardDiscard(AllowCardDiscard);
            
            if (AllowCardActivation != GameState.AllowCardActivation)
                GameState.SetAllowCardActivation(AllowCardActivation);
            
            if (AllowCardPlay != GameState.AllowCardPlay)
                GameState.SetAllowCardPlay(AllowCardPlay);
            
            if (AllowSkip != GameState.AllowSkip)
                GameState.SetAllowSkip(AllowSkip);
            
            if (AllowEndTurn != GameState.AllowEndTurn)
                GameState.SetAllowEndTurn(AllowEndTurn);
        }
    }
}