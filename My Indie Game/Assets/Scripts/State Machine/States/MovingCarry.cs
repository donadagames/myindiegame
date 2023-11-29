using UnityEngine;

public class MovingCarry : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float jumpTime;
    public MovingCarry(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        inputHandler.isFalling = false;
        jumpTime = Time.time;
        inputHandler.jumpCount++;

        inputHandler.SearchForInteractables();

        inputHandler.hasPressedJumpButton = false;
        status.player.animations.PlayAnimation(status.player.animations.JUMP_IN_PLACE,
     status.isSafeZone);
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
