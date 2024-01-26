using UnityEngine;

public class Die : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float time;
    private int count = 0;

    public Die(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        time = Time.time;
        count = 0;

        status.lili.mouthRenderer.mesh = status.lili.mouthNormalAndDeath[1];
        status.lili.eyeRenderer.mesh = status.lili.eyeNormalAndDeath[1];

        inputHandler.input = new Vector2(0, 0);
        status.uiController.SetPositionFocusUI(Direction.Default);

        status.input.isInteracting = false;
        status.player.sword.shouldCheck = false;
        status.player.animations.PlayAnimation(status.player.animations.DIE,
    status.isSafeZone);

        if (inputHandler.isCarrying)
        {
            inputHandler.carryObject.SetDefaultPosition();
        }
    }

    public void OnExit()
    {
        status.lili.mouthRenderer.mesh = status.lili.mouthNormalAndDeath[0];
        status.lili.eyeRenderer.mesh = status.lili.eyeNormalAndDeath[0];

        inputHandler.isFalling = false;
        inputHandler.SetDefaultConfigurations();
        inputHandler.SetGroundConfiguration(status.isSafeZone);
    }

    public void Tick()
    {
        if (Time.time >= time + status.player.dieClipDuration && count == 0)
        {
            count = 1;
            status.DealGhost();
        }
    }
}
