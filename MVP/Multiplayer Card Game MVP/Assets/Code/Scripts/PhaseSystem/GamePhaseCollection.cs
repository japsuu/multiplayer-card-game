using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhaseSystem
{
    /// <summary>
    /// Contains multiple sub-phases.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Phase Collection", fileName = "Phases_", order = 0)]
    public class GamePhaseCollection : GamePhase
    {
        [SerializeField]
        private List<GamePhase> _subPhases;


        protected sealed override IEnumerator OnExecute()
        {
            foreach (GamePhase subPhase in _subPhases)
            {
                yield return subPhase.Execute();
            }
        }
    }
}