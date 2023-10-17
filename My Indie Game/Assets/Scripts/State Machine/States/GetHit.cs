using UnityEngine;

public class GetHit : IState
{
    private readonly Status status;

    private float time;

    public GetHit(Status _status)
    {
        status = _status;
    }

    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;
        status.input.isInteracting = false;
        time = Time.time;
        status.player.animations.PlayAnimation(status.player.animations.GETHIT,
            status.isSafeZone);
    }

    public void OnExit()
    {
        status.isDamaged = false;
    }

    public void Tick()
    {
        if (Time.time > time + status.player.getHitClipDuration)
        {
            status.isDamaged = false;
        }
    }
}
