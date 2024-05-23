using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace PhaseSystem.Enemies
{
    /// <summary>
    /// Contains the logic for the enemies' turn.
    /// The logic is separated to multiple sub-phases.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Enemies Phase", fileName = "Phase_Enemies", order = 0)]
    public class EnemiesPhase : GamePhase
    {
        [SerializeField]
        private List<GamePhase> _subPhases;

        public override string Name => "Enemies' Phase";
        
        
        public override IEnumerator Execute()
        {
            yield return PhaseBanner.Instance.DisplayPhase(Name);

            foreach (GamePhase subPhase in _subPhases)
            {
                yield return subPhase.Execute();
            }
        }
    }
}