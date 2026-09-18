using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
[SerializeField] private string _name;
public string Name => _name;

[SerializeField] private int _energyCost;
public int EnergyCost => _energyCost;
[SerializeField] private CardType _cardType;
public CardType CardType => _cardType;

[SerializeField] private List<CardEffect> _cardEffects;
public IReadOnlyList <CardEffect> CardEffects => _cardEffects;
  
}

public enum CardType
{
    Attack,
    Skill,
    Power,
}
