using Gameplay.Cards;
using TMPro;
using Tools.UI.Card;
using UnityEngine;

namespace UI
{
    public class UiCardLink : MonoBehaviour
    {
        private Card _card;
        public Card Card => _card;
        
        [SerializeField] private UiTexturePicker _texturePicker;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _energyText;

        public void Initialize(Card card)
        {
            _card = card;

            _nameText.text = card.Name;
            _descriptionText.text = card.Description;
            _energyText.text = card.EnergyCost.ToString();
            _texturePicker.SetSprite(card.Sprite);
        }
    }
}