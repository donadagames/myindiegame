using UnityEngine;

public class Moving : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;

    private float fallTime;
    private bool shouldCheckFalling = true;
    private float time;
    public Moving(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }
    public void OnEnter()
    {
        time = Time.time;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        shouldCheckFalling = true;
        status.player.animations.PlayAnimation(status.player.animations.MOVE,
            status.isSafeZone);
        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.isFalling = false;
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.ApplyAllMovement();
        CheckIfIsFalling();

        if (inputHandler.stateMachine.shouldChange)
        {
            status.player.animations.PlayAnimation(status.player.animations.MOVE,
        status.isSafeZone);
            inputHandler.stateMachine.shouldChange = false;
        }

        if (Time.time > time + .25f)
        {
            inputHandler.DetectWater();
        }
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
