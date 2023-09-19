using UnityEngine;

public class Floating : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public Floating(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;
        inputHandler.SetSwimmingConfiguration();
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        status.player.animations.PlayAnimation(status.player.animations.FLOAT,
            status.isSafeZone);
    }

    public void OnExit()
    {
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.DetectWater();
    }
}
