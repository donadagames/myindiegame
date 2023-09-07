using UnityEngine;

public class JumpingInPlace : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float jumpTime;
    public JumpingInPlace(Status _status, InputHandler _inputHandler)
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
        status.player.animator.Play(Animation.JumpInPlace_Unarmed.ToString());
        inputHandler.Jump(status.player.jumpInPlaceHight);
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

        if (Time.time >= jumpTime + status.player.jumpClipDuration)
        {
            inputHandler.jumpCount = 0;
        }
    }
}
