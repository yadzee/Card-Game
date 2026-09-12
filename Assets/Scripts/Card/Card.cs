public class Card
{
    private CardData _cardData;
    public string Name => _cardData.Name;
    public int EnergyCost => _cardData.EnergyCost;
    public CardType CardType => _cardData.CardType;
    public int Damage => _cardData.Damage;
    public int Block => _cardData.Block;
   
    public Card(CardData cardData)
        {
        _cardData = cardData;
        }
}
