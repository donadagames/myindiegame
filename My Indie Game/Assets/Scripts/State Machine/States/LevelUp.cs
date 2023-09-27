using System.Collections;
using System.Collections.Generic;
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
