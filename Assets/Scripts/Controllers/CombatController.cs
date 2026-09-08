using UnityEngine;

public class CombatController: MonoBehaviour
{
     private Turns _curretTurn;
     [SerializeField] private Player _player;
     [SerializeField] private Enemy _enemy;

    private void Start()
    {
        PlayerTurn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            EndPlayerTurn();
    }
    
 
    public void EndPlayerTurn()
    {
        if (_curretTurn == Turns.PlayerTurn)
        {
            EnemyTurn();
            _player.EnemyStrikesMe(_enemy);
            PlayerTurn();
        }
    }

    public void PlayerTurn()
    {
        // _endTurn = false;
        _player.ResetEnergy();
        _curretTurn = Turns.PlayerTurn;
        Debug.Log("Player turn" + _curretTurn);
    }
    
    public void EnemyTurn()
    {
         // _endTurn = false;
        _curretTurn = Turns.EnemyTurn;
        Debug.Log("Enemy turn");
    }
    
    private enum Turns
    {
        PlayerTurn,
        EnemyTurn
    }
}
