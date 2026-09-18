using System.Collections.Generic;

public class Card
{
    private CardData _cardData;

    public string Name => _cardData.Name;
    public int EnergyCost => _cardData.EnergyCost;
    public CardType CardType => _cardData.CardType;
    public IReadOnlyList<CardEffect> CardEffects => _cardData.CardEffects;

    public Card(CardData cardData)
    {
        _cardData = cardData;
    }
}
