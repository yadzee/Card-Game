using UnityEngine;

[CreateAssetMenu(fileName = "BlockEffect", menuName = "Scriptable Objects/BlockEffect")]
public class BlockEffect: CardEffect
{
    [SerializeField] private int _block;
    public override void Execute(Player player, Enemy enemy)
    {
        player.GainBlock(_block);
    }
}
