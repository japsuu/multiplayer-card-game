using System.Collections;
using UI;
using UnityEngine;

namespace PhaseSystem
{
    public abstract class GamePhase : ScriptableObject
    {
        [SerializeField]
        private string _name = "Unnamed Phase";

        [SerializeField]
        private bool _displayBanner = true;

        public virtual string Name => _name;


        public IEnumerator Execute()
        {
            if (_displayBanner)
                yield return PhaseBanner.Instance.DisplayPhase(Name);

            yield return OnEnter();
            yield return OnExecute();
            yield return OnExit();
        }
        
        
        protected virtual IEnumerator OnEnter() { yield return null; }
        protected abstract IEnumerator OnExecute();
        protected virtual IEnumerator OnExit() { yield return null; }
    }
}