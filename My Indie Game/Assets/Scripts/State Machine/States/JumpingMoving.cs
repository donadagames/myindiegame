using UnityEngine;

public class JumpingMoving : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float jumpTime;

    public JumpingMoving(Status _status, InputHandler _inputHandler)
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
        status.player.animations.PlayAnimation(status.player.animations.JUMP_MOVING,
    status.isSafeZone);
        inputHandler.Jump(status.player.jumpMovingHight);
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.jumpCount = 0;
        inputHandler.canMeleeAttack = true;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;
        inputHandler.canMagicAttack = true;
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();
        inputHandler.DetectWater();
        inputHandler.SearchForEnemySpawner();

        if (Time.time >= jumpTime + status.player.jumpClipDuration)
        {
            inputHandler.jumpCount = 0;
        }
    }
}
