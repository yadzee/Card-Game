using Gameplay.Cards;
using UnityEngine;

namespace UI
{
    public class UiCardLink : MonoBehaviour
    {
        private Card _card;
        public Card Card => _card;

        public void Initialize(Card card)
        {
            _card = card;
        }
    }
}