using UnityEngine;

public class Falling : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public Falling(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        status.player.animator.Play(Animation.Falling_Unarmed.ToString());
    }

    public void OnExit()
    {
        inputHandler.jumpCount = 0;
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();
    }
}
