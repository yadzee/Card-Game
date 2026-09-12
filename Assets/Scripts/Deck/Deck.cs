using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> _drawPile;
    public CardData StrikeCardData;
    public CardData DefenceCardData;
    public CardData BashCardData;
    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _drawPile = new List<Card>();
        for (int i = 0; i < 5; i++)
        {
            _drawPile.Add(new Card(StrikeCardData));
        }
        for (int i = 0; i < 4; i++)
        {
            _drawPile.Add(new Card(DefenceCardData));
        }
        _drawPile.Add(new Card(BashCardData));
}

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Card DrawUpperCard()
    {
        if (_drawPile.Count == 0)
        {
            return null;
        }
        Card upperCard = _drawPile[_drawPile.Count - 1];
        _drawPile.RemoveAt(_drawPile.Count - 1);
        Debug.Log("Draw Upper Card");
        return upperCard;
    }
}
