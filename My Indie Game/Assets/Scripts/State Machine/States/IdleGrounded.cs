using UnityEngine;

public class IdleGrounded : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float fallTime;
    private bool shouldCheckFalling = true;
    public IdleGrounded(Status _status, InputHandler _inputHandler)
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
        status.player.animations.PlayAnimation(status.player.animations.IDLE,
            status.isSafeZone);
    }
  
    public void OnExit()
    {
        return;
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
