using Gameplay.Characters;
using UnityEngine;

namespace Gameplay.Cards.Effects
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Scriptable Objects/DamageEffect")]
    public class DamageEffect : CardEffect
    {
        [SerializeField] private int damage;

        public override void Execute(Player player, Enemy enemy)
        {
            enemy.TakeDamage(damage);
        }

        public void ExecuteMultiplier(Player player, Enemy enemy, float multiplier)
        {
            enemy.TakeDamage(Mathf.FloorToInt(damage * multiplier));
        }
    }
}