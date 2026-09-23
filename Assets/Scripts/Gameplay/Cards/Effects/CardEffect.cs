using Gameplay.Characters;
using UnityEngine;

namespace Gameplay.Cards.Effects
{
    public abstract class CardEffect : ScriptableObject
    {
        public abstract void Execute(Player player, Enemy enemy);
    }
}