using Cards.Tags;
using TMPro;
using UnityEngine;

namespace UI.Cards
{
    public class CardTagUIElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tagDescriptionText;
        
        
        public void Initialize(CardTag cardTag)
        {
            _tagDescriptionText.text = $"{cardTag.Name}: {cardTag.Description}";
        }
    }
}