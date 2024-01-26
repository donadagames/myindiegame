using UnityEngine;

public class Rebirth : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;

    private float time;

    public Rebirth(Status _satus, InputHandler _inputHandler)
    {
        status = _satus;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        time = Time.time;
        inputHandler.input = new Vector2(0, 0);
        status.uiController.SetPositionFocusUI(Direction.Default);
        inputHandler.SetGroundConfiguration(status.isSafeZone);
        status.RecoverEnergy(status.energy);
        status.RecoverHealth(status.health);
        status.isAlive = true;
        inputHandler.isInteracting = false;
        status.player.sword.shouldCheck = false;
        status.player.animations.PlayAnimation(status.player.animations.REBIRTH, status.isSafeZone);

        if (inputHandler.isCarrying)
        {
            inputHandler.carryObject.SetDefaultPosition();
        }
    }

    public void OnExit()
    {
        inputHandler.isRebirth = false;
        inputHandler.isFalling = false;
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        if (Time.time >= time + status.player.rebirthClipDuration)
        {
            inputHandler.isRebirth = false;
        }
    }
}
