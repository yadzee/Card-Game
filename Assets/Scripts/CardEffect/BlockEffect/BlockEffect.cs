using UnityEngine;

[CreateAssetMenu(fileName = "BlockEffect", menuName = "Scriptable Objects/BlockEffect")]
public class BlockEffect: CardEffect
{
    [SerializeField] private int _block;
    public override void Execute(Player player, Enemy enemy)
    {
        player.GainBlock(_block);
    }
    
    public void ExecuteMultiplier(Player player, Enemy enemy, float multiplier)
    {
        player.GainBlock(Mathf.FloorToInt(_block * multiplier));
    }
}
