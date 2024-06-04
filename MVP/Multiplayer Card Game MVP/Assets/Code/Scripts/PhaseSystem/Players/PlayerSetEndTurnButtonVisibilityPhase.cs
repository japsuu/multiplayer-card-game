using System.Collections;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Sets the end turn button visibility.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Set end turn button visibility", fileName = "Phase_EndTurnButton_SetVisibility", order = 0)]
    public class PlayerSetEndTurnButtonVisibilityPhase : GamePhase
    {
        [SerializeField]
        private bool _shouldBeVisible = true;


        protected override IEnumerator OnExecute()
        {
            GameLoopManager.SetShowEndTurnButton(_shouldBeVisible);

            yield return null;
        }
    }
}