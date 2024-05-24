using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhaseSystem
{
    /// <summary>
    /// Contains multiple sub-phases. All phases are executed sequentially.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Phase Collection", fileName = "Phases_", order = 0)]
    public abstract class SequentialPhaseCollection<T> : GamePhase
    {
        [SerializeField]
        private List<SequentialPhase<T>> _subPhases;
        
        protected abstract IEnumerable<T> Instances { get; }


        protected sealed override IEnumerator OnExecute()
        {
            foreach (T instance in Instances)
            {
                foreach (SequentialPhase<T> subPhase in _subPhases)
                {
                    yield return subPhase.Execute(instance);
                }
            }
        }
    }
}