using System.Collections;
using UI;
using UnityEngine;

namespace PhaseSystem
{
    public abstract class SequentialPhase<T> : ScriptableObject
    {
        [SerializeField]
        private string _name = "Unnamed Phase";

        [SerializeField]
        private bool _displayBanner = true;

        public virtual string Name => _name;


        public IEnumerator Execute(T instance)
        {
            if (_displayBanner)
                yield return PhaseBanner.Instance.DisplayPhase(Name);

            yield return OnEnter(instance);
            yield return OnExecute(instance);
            yield return OnExit(instance);
        }
        
        
        protected virtual IEnumerator OnEnter(T instance) { yield return null; }
        protected abstract IEnumerator OnExecute(T instance);
        protected virtual IEnumerator OnExit(T instance) { yield return null; }
    }
}