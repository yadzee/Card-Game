using System.Collections.Generic;
using Gameplay.Cards.Effects;
using UnityEngine;

namespace Gameplay.Cards
{
    public class Card
    {
        private readonly CardData _cardData;

        public string Name => _cardData.Name;
        public int EnergyCost => _cardData.EnergyCost;
        public CardType CardType => _cardData.CardType;
        public IReadOnlyList<CardEffect> CardEffects => _cardData.CardEffects;
        public Sprite Sprite => _cardData.Sprite;

        public Card(CardData cardData)
        {
            _cardData = cardData;
        }
    }
}