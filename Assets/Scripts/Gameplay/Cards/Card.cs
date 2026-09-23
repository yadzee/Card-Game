using System.Collections.Generic;
using Gameplay.Cards.Effects;

namespace Gameplay.Cards
{
    public class Card
    {
        private readonly CardData _cardData;

        public string Name => _cardData.Name;
        public int EnergyCost => _cardData.EnergyCost;
        public CardType CardType => _cardData.CardType;
        public IReadOnlyList<CardEffect> CardEffects => _cardData.CardEffects;

        public Card(CardData cardData)
        {
            _cardData = cardData;
        }
    }
}