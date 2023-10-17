using UnityEngine;

public class LevelUp : IState
{
    private readonly Status status;

    private float time;

    public LevelUp(Status _status)
    {
        status = _status;
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
    }

    public void Tick()
    {
        if (Time.time > time + status.player.levelUpClipDuration)
        {
            status.isLevelUp = false;
        }
    }
}
