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
        status.player.animator.Play(Animation.DoubleJump_Unarmed.ToString());
        inputHandler.Jump(status.player.jumpMovingHight);
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.jumpCount = 0;
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();

        if (Time.time >= jumpTime + status.player.doubleJumpClipDuration)
        {
            inputHandler.jumpCount = 0;
        }
    }

}
