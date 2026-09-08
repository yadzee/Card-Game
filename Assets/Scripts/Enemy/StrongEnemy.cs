using UnityEngine;

public class StrongEnemy : Enemy
{
    public override int Attack()
    {
        Debug.Log("Strong Enemy attacks");
        return DefaultDamage * 2;
    }
}
