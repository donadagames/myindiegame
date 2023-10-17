public class EnemyDizzy : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner;

    public EnemyDizzy(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        spawner.enemy.animator.Play(spawner.enemy.DIZZY);
    }
    public void OnExit()
    {
    }
    public void Tick()
    {
    }
}
