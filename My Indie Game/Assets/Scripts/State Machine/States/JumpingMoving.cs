using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class JumpingMoving : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public float jumpTime;

    public JumpingMoving(Status _status, InputHandler _inputHandler)
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
        inputHandler.Jump(status.player.jumpMovingHight);
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.canJump = true;
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();

        if (Time.time >= jumpTime + status.player.jumpClipDuration)
        {
            inputHandler.canJump = true;
        }
    }
}
