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

        inputHandler.LandPuff();
        status.player.animations.PlayAnimation(status.player.animations.LAND,
          status.isSafeZone);
    }

    public void OnExit()
    {
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        inputHandler.GetDirection();

        if (Time.time >= landTime + status.player.landClipDuration)
        {
            inputHandler.hasEndedLanding = true;
        }
    }
}
