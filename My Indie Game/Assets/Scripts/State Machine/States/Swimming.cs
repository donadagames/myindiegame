public class Swimming : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;
    public Swimming(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {

        inputHandler.SetSwimmingConfiguration();
        status.player.sword.shouldCheck = false;
        inputHandler.direction.y = 0;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        status.player.animations.PlayAnimation(status.player.animations.SWIM,
            status.isSafeZone);
    }

    public void OnExit()
    {
        inputHandler.isFalling = false;
        inputHandler.SetDefaultConfigurations();
        inputHandler.SetGroundConfiguration(status.isSafeZone);

    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.ApplyAllMovement();
        inputHandler.SearchForEnemySpawner();
        inputHandler.DetectWater();
    }
}
