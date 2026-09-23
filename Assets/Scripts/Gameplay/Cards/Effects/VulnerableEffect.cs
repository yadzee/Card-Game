using Gameplay.Characters;
using UnityEngine;

namespace Gameplay.Cards.Effects
{
    [CreateAssetMenu(fileName = "VulnerableEffect", menuName = "Scriptable Objects/VulnerableEffect")]
    public class VulnerableEffect : CardEffect
    {
        [SerializeField] private int vulnerable;

        public override void Execute(Player player, Enemy enemy)
        {
            enemy.ApplyVulnerable(vulnerable);
        }
    }
}