using UnityEngine;

public class GetHit : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;
    private float time;

    public GetHit(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        this.inputHandler = _inputHandler;
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
        inputHandler.GetDirection();
        inputHandler.ApplyGravity();

        if (Time.time > time + status.player.getHitClipDuration)
        {
            status.isDamaged = false;
        }

        if (status.input.isHit)
        {
            if (status.input.impact.magnitude > 0.2F) 
                status.player.characterController.Move(status.input.impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            status.input.impact = Vector3.Lerp(status.input.impact, Vector3.zero, 5 * Time.deltaTime);
        }

    }
}
