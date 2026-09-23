using UnityEngine;
using Gameplay.Combat;

namespace Gameplay.Characters
{
    public abstract class Enemy : MonoBehaviour
    {
        protected readonly int DefaultDamage = 5;
        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set => _currentHealth = value >= 0 ? value : 0;
        }

        private int _maxHealth;

        [SerializeField] private int block;

        public int Block
        {
            get => block;
            set => block = value >= 0 ? value : 0;
        }

        private int _vulnerable;

        public int Vulnerable
        {
            get => _vulnerable;
            private set => _vulnerable = value >= 0 ? value : 0;
        }

        [SerializeField] private EnemyIntent currentIntent;

        public abstract int Attack();


        private void Start()
        {
            _maxHealth = 30;
            CurrentHealth = _maxHealth;
        }


        public void TakeDamage(int damage)
        {
            if (Vulnerable > 0)
            {
                damage = Mathf.FloorToInt(damage * 1.5f);
            }

            var currentDamage = damage;

            currentDamage -= Block;
            Block -= damage;

            if (currentDamage <= 0)
            {
                currentDamage = 0;
            }

            if (Block == 0)
            {
                CurrentHealth -= currentDamage;
                Debug.Log("Enemy HP: " + CurrentHealth);
                Debug.Log($"Enemy has taken {currentDamage} damage");
            }
        }

        public void Heal(int heal)

        {
            CurrentHealth += heal;
            if (CurrentHealth > _maxHealth)
                CurrentHealth = _maxHealth;
            Debug.Log($"Enemy has restored {heal} heal points");
        }

        public void ApplyVulnerable(int amount)
        {
            Vulnerable += amount;
            Debug.Log($"Enemy gained {amount} Vulnerable. Current Vulnerable: {Vulnerable}");
        }

        public void ReduceVulnerable()
        {
            Vulnerable -= 1;
            Debug.Log($"Current reduced Vulnerable: {Vulnerable}");
        }

        public void ChooseNextIntent()
        {
            currentIntent = (EnemyIntent)UnityEngine.Random.Range(0, 2);
        }

        public EnemyAction ExecuteIntent()
        {
            switch (currentIntent)
            {
                case EnemyIntent.Attack:
                    return new EnemyAction(EnemyIntent.Attack, Attack());
                case EnemyIntent.Block:
                    return new EnemyAction(EnemyIntent.Block, 5);
                default:
                    return null;
            }
        }
    }
}