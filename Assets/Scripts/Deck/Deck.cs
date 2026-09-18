using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> _drawPile;
    private List<Card> _discardPile;
    private List<Card> _handDeck;
    
    private const int HandMaxCapacity = 10;
    public IReadOnlyList<Card> Hand => _handDeck;

    public CardData strikeCardData;
    public CardData defenceCardData;
    public CardData bashCardData;


    private void Awake()
    {
        _drawPile = new List<Card>();
        _discardPile = new List<Card>();
        _handDeck = new List<Card>();
        
        for (int i = 0; i < 4; i++)
        {
            _drawPile.Add(new Card(defenceCardData));
        }
        for (int i = 0; i < 5; i++)
        {
            _drawPile.Add(new Card(strikeCardData));
        }
        _drawPile.Add(new Card(bashCardData));  
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    private Card DrawUpperCard()
    {
        if (_handDeck.Count >= HandMaxCapacity)
        {
            Debug.Log("Reached maximum hand capacity");
            return null;
        }
        // AddCardToHand() тоже проверяет вместимость руки.
        // Позже решим, нужна ли эта проверка в обоих местах
        
        if (_drawPile.Count == 0)
        {
            if (_discardPile.Count == 0)
            {
                Debug.Log("No cards available to draw.");
                return null;
            }
            ShuffleDiscardIntoDrawPile();
        }
        Card upperCard = _drawPile[_drawPile.Count - 1];
        _drawPile.RemoveAt(_drawPile.Count - 1);
        AddCardToHand(upperCard);
        return upperCard;
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawUpperCard();
        }
        Debug.Log($"Draw {amount} cards");
        Debug.Log($"In drawPile remain {_drawPile.Count} cards");
    }

    public void DiscardHand()
    {
        _discardPile.AddRange(_handDeck);
        _handDeck.Clear();
        Debug.Log($"In discardPile remain  {_discardPile.Count} cards");
    }
    
    public void DiscardCard(Card card)
    {
        if (!_handDeck.Remove(card))
        {
            Debug.Log("Card is not in hand!");
            return;
        }

        _discardPile.Add(card);
    }

    private bool AddCardToHand(Card card)
    {
        if (_handDeck.Count >= HandMaxCapacity)
        {
            Debug.Log("Hand is full!");
            return false;
        }

        _handDeck.Add(card); 
        return true;
    }

    private void ShuffleDiscardIntoDrawPile()
    {
        for (int i = _discardPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i +1);
            
            var temp = _discardPile[i];
            _discardPile[i] = _discardPile[randomIndex];
            _discardPile[randomIndex] = temp;
        }
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();
        Debug.Log("Shuffling discard into draw pile");
    }
}
