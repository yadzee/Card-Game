using UnityEngine;


public class Player : MonoBehaviour
{
  [SerializeField] 
  private int _maxHealth;
  [SerializeField] private int _currentHealth;
  [SerializeField] private bool _isDead;
  [SerializeField] private bool _isEnergyExhausted;
  private const int _energyMax = 3;
 
 
   
   [SerializeField] private int _currentEnergy;

   public int CurrentEnergy
   {
       get => _currentEnergy;
       set => _currentEnergy = value >= 0 ? value : 0;
   }
   
   [SerializeField] private int _block;
   public int Block
   {
       get => _block; 
       set => _block = value >= 0 ? value : 0; 
   }
   

    private void Start()
    {
        _maxHealth = 30;
        _currentHealth = _maxHealth;
        CurrentEnergy = _energyMax;
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
                SpendEnergy(5);}
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            RestoreEnergy(1);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBlock();  
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetBlock(); 
        }
        
    }

    private void TakeDamage(int damage)
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
            Debug.Log("block "+ Block);
       
            if (Block == 0)
            {
                _currentHealth -= currentDamage;
                Debug.Log("Player HP: " + _currentHealth);
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
     if (_currentHealth <= 0 && !_isDead)
     {
         _isDead = true;
         Debug.Log("Player is dead");
     }
    }
    
    private void ResetBlock()
    {
        Block = 0;
        Debug.Log("ResetBlock. Current Block: " + Block);
    }

    private void GetBlock()
    {
        Block = Block + 10;
        Debug.Log("GetBlock + 10. Current Block: " + Block);
    }

    private void SpendEnergy(int amount)
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
    
    public void RestoreEnergy(int amount)
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
        CurrentEnergy = _energyMax;
        _isEnergyExhausted = false;
        Debug.Log("Energy reset: " + CurrentEnergy);
    }

    public void EnemyStrikesMe(Enemy enemy)
    {
        int damageReceived = enemy.Attack();
            TakeDamage(damageReceived);
    }
    
    // private void SwitchTurn()
    // {
    //     if (turn == Turns.PlayerTurn)
    //     {
    //         turn = Turns.EnemyTurn;
    //         Debug.Log("Enemy's turn");
    //     }
    //     else
    //     {
    //         turn = Turns.PlayerTurn;
    //         Debug.Log("Player's turn");
    //     }
    // }
    
}
