using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    protected readonly int DefaultDamage = 5;
    private int _currentHealth;

    public int CurrentHealth    
    {
        get => _currentHealth;
        private set
        {
            if (value >= 0 && value <= _maxHealth)
                _currentHealth = value;
            else
            {
                Debug.LogError("value is out of range in CurrentHealth");
            }
        }
    }
    private int _maxHealth;

    public abstract int Attack();


    private void Start()
    {
        _maxHealth = 30;
        CurrentHealth = _maxHealth; 
    }


    public void TakeDamage(int damage)
    {
        if (damage > CurrentHealth)
            CurrentHealth = 0;
        else
        {
            CurrentHealth -= damage;
        }
        Debug.Log($"Enemy has taken {damage} damage");
    }

    public void Heal(int heal)

    {
        CurrentHealth += heal;
        if (CurrentHealth > _maxHealth)
            CurrentHealth = _maxHealth;
        Debug.Log($"Enemy has restored {heal} heal points");
    }
    
}

