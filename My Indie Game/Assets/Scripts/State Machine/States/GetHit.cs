using UnityEditor.Rendering;
using UnityEngine;

public class GetHit : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float time;

    public GetHit(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;
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
