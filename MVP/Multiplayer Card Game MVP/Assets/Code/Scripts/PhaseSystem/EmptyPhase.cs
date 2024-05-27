using System.Collections;
using UnityEngine;

namespace PhaseSystem
{
    [CreateAssetMenu(menuName = "Phases/Empty", fileName = "Empty Phase", order = 0)]
    public class EmptyPhase : GamePhase
    {
        protected override IEnumerator OnExecute() => null;
    }
}