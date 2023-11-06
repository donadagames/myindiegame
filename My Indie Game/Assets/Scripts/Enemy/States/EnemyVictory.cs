public class EnemyVictory : IState
{
    // Start is called before the first frame update
    private readonly EnemySpawner spawner;

    public EnemyVictory(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
    public void OnEnter()
    {
        spawner.enemy.isOnFire = false;
        spawner.enemy.isFreezed = false;
        spawner.enemy.isDizzy = false;
        spawner.enemy.onFire_VFX.SetActive(false);
        spawner.enemy.dizzy_VFX.SetActive(false);
        spawner.enemy.freezed_VFX.SetActive(false);
        spawner.enemy.ui.gameObject.SetActive(false);

        spawner.enemy.animator.Play(spawner.enemy.VICTORY);
    }
    public void OnExit()
    {
    }
    public void Tick()
    {
    }
}
