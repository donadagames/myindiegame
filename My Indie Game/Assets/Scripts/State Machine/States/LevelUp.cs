using UnityEngine;

public class LevelUp : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;
    private float time;

    public LevelUp(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;
        status.input.isInteracting = false;
        time = Time.time;
        status.player.animations.PlayAnimation(status.player.animations.LEVELUP,
            status.isSafeZone);
    }

    public void OnExit()
    {
        status.isLevelUp = false;
        inputHandler.hasPressedJumpButton = false;
        inputHandler.jumpCount = 0;
        inputHandler.canMeleeAttack = true;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;
        inputHandler.canMagicAttack = true;
    }

    public void Tick()
    {
        if (Time.time > time + status.player.levelUpClipDuration)
        {
            status.isLevelUp = false;
        }
    }
}
