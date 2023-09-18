using UnityEngine;

public class Falling : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public Falling(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.animations.PlayAnimation(status.player.animations.FALL,
            status.isSafeZone);
    }

    public void OnExit()
    {
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();
        inputHandler.DetectWater();
        inputHandler.SearchForEnemySpawner();
    }
}
