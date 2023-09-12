using UnityEngine;

public class DoubleJumpig : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float jumpTime;

    public DoubleJumpig(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }


    public void OnEnter()
    {
        inputHandler.isFalling = false;
        jumpTime = Time.time;
        inputHandler.jumpCount++;
        inputHandler.hasPressedJumpButton = false;
        status.player.animations.PlayAnimation(status.player.animations.DOUBLE_JUMP,
    status.isSafeZone);
        inputHandler.Jump(status.player.jumpMovingHight * 1.2f);
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

        if (Time.time >= jumpTime + status.player.doubleJumpClipDuration)
        {
            inputHandler.jumpCount = 0;
        }
    }

}
