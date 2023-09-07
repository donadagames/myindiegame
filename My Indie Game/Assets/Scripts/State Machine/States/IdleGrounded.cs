using UnityEngine;

public class IdleGrounded : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    public IdleGrounded(Status _status, InputHandler _inputHandler)
    { 
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        inputHandler.hasLanded = false;
        status.player.animator.Play(Animation.
            Idle_Unarmed.ToString());
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        inputHandler.GetInput();
    }
}
