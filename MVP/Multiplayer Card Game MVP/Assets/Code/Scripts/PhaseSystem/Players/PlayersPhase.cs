using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Contains the logic for the players' turn.
    /// The logic is separated to multiple sub-phases.
    /// NOTE: Multiplayer related logic will be implemented later, so we can assume that there is only one player.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Players Phase", fileName = "Phase_Players", order = 0)]
    public class PlayersPhase : GamePhase
    {
        [SerializeField]
        private List<GamePhase> _subPhases;

        public override string Name => "Players' Phase";


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