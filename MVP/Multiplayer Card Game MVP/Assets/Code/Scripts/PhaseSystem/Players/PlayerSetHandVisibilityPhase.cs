using System;
using System.Collections;
using Cards;
using UnityEngine;

namespace PhaseSystem.Players
{
    /// <summary>
    /// Sets if the player can see their hand.
    /// </summary>
    [CreateAssetMenu(menuName = "Phases/Player/Set Hand (Cards) Visibility", fileName = "Phase_Players_Hand_SetVisibility", order = 0)]
    public class PlayerSetHandVisibilityPhase : GamePhase
    {
        private enum HandVisibility
        {
            Show,
            Hide
        }

        [SerializeField]
        private HandVisibility _visibility;


        protected override IEnumerator OnExecute()
        {
            switch (_visibility)
            {
                case HandVisibility.Show:
                    PlayerHandManager.Instance.ShowHand();
                    break;
                case HandVisibility.Hide:
                    PlayerHandManager.Instance.HideHand();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return null;
        }
    }
}