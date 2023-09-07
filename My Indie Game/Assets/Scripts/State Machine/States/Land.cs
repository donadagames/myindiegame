using UnityEngine;

public class Land : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;

    private float landTime;

    public Land(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        landTime = Time.time;
        inputHandler.hasEndedLanding = false;
        //VFX Land "Puff"

        status.player.animator.Play(Animation.
            Land_Unarmed.ToString());
    }

    public void OnExit()
    {
        inputHandler.hasEndedLanding = false;
    }

    public void Tick()
    {
        if (Time.time >= landTime + status.player.landClipDuration)
        {
            inputHandler.hasEndedLanding = true;
        }
    }
}
