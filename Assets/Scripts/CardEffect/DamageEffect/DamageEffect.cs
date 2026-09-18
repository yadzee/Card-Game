using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Scriptable Objects/DamageEffect")]
public class DamageEffect : CardEffect
{
    [SerializeField] private int _damage;

    public override void Execute(Player player, Enemy enemy)
    {
        enemy.TakeDamage(_damage);
    }
}