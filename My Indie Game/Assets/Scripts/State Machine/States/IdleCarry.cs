using UnityEngine;

public class IdleCarry : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float fallTime;
    private bool shouldCheckFalling = true;

    public IdleCarry(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        inputHandler.isFalling = false;
        status.player.sword.shouldCheck = false;
        inputHandler.hasEndedLanding = false;
        shouldCheckFalling = true;
        status.player.animations.animator.Play("CarryMoveIdle");
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.jumpCount = 0;
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.ApplyAllMovement();
        inputHandler.SearchForEnemySpawner();
        CheckIfIsFalling();
    }

    void CheckIfIsFalling()
    {
        if (inputHandler.IsGrounded()) return;

        if (!inputHandler.IsGrounded() && shouldCheckFalling)
        {
            shouldCheckFalling = false;
            fallTime = Time.time;
        }
        else if (!inputHandler.IsGrounded() && !shouldCheckFalling)
        {
            if (Time.time - fallTime > inputHandler.fallingDuration)
            {
                inputHandler.isFalling = true;
            }
        }
        else
        {
            return;
        }
    }
}
