using UnityEngine;

namespace Gameplay.Characters
{
    public class BasicEnemy : Enemy
    {
        public override int Attack()
        {
            Debug.Log("Basic Enemy attacks");
            return DefaultDamage + 3;
        }
    }
}