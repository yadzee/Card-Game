using System;
using UnityEngine;

namespace Gameplay.Characters
{
    public class Player : MonoBehaviour
    {
        private int _maxHealth;
        public int MaxHealth
        {
            get => _maxHealth;
            private set
            {
                _maxHealth = value >= 0 ? value : 0;
            }
        }
        private int _currentHealth;
        
        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value >= 0 ? value : 0;
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }
        private bool _isDead;
        private bool _isEnergyExhausted;
        private const int EnergyMax = 3;

        private int _currentEnergy;

        public int CurrentEnergy
        {
            get => _currentEnergy;
            private set
            {
                _currentEnergy = value >= 0 ? value : 0;
                OnEnergyChanged?.Invoke(_currentEnergy);
            }
        }

        private int _block;

        public int Block
        {
            get => _block;
            private set => _block = value >= 0 ? value : 0;
        }
        
        public event Action<int> OnEnergyChanged;
        public event Action<int> OnHealthChanged;
        public event Action OnDeath;
        


        private void Awake()
        {
            MaxHealth = 30;
            CurrentHealth = MaxHealth;
            CurrentEnergy = EnergyMax;
            Block = 10;
            _isDead = false;
            _isEnergyExhausted = false;

            Debug.Log("Player is alive");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_isDead)
                {
                    TakeDamage(4);
                    Debug.Log("damage has taken");
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!_isEnergyExhausted)
                {
                    Debug.Log("SpendEnergy ");
                    SpendEnergy(1);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                RestoreEnergy(1);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetBlock();
            }
        }

        public void TakeDamage(int damage)
        {
            if (!_isDead)
            {
                var currentDamage = damage;

                currentDamage -= Block;
                Block -= damage;

                if (currentDamage <= 0)
                {
                    currentDamage = 0;
                }


                Debug.Log("currentdamage " + currentDamage);
                Debug.Log("block " + Block);

                if (Block == 0)
                {
                    CurrentHealth -= currentDamage;
                    Debug.Log("Player HP: " + CurrentHealth);
                }

                else
                {
                    Debug.Log("Blocked");
                }

                CheckHealth();
            }
        }

        private void CheckHealth()
        {
            if (CurrentHealth <= 0 && !_isDead)
            {
                _isDead = true;
                OnDeath?.Invoke();
                Debug.Log("Player is dead");
            }
        }

        public void ResetBlock()
        {
            Block = 0;
            Debug.Log("ResetBlock. Current Block: " + Block);
        }

        public void GainBlock(int amount)
        {
            Block += amount;
            Debug.Log($"Gain {amount} Block. Current Block: {Block}");
        }

        public void SpendEnergy(int amount)
        {
            if (CurrentEnergy > 0 && amount <= CurrentEnergy)
            {
                CurrentEnergy -= amount;
            }

            if (CurrentEnergy == 0)
            {
                _isEnergyExhausted = true;
                Debug.Log("Energy Exhausted");
            }
        }

        private void RestoreEnergy(int amount)
        {
            CurrentEnergy += amount;
            if (CurrentEnergy > 0)
            {
                _isEnergyExhausted = false;
            }

            Debug.Log("RestoreEnergy " + CurrentEnergy);
        }

        public void ResetEnergy()
        {
            CurrentEnergy = EnergyMax;
            _isEnergyExhausted = false;
            Debug.Log("Energy reset: " + CurrentEnergy);
        }
    }
}