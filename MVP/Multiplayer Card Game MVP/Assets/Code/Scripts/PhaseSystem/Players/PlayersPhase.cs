using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Contains the logic for the players' turn.
    /// The logic is separated to multiple sub-phases.
    /// NOTE: Multiplayer related logic will be implemented later, so we can assume that there is only one player.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Players Phase", fileName = "Phase_Players", order = 0)]
    public class PlayersPhase : GamePhaseCollection
    {
    }
}