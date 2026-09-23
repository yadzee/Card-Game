using Gameplay.Characters;
using UnityEngine;

namespace Gameplay.Cards.Effects
{
    [CreateAssetMenu(fileName = "BlockEffect", menuName = "Scriptable Objects/BlockEffect")]
    public class BlockEffect : CardEffect
    {
        [SerializeField] private int block;

        public override void Execute(Player player, Enemy enemy)
        {
            player.GainBlock(block);
        }

        public void ExecuteMultiplier(Player player, Enemy enemy, float multiplier)
        {
            player.GainBlock(Mathf.FloorToInt(block * multiplier));
        }
    }
}