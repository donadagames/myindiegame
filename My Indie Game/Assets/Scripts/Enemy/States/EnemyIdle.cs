public class EnemyIdle : IState
{
    private readonly EnemySpawner spawner;

    public EnemyIdle(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    public void OnEnter()
    {
        spawner.enemy.ui.healthBar.SetActive(false);
        spawner.enemy.animator.Play(spawner.enemy.IDLE);

        if (spawner.status.enemies.Contains(spawner.enemy))
        {
            spawner.status.enemies.Remove(spawner.enemy);
        }
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        spawner.enemy.GetPlayerDistance(spawner.status.player.transform);
        spawner.enemy.FaceTarget(spawner.status.player.transform);
    }
}
