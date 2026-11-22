namespace Gameplay.Characters
{
    public class BasicEnemy : Enemy
    {
        public override int CalculateAttackDamage()
        {
            return DefaultDamage + 3;
        }
    }
}