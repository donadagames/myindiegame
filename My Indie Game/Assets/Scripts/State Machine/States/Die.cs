using UnityEngine;

public class Die : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public Die(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.input.isInteracting = false;
        status.player.sword.shouldCheck = false;
        status.player.animations.PlayAnimation(status.player.animations.DIE,
    status.isSafeZone);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
