namespace Gameplay.Characters
{
    public class StrongEnemy : Enemy
    {
        public override int CalculateAttackDamage()
        {
            return DefaultDamage * 2;
        }
    }
}