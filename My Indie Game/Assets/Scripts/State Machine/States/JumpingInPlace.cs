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
        jumpTime = Time.time;
        inputHandler.canJump = false;
        inputHandler.hasPressedJumpButton = false;
        status.player.animator.Play(Animation.JumpInPlace_Unarmed.ToString());
        inputHandler.Jump(status.player.jumpInPlaceHight);
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.canJump = true;
    }

    public void Tick()
    {
        inputHandler.ApplyGravity();
        inputHandler.ApplyMovement();

        if (Time.time >= jumpTime + status.player.jumpClipDuration)
        {
            inputHandler.canJump = true;
        }
    }
}
