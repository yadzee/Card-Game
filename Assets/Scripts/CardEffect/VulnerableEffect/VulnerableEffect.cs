using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableEffect", menuName = "Scriptable Objects/VulnerableEffect")]
public class VulnerableEffect : CardEffect
{
    [SerializeField] private int _vulnerable;

    public override void Execute(Player player, Enemy enemy)
    {
        enemy.ApplyVulnerable(_vulnerable);
    }
}
