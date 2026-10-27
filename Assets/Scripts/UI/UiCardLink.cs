using Gameplay.Cards;
using UnityEngine;
using Tools.UI.Card;

namespace UI
{
    public class UiCardLink : MonoBehaviour
    {
        private Card _card;
        public Card Card => _card;

        public void Initialize(Card card)
        {
            _card = card;

            UiTexturePicker texturePicker =
                GetComponentInChildren<UiTexturePicker>();

            if (texturePicker != null)
                texturePicker.SetSprite(card.Sprite);
        }
    }
}