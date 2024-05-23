using System.Collections;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Setup phase for the players.
    /// Apply any initial setup logic here, like tabled card bonuses, etc.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Players/Setup", fileName = "Phase_Players_Setup", order = 0)]
    public class PlayerSetupPhase : GamePhase
    {
        public override string Name => "Setup Phase";
        
        
        public override IEnumerator Execute()
        {
            yield return null;
        }
    }
}