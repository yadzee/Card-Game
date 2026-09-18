

public class EnemyAction
{
    public EnemyIntent Intent { get; }
    public int Value { get; }

    public EnemyAction(EnemyIntent intent, int value)
    {
        Intent = intent;
        Value = value;
    }

}
